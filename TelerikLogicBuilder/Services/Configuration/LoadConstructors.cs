using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class LoadConstructors : ILoadConstructors
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEncryption _encryption;
        private readonly IMessageBoxOptionsHelper _messageBoxOptionsHelper;
        private readonly IXmlValidator _xmlValidator;
        private readonly ICreateConstructors _createConstructors;

        public LoadConstructors(IConfigurationService configurationService, IXmlValidator xmlValidator, ICreateConstructors createConstructors, IContextProvider contextProvider)
        {
            _configurationService = configurationService;
            _pathHelper = contextProvider.PathHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _encryption = contextProvider.Encryption;
            _messageBoxOptionsHelper = contextProvider.MessageBoxOptionsHelper;
            _createConstructors = createConstructors;
            _xmlValidator = xmlValidator;
            
        }

        public XmlDocument Load()
        {
            string fullPath = _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ConfigurationFiles.Constructors
            );

            try
            {
                if (!File.Exists(fullPath))
                    return _createConstructors.Create();

                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(_encryption.DecryptFromFile(fullPath));
                ValidateXml(_xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml);

                return xmlDocument;

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(SchemaName.ConstructorSchema, xmlString);
                    if (validationResponse.Success == false)
                        throw new XmlValidationException(string.Join(Environment.NewLine, validationResponse.Errors));
                }
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is XmlValidationException || ex is LogicBuilderException)
                {
                    DialogResult dialogResult = DisplayMessage.ShowQuestion
                    (
                        $"{GetDialogMessage()}{Environment.NewLine}{Environment.NewLine}{Strings.createNewConstructorsFileQuestion}",
                        _messageBoxOptionsHelper.MessageBoxOptions
                    );

                    if (dialogResult == DialogResult.OK)
                    {
                        return _createConstructors.Create();
                    }
                    else
                    {
                        throw;
                    }

                    string GetDialogMessage()
                        => string.Format(CultureInfo.CurrentCulture, Strings.invalidConfigurationDocumentFormat, fullPath);
                }

                throw new CriticalLogicBuilderException(ex.Message);
            }
        }
    }
}
