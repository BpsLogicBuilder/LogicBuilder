using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class CreateProjectProperties : ICreateProjectProperties
    {
        private readonly ICreateDefaultApplication _createDefaultApplication;
        private readonly IEncryption _encryption;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IProjectPropertiesItemFactory _projectPropertiesItemFactory;
        private readonly IXmlValidator _xmlValidator;

        public CreateProjectProperties(
            ICreateDefaultApplication createDefaultApplication,
            IEncryption encryption,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            IProjectPropertiesItemFactory projectPropertiesItemFactory,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _projectPropertiesItemFactory = projectPropertiesItemFactory;
            _createDefaultApplication = createDefaultApplication;
            _encryption = encryption;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.ProjectPropertiesSchema);
        }

        public ProjectProperties Create(string pathToProjectFolder, string projectName)
        {
            string projectPath = _pathHelper.CombinePaths(pathToProjectFolder.Trim(), projectName);

            ProjectProperties projectProperties = _projectPropertiesItemFactory.GetProjectProperties
            (
                projectName,
                projectPath,
                GetDefaultApplicationList(),
                new HashSet<string>()
            );

            Save(projectProperties);

            return projectProperties;
        }

        public ProjectProperties Create(string fullPath)
        {
            string projectName = _pathHelper.GetFileNameNoExtention(fullPath);
            string projectPath = _pathHelper.GetFilePath(fullPath);

            ProjectProperties projectProperties = _projectPropertiesItemFactory.GetProjectProperties
            (
                projectName,
                projectPath,
                GetDefaultApplicationList(),
                new HashSet<string>()
            );

            Save(projectProperties);

            return projectProperties;
        }

        private Dictionary<string, Application> GetDefaultApplicationList()
        {
            Application application = _createDefaultApplication.Create
            (
                string.Format
                (
                    CultureInfo.CurrentCulture, 
                    Strings.applicationNameFormat, 
                    1.ToString("00", CultureInfo.CurrentCulture)
                )
            );

            return new Dictionary<string, Application>
            {
                [application.Name.ToLowerInvariant()] = application
            };
        }

        private void Save(ProjectProperties projectProperties)
        {
            try
            {
                string xmlString = projectProperties.ToXml;
                var validationResponse = _xmlValidator.Validate(xmlString);
                if (validationResponse.Success == false)
                    throw new CriticalLogicBuilderException(string.Join(Environment.NewLine, validationResponse.Errors));

                if (!Directory.Exists(projectProperties.ProjectPath))
                    _fileIOHelper.CreateDirectory(projectProperties.ProjectPath);

                _encryption.EncryptToFile(projectProperties.ProjectFileFullName, xmlString);
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
