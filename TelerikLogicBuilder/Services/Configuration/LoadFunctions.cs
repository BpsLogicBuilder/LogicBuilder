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
    internal class LoadFunctions : ILoadFunctions
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEncryption _encryption;
        private readonly IXmlValidator _xmlValidator;
        private readonly ICreateFunctions _createFunctions;
        private readonly IBuiltInFunctionsLoader _builtInFunctionsLoader;
        private readonly IMainWindow _mainWindow;

        public LoadFunctions(IContextProvider contextProvider, IXmlValidator xmlValidator, IBuiltInFunctionsLoader builtInFunctionsLoader, ICreateFunctions createFunctions, IMainWindow mainWindow)
        {
            _configurationService = contextProvider.ConfigurationService;
            _pathHelper = contextProvider.PathHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _encryption = contextProvider.Encryption;
            _xmlValidator = xmlValidator;
            _builtInFunctionsLoader = builtInFunctionsLoader;
            _createFunctions = createFunctions;
            _mainWindow = mainWindow;
        }

        public XmlDocument Load()
        {
            string fullPath = _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath, 
                ConfigurationFiles.Functions
            );

            try
            {
                if (!File.Exists(fullPath))
                    return _createFunctions.Create();

                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(_encryption.DecryptFromFile(fullPath));
                AppendBuildtInFunctions(xmlDocument);
                ValidateXml(_xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml);

                return xmlDocument;

                void AppendBuildtInFunctions(XmlDocument xmlDocument)
                    => _xmlDocumentHelpers.GetDocumentElement(xmlDocument).AppendChild
                    (
                        _xmlDocumentHelpers.MakeFragment
                        (
                            xmlDocument,
                            _xmlDocumentHelpers.GetDocumentElement(_builtInFunctionsLoader.Load()).OuterXml
                        )
                    );

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(SchemaName.FunctionsSchema, xmlString);
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
                        $"{GetDialogMessage()}{Environment.NewLine}{Environment.NewLine}{Strings.createNewFunctionsFileQuestion}"
                    );

                    if (dialogResult == DialogResult.OK)
                    {
                        return _createFunctions.Create();
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
