using ABIS.LogicBuilder.FlowBuilder.Configuration;
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
        private readonly IContextProvider _contextProvider;
        private readonly IEncryption _encryption;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlValidator _xmlValidator;

        public CreateProjectProperties(
            IContextProvider contextProvider,
            IEncryption encryption,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            IXmlValidatorFactory xmlValidatorFactory)
        {
            _contextProvider = contextProvider;
            _encryption = encryption;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _xmlValidator = xmlValidatorFactory.GetXmlValidator(SchemaName.ProjectPropertiesSchema);
        }

        public ProjectProperties Create(string pathToProjectFolder, string projectName)
        {
            string projectPath = _pathHelper.CombinePaths(pathToProjectFolder.Trim(), projectName);

            ProjectProperties projectProperties = new
            (
                projectName,
                projectPath,
                GetDefaultApplicationList(),
                new HashSet<string>(),
                _contextProvider
            );

            Save(projectProperties);

            return projectProperties;
        }

        public ProjectProperties Create(string fullPath)
        {
            string projectName = _pathHelper.GetFileNameNoExtention(fullPath);
            string projectPath = _pathHelper.GetFilePath(fullPath);

            ProjectProperties projectProperties = new
            (
                projectName,
                projectPath,
                GetDefaultApplicationList(),
                new HashSet<string>(),
                _contextProvider
            );

            Save(projectProperties);

            return projectProperties;
        }

        private Dictionary<string, Application> GetDefaultApplicationList()
        {
            string applicationNameString = string.Format(CultureInfo.CurrentCulture, Strings.applicationNameFormat, 1.ToString("00", CultureInfo.CurrentCulture));
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            return new Dictionary<string, Application>
            {
                [applicationNameString] = new
                (
                    applicationNameString,
                    applicationNameString,
                    string.Concat(applicationNameString, Strings.dotExe),
                    _pathHelper.CombinePaths(programFilesPath, applicationNameString, Strings.defaultActivityAssemblyFolder),
                    RuntimeType.NetCore,
                    new List<string>(),
                    Strings.defaultActivityClass,
                    string.Concat(applicationNameString, Strings.dotExe),
                    _pathHelper.CombinePaths(programFilesPath, applicationNameString),
                    new List<string>(),
                    Strings.defaultResourcesFile,
                    _pathHelper.CombinePaths(programFilesPath, applicationNameString, Strings.defaultResourcesFolder),
                    Strings.defaultRulesFile,
                    _pathHelper.CombinePaths(programFilesPath, applicationNameString, Strings.defaultRulesFolder),
                    new List<string>(),
                    new WebApiDeployment
                    (
                        Strings.defaultPostFileDataUrl,
                        Strings.defaultPostVariableMetaDataUrl,
                        Strings.defaultDeleteRulesUrl,
                        Strings.defaultDeleteAllRulesUrl,
                        _contextProvider
                    ),
                    _contextProvider
                )
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
