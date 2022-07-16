﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
    internal class LoadProjectProperties : ILoadProjectProperties
    {
        private readonly IEncryption _encryption;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly ICreateProjectProperties _createProjectProperties;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlValidator _xmlValidator;
        private readonly IApplicationXmlParser _applicationXmlParser;
        private readonly IContextProvider _contextProvider;

        public LoadProjectProperties(IContextProvider contextProvider, ICreateProjectProperties createProjectProperties, IApplicationXmlParser applicationXmlParser, IXmlValidator xmlValidator)
        {
            _encryption = contextProvider.Encryption;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _createProjectProperties = createProjectProperties;
            _mainWindow = contextProvider.MainWindow;
            _pathHelper = contextProvider.PathHelper;
            _applicationXmlParser = applicationXmlParser;
            _contextProvider = contextProvider;
            _xmlValidator = xmlValidator;
        }

        public ProjectProperties Load(string fullPath)
        {
            try
            {
                if (!File.Exists(fullPath))
                    return _createProjectProperties.Create(fullPath);

                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(_encryption.DecryptFromFile(fullPath));
                var validationResponse = _xmlValidator.Validate
                (
                    SchemaName.ProjectPropertiesSchema,
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml
                );
                if (validationResponse.Success == false)
                    throw new XmlValidationException(string.Join(Environment.NewLine, validationResponse.Errors));

                return new ProjectPropertiesXmlParserUtility
                (
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument),
                    _contextProvider,
                    _applicationXmlParser
                ).GetProjectProperties
                (
                    _pathHelper.GetFileNameNoExtention(fullPath),
                    _pathHelper.GetFilePath(fullPath)
                );
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is XmlValidationException)
                {
                    DialogResult dialogResult = DisplayMessage.ShowQuestion
                    (
                        $"{GetDialogMessage()}{Environment.NewLine}{Environment.NewLine}{Strings.createNewProjectFileQuestion}"
                    );

                    if (dialogResult == DialogResult.OK)
                    {
                        return _createProjectProperties.Create(fullPath);
                    }
                    else
                    {
                        throw;
                    }

                    string GetDialogMessage()
                        => string.Format(CultureInfo.CurrentCulture, Strings.invalidConfigurationDocumentFormat, fullPath);
                }

                throw;
            }
        }
    }
}
