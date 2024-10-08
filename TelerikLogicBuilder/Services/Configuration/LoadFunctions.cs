﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class LoadFunctions : ILoadFunctions
    {
        private readonly IBuiltInFunctionsLoader _builtInFunctionsLoader;
        private readonly IConfigurationService _configurationService;
        private readonly ICreateFunctions _createFunctions;
        private readonly IEncryption _encryption;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;

        public LoadFunctions(
            IBuiltInFunctionsLoader builtInFunctionsLoader,
            IConfigurationService configurationService,
            ICreateFunctions createFunctions,
            IEncryption encryption,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _builtInFunctionsLoader = builtInFunctionsLoader;
            _configurationService = configurationService;
            _createFunctions = createFunctions;
            _encryption = encryption;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.FunctionsSchema);
        }

        public XmlDocument Load()
        {
            string fullPath = _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ConfigurationFiles.Functions
            );

            return LoadXmlDocument(fullPath);
        }

        public XmlDocument Load(string fullPath)
        {
            return LoadXmlDocument(fullPath);
        }

        private XmlDocument LoadXmlDocument(string fullPath)
        {
            try
            {
                if (!File.Exists(fullPath))
                    return _createFunctions.Create();

                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(LoadXml());
                AppendBuildtInFunctions(xmlDocument);
                ValidateXml(_xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml);

                return xmlDocument;

                string LoadXml()
                {
                    string loadedString = _encryption.DecryptFromFile(fullPath);
                    if (!string.IsNullOrEmpty(loadedString))
                        return loadedString;

                    return _fileIOHelper.ReadFromFile(fullPath);
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

                void ValidateXml(string xmlString)
                {
                    var validationResponse = _xmlValidator.Validate(xmlString);
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
