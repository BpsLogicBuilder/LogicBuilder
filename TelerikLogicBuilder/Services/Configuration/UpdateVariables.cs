using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.IO;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class UpdateVariables : IUpdateVariables
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEncryption _encryption;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlValidator _xmlValidator;

        public UpdateVariables(
            IConfigurationService configurationService,
            IEncryption encryption,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _configurationService = configurationService;
            _encryption = encryption;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.VariablesSchema);
        }

        public void Update(XmlDocument xmlDocument)
        {
            try
            {
                if (xmlDocument.DocumentElement == null)
                    throw _exceptionHelper.CriticalException("{12B40BC8-FCCD-4A4A-B121-660C0B7B4491}");

                string xmlString = xmlDocument.DocumentElement.OuterXml;
                ValidateXml(xmlString);
                SaveXml(xmlString);

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(xmlString);
                    if (validationResponse.Success == false)
                        throw new CriticalLogicBuilderException(string.Join(Environment.NewLine, validationResponse.Errors));
                }

                void SaveXml(string xmlString)
                {
                    string fullPath = _pathHelper.CombinePaths
                    (
                        _configurationService.ProjectProperties.ProjectPath,
                        ConfigurationFiles.Variables
                    );

                    if (!Directory.Exists(_pathHelper.GetFilePath(fullPath)))
                        _fileIOHelper.CreateDirectory(_pathHelper.GetFilePath(fullPath));

                    _encryption.EncryptToFile(fullPath, xmlString);
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
