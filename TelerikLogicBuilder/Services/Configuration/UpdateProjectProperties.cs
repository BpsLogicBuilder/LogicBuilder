using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
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
    internal class UpdateProjectProperties : IUpdateProjectProperties
    {
        private readonly IEncryption _encryption;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IProjectPropertiesItemFactory _projectPropertiesItemFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IXmlValidator _xmlValidator;

        public UpdateProjectProperties(
            IEncryption encryption,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            IProjectPropertiesItemFactory projectPropertiesItemFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _projectPropertiesItemFactory = projectPropertiesItemFactory;
            _encryption = encryption;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.ProjectPropertiesSchema);
        }

        public ProjectProperties Update(string fullPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes)
        {
            string projectName = _pathHelper.GetFileNameNoExtention(fullPath);
            string projectPath = _pathHelper.GetFilePath(fullPath);

            var directoryInfo = _fileIOHelper.GetNewDirectoryInfo(projectPath);
            if (!directoryInfo.Exists)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.projectPathDoesNotExistFormat, projectPath));

            if (directoryInfo.Parent == null)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.projectPathCannotBeTheRootFolderFormat, projectPath));

            ProjectProperties projectProperties = _projectPropertiesItemFactory.GetProjectProperties
            (
                projectName,
                projectPath,
                applicationList,
                connectorObjectTypes
            );

            Save(projectProperties);

            return projectProperties;
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
                _fileIOHelper.SaveFile
                (
                    $"{projectProperties.ProjectFileFullName}{FileExtensions.XMLFILEEXTENSION}",
                    _xmlDocumentHelpers.GetXmlString(xmlString)
                );
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
