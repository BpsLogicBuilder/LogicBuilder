using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class LoadFragments : ILoadFragments
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IMessageBoxOptionsHelper _messageBoxOptionsHelper;
        private readonly IXmlValidator _xmlValidator;
        private readonly ICreateFragments _createFragments;

        public LoadFragments(IConfigurationService configurationService, IXmlValidator xmlValidator, ICreateFragments createFragments, IContextProvider contextProvider)
        {
            _configurationService = configurationService;
            _pathHelper = contextProvider.PathHelper;
            _messageBoxOptionsHelper = contextProvider.MessageBoxOptionsHelper;
            _createFragments = createFragments;
            _xmlValidator = xmlValidator;
        }

        public XmlDocument Load()
        {
            string fullPath = _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ConfigurationFiles.Fragments
            );

            try
            {
                XmlDocument xmlDocument = new();
                xmlDocument.Load(fullPath);
                ValidateXml(xmlDocument.DocumentElement.OuterXml);

                return xmlDocument;

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(SchemaName.FragmentsSchema, xmlString);
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
                        $"{GetDialogMessage()}{Environment.NewLine}{Environment.NewLine}{Strings.createNewFragmentsFileQuestion}",
                        _messageBoxOptionsHelper.MessageBoxOptions
                    );

                    if (dialogResult == DialogResult.OK)
                    {
                        return _createFragments.Create();
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
