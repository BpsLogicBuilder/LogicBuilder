using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class CreateFunctions : ICreateFunctions
    {
        private readonly IBuiltInFunctionsLoader _builtInFunctionsLoader;
        private readonly IConfigurationService _configurationService;
        private readonly IEncryption _encryption;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;

        public CreateFunctions(
            IBuiltInFunctionsLoader builtInFunctionsLoader,
            IConfigurationService configurationService,
            IEncryption encryption,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _builtInFunctionsLoader = builtInFunctionsLoader;
            _configurationService = configurationService;
            _encryption = encryption;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.FunctionsSchema);
        }

        public XmlDocument Create()
        {
            try
            {
                string xmlString = CreateXml();
                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
                AppendBuildtInFunctions(xmlDocument);
                ValidateXml(_xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml);

                SaveXml(xmlDocument);

                return xmlDocument;

                string CreateXml()
                {
                    StringBuilder stringBuilder = new();
                    using XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.FORMSELEMENT);
                        xmlTextWriter.WriteStartElement(XmlDataConstants.FORMELEMENT);
                            xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, XmlDataConstants.FUNCTIONSFORMROOTNODENAME);
                            xmlTextWriter.WriteStartElement(XmlDataConstants.FOLDERELEMENT);
                                xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, XmlDataConstants.FUNCTIONSROOTFOLDERNAMEATTRIBUTE);
                            xmlTextWriter.WriteEndElement();
                        xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();

                    return stringBuilder.ToString();
                }

                void AppendBuildtInFunctions(XmlDocument xmlDocument) 
                    => _xmlDocumentHelpers.GetDocumentElement(xmlDocument).AppendChild
                    (
                        _xmlDocumentHelpers.MakeFragment
                        (
                            xmlDocument,
                            _xmlDocumentHelpers.GetDocumentElement(_builtInFunctionsLoader.Load()).OuterXml
                        )
                    );

                void RemoveBuildtInFunctions(XmlDocument saveDocument)
                {
                    _xmlDocumentHelpers.GetDocumentElement(saveDocument).RemoveChild
                    (
                        _xmlDocumentHelpers.SelectSingleElement
                        (
                            saveDocument,
                            $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']"
                        )
                    );
                }

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(xmlString);
                    if (validationResponse.Success == false)
                        throw new CriticalLogicBuilderException(string.Join(Environment.NewLine, validationResponse.Errors));
                }

                void SaveXml(XmlDocument xmlDocument)
                {
                    string fullPath = _pathHelper.CombinePaths
                    (
                        _configurationService.ProjectProperties.ProjectPath, 
                        ConfigurationFiles.Functions
                    );

                    if (!Directory.Exists(_pathHelper.GetFilePath(fullPath)))
                        _fileIOHelper.CreateDirectory(_pathHelper.GetFilePath(fullPath));

                    XmlDocument saveDocument = _xmlDocumentHelpers.ToXmlDocument(_xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml);
                    RemoveBuildtInFunctions(saveDocument);

                    string xmlToSave = _xmlDocumentHelpers.GetDocumentElement(saveDocument).OuterXml;
                    ValidateXml(xmlToSave);
                    _encryption.EncryptToFile(fullPath, xmlToSave);
                    _fileIOHelper.SaveFile
                    (
                        $"{fullPath}{FileExtensions.XMLFILEEXTENSION}",
                        _xmlDocumentHelpers.GetXmlString(xmlToSave)
                    );
                }
            }
            catch (XmlException ex)
            {
                throw new CriticalLogicBuilderException
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.invalidConfigurationDocumentFormat,
                        _pathHelper.CombinePaths
                        (
                            _configurationService.ProjectProperties.ProjectPath,
                            ConfigurationFiles.Functions
                        )
                    ),
                    ex
                );
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message);
            }
        }
    }
}
