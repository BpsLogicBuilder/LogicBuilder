using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class CreateFunctions : ICreateFunctions
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEncryption _encryption;
        private readonly IXmlValidator _xmlValidator;
        private readonly IBuiltInFunctionsLoader _builtInFunctionsLoader;

        public CreateFunctions(IConfigurationService configurationService, IXmlValidator xmlValidator, IBuiltInFunctionsLoader builtInFunctionsLoader, IContextProvider contextProvider)
        {
            _configurationService = configurationService;
            _pathHelper = contextProvider.PathHelper;
            _fileIOHelper = contextProvider.FileIOHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _encryption = contextProvider.Encryption;
            _xmlValidator = xmlValidator;
            _builtInFunctionsLoader = builtInFunctionsLoader;
        }

        public XmlDocument Create()
        {
            try
            {
                string xmlString = CreateXml();
                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
                AppendBuildtInFunctions(xmlDocument);
                ValidateXml(xmlDocument.DocumentElement.OuterXml);

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
                    => xmlDocument.DocumentElement.AppendChild
                    (
                        _xmlDocumentHelpers.MakeFragment
                        (
                            xmlDocument,
                            _builtInFunctionsLoader.Load().DocumentElement.OuterXml
                        )
                    );

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(SchemaName.FunctionsSchema, xmlString);
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

                    XmlDocument saveDocument = _xmlDocumentHelpers.ToXmlDocument(xmlDocument.DocumentElement.OuterXml);
                    saveDocument.DocumentElement.RemoveChild
                    (
                        saveDocument.SelectSingleNode
                        (
                            $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']"
                        )
                    );

                    ValidateXml(saveDocument.DocumentElement.OuterXml);

                    _encryption.EncryptToFile(fullPath, saveDocument.DocumentElement.OuterXml);
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
