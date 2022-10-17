using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
    internal class LoadProjectProperties : ILoadProjectProperties
    {
        private readonly ICreateProjectProperties _createProjectProperties;
        private readonly IEncryption _encryption;
        private readonly IPathHelper _pathHelper;
        private readonly IProjectPropertiesXmlParser _projectPropertiesXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;

        public LoadProjectProperties(
            ICreateProjectProperties createProjectProperties,
            IEncryption encryption,
            IPathHelper pathHelper,
            IProjectPropertiesXmlParser projectPropertiesXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _createProjectProperties = createProjectProperties;
            _encryption = encryption;
            _pathHelper = pathHelper;
            _projectPropertiesXmlParser = projectPropertiesXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.ProjectPropertiesSchema);
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
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument).OuterXml
                );
                if (validationResponse.Success == false)
                    throw new XmlValidationException(string.Join(Environment.NewLine, validationResponse.Errors));

                return _projectPropertiesXmlParser.GeProjectProperties
                (
                    _xmlDocumentHelpers.GetDocumentElement(xmlDocument),
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
