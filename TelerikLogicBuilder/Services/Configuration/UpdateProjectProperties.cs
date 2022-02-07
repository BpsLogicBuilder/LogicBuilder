using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class UpdateProjectProperties : IUpdateProjectProperties
    {
        private readonly IEncryption _encryption;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlValidator _xmlValidator;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IContextProvider _contextProvider;

        public UpdateProjectProperties(IContextProvider contextProvider)
        {
            _encryption = contextProvider.Encryption;
            _pathHelper = contextProvider.PathHelper;
            _xmlValidator = contextProvider.XmlValidator;
            _fileIOHelper = contextProvider.FileIOHelper;
            _contextProvider = contextProvider;
        }

        public ProjectProperties Update(string fullPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes)
        {
            string projectName = _pathHelper.GetFileNameNoExtention(fullPath);
            string projectPath = _pathHelper.GetFilePath(fullPath);

            ProjectProperties projectProperties = new
            (
                projectName,
                projectPath,
                applicationList,
                new HashSet<string>(),
                _contextProvider
            );

            Save(projectProperties);

            return projectProperties;
        }

        private void Save(ProjectProperties projectProperties)
        {
            try
            {
                string xmlString = projectProperties.ToXml;
                _xmlValidator.Validate(SchemaName.ProjectPropertiesSchema, xmlString);

                if (!Directory.Exists(projectProperties.ProjectPath))
                    _fileIOHelper.CreateDirectory(projectProperties.ProjectPath);

                _encryption.EncryptToFile(projectProperties.ProjectFileFullName, xmlString);
            }
            catch (XmlException ex)
            {
                throw new CriticalLogicBuilderException(ex.Message, ex);
            }
            catch (XmlValidationException ex)
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
