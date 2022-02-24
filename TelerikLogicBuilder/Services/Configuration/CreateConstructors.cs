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
    internal class CreateConstructors : ICreateConstructors
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEncryption _encryption;
        private readonly IXmlValidator _xmlValidator;

        public CreateConstructors(IConfigurationService configurationService, IXmlValidator xmlValidator, IContextProvider contextProvider)
        {
            _configurationService = configurationService;
            _pathHelper = contextProvider.PathHelper;
            _fileIOHelper = contextProvider.FileIOHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _encryption = contextProvider.Encryption;
            _xmlValidator = xmlValidator;
        }

        public XmlDocument Create()
        {
            try
            {
                string xmlString = CreateXml();
                ValidateXml(xmlString);
                SaveXml(xmlString);

                return _xmlDocumentHelpers.ToXmlDocument(xmlString);

                string CreateXml()
                {
                    StringBuilder stringBuilder = new();
                    using XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder);
                    xmlTextWriter.WriteElementString(XmlDataConstants.FORMELEMENT, string.Empty);
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();

                    return stringBuilder.ToString();
                }

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(SchemaName.ConstructorSchema, xmlString);
                    if (validationResponse.Success == false)
                        throw new CriticalLogicBuilderException(string.Join(Environment.NewLine, validationResponse.Errors));
                }

                void SaveXml(string xmlString)
                {
                    string fullPath = _pathHelper.CombinePaths
                    (
                        _configurationService.ProjectProperties.ProjectPath, 
                        ConfigurationFiles.Constructors
                    );

                    if (!Directory.Exists(_pathHelper.GetFilePath(fullPath)))
                        _fileIOHelper.CreateDirectory(_pathHelper.GetFilePath(fullPath));

                    _encryption.EncryptToFile(fullPath, xmlString);
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
                            ConfigurationFiles.Constructors
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
