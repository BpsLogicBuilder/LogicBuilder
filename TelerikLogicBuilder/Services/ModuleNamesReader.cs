using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ModuleNamesReader : IModuleNamesReader
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;

        public ModuleNamesReader(IConfigurationService configurationService, IFileIOHelper fileIOHelper, IPathHelper pathHelper)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
        }

        public IDictionary<string, string> GetNames()
        {
            Dictionary<string, string> moduleNames = new();

            if (_configurationService.ProjectProperties.ProjectPath.Length == 0)
                return moduleNames;

            if (_configurationService.ProjectProperties.ProjectName.Length == 0)
                return moduleNames;

            string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
            if (!Directory.Exists(documentPath))
                _fileIOHelper.CreateDirectory(documentPath);

            AddModuleNames(documentPath, moduleNames);
            return moduleNames;
        }

        private void AddModuleNames(string directory, IDictionary<string, string> moduleNames)
        {
            DirectoryInfo directoryInfo = new(directory);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Name.ToLowerInvariant().EndsWith(FileExtensions.VISIOFILEEXTENSION)
                    || fileInfo.Name.ToLowerInvariant().EndsWith(FileExtensions.VSDXFILEEXTENSION)
                    || fileInfo.Name.ToLowerInvariant().EndsWith(FileExtensions.TABLEFILEEXTENSION))
                {
                    string moduleName = _pathHelper.GetModuleName(fileInfo.FullName);
                    string moduleNameKey = moduleName.ToLower(CultureInfo.CurrentCulture).Trim();
                    if (moduleNames.ContainsKey(moduleNameKey))
                        continue;

                    moduleNames.Add(moduleNameKey, moduleName);
                }
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
                AddModuleNames(_pathHelper.CombinePaths(directory, subDirectoryInfo.Name), moduleNames);
        }
    }
}
