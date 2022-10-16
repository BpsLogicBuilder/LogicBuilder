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
    internal class UpdateConstructors : IUpdateConstructors
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEncryption _encryption;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlValidator _xmlValidator;

        public UpdateConstructors(
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
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.ConstructorSchema);
        }

        public void Update(XmlDocument xmlDocument)
        {
            try
            {
                if (xmlDocument.DocumentElement == null)
                    throw _exceptionHelper.CriticalException("{E0EED35F-5E93-408E-8011-C568DF925D30}");

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
                        ConfigurationFiles.Constructors
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
