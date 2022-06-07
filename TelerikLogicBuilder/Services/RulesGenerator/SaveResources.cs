using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class SaveResources : ISaveResources
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly ILongStringManager _longStringManager;
        private readonly IPathHelper _pathHelper;

        public SaveResources(IConfigurationService configurationService, IFileIOHelper fileIOHelper, ILongStringManager longStringManager, IPathHelper pathHelper)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _longStringManager = longStringManager;
            _pathHelper = pathHelper;
        }

        public void Save(string sourceFile, IDictionary<string, string> resourceStrings, string documentTypeFolder)
        {
            foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
            {
                Save
                (
                    sourceFile,
                    resourceStrings,
                    application,
                    GetFullFolderPath(application, documentTypeFolder)
                );
            }
        }

        private void Save(string sourceFile, IDictionary<string, string> resourceStrings, Application application, string saveFolderPath)
        {
            string module = _pathHelper.GetModuleName(sourceFile);
            if (application.ExcludedModules.Contains(module))
                return;

            if (!Directory.Exists(saveFolderPath))
                _fileIOHelper.CreateDirectory(saveFolderPath);

            string fileNameNoExtension = _pathHelper.GetFileNameNoExtention(sourceFile);
            string fullSavePath = $"{_pathHelper.CombinePaths(saveFolderPath, fileNameNoExtension)}{FileExtensions.RESOURCEFILEEXTENSION}";

            try
            {
                _fileIOHelper.SetWritable(fullSavePath, true);
                using Stream fs = new FileStream(fullSavePath, FileMode.OpenOrCreate, FileAccess.Write);
                using IResourceWriter resourceWriter = new ResourceWriter(fs);
                foreach (string key in resourceStrings.Keys)
                    resourceWriter.AddResource(key, _longStringManager.GetLongStringForBinary(resourceStrings[key], application.Runtime));

                resourceWriter.Generate();
                resourceWriter.Close();
                fs.Close();
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Full folder path to the resources file
        /// </summary>
        /// <param name="application"></param>
        /// <param name="documentTypeFolder">diagram or table</param>
        /// <returns></returns>
        private string GetFullFolderPath(Application application, string documentTypeFolder)
            => _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ProjectPropertiesConstants.RULESFOLDER,
                application.Name,
                documentTypeFolder
            );
    }
}
