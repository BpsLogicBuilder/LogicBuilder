﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
    internal class LoadProjectProperties : ILoadProjectProperties
    {
        private readonly IEncryption _encryption;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly ICreateProjectProperties _createProjectProperties;
        private readonly IMessageBoxOptionsHelper _messageBoxOptionsHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlValidator _xmlValidator;
        private readonly IContextProvider _contextProvider;

        public LoadProjectProperties(IContextProvider contextProvider)
        {
            _encryption = contextProvider.Encryption;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _createProjectProperties = contextProvider.CreateProjectProperties;
            _messageBoxOptionsHelper = contextProvider.MessageBoxOptionsHelper;
            _pathHelper = contextProvider.PathHelper;
            _xmlValidator = contextProvider.XmlValidator;
            _contextProvider = contextProvider;
        }

        public ProjectProperties Load(string fullPath)
        {
            try
            {
                XmlDocument xmlDocument = _xmlDocumentHelpers.ToXmlDocument(_encryption.DecryptFromFile(fullPath));
                _xmlValidator.Validate(SchemaName.ProjectPropertiesSchema, xmlDocument.DocumentElement.OuterXml);

                return new ProjectPropertiesXmlParserUtility
                (
                    xmlDocument.DocumentElement,
                    _contextProvider
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
                        $"{GetDialogMessage()}{Environment.NewLine}{Environment.NewLine}{Strings.createNewProjectFileQuestion}",
                        _messageBoxOptionsHelper.MessageBoxOptions
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
