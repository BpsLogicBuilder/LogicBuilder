using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System;
using System.IO;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class UpdateFunctions : IUpdateFunctions
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IEncryption _encryption;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;
        private readonly IExceptionHelper _exceptionHelper;

        public UpdateFunctions(IConfigurationService configurationService, IXmlValidator xmlValidator, IContextProvider contextProvider)
        {
            _configurationService = configurationService;
            _pathHelper = contextProvider.PathHelper;
            _fileIOHelper = contextProvider.FileIOHelper;
            _encryption = contextProvider.Encryption;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _xmlValidator = xmlValidator;
        }

        public void Update(XmlDocument xmlDocument)
        {
            try
            {
                if (xmlDocument.DocumentElement == null)
                    throw _exceptionHelper.CriticalException("{6BD83CD7-1313-4F59-AAFE-EA674876EEBB}");

                ValidateXml(xmlDocument.DocumentElement.OuterXml);
                SaveXml(xmlDocument);

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

                    if (xmlDocument.DocumentElement == null)
                        throw _exceptionHelper.CriticalException("{8DAF378E-9690-4AE5-A789-D8110404535E}");

                    XmlDocument saveDocument = _xmlDocumentHelpers.ToXmlDocument(xmlDocument.DocumentElement.OuterXml);
                    saveDocument.DocumentElement!.RemoveChild/*Not null if loaded using XmlDocumentHelpers.ToXmlDocument.*/
                    (
                        saveDocument.SelectSingleNode
                        (
                            $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']"
                        )!/*This node should always be present in the calling code.*/
                    );

                    ValidateXml(saveDocument.DocumentElement.OuterXml);

                    _encryption.EncryptToFile(fullPath, saveDocument.DocumentElement.OuterXml);
                }
            }
            catch (XmlException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }
            catch (LogicBuilderException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }
        }
    }
}
