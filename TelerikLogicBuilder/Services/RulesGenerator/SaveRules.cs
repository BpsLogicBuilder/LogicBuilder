using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class SaveRules : ISaveRules
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IRulesAssembler _rulesAssembler;

        public SaveRules(IConfigurationService configurationService, IFileIOHelper fileIOHelper, IPathHelper pathHelper, IRulesAssembler rulesAssembler)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _rulesAssembler = rulesAssembler;
        }

        public void Save(string sourceFile, IList<RuleBag> rules, string documentTypeFolder, RuleChainingBehavior ruleChainingBehavior)
        {
            foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
            {
                Save
                (
                    sourceFile,
                    rules,
                    application,
                    GetFullFolderPath(application, documentTypeFolder),
                    ruleChainingBehavior
                );
            }
        }

        private void Save(string sourceFile, IList<RuleBag> rules, Application application, string saveFolderPath, RuleChainingBehavior ruleChainingBehavior)
        {
            string module = _pathHelper.GetModuleName(sourceFile).ToLower(CultureInfo.InvariantCulture);
            if (application.ExcludedModules.Contains(module))
                return;

            if (!Directory.Exists(saveFolderPath))
                _fileIOHelper.CreateDirectory(saveFolderPath);

            string fileNameNoExtension = _pathHelper.GetFileNameNoExtention(sourceFile);
            string fullSavePath = $"{_pathHelper.CombinePaths(saveFolderPath, fileNameNoExtension)}{FileExtensions.RULESFILEEXTENSION}";

            RuleSet ruleSet = new()
            {
                Name = module,
                ChainingBehavior = ruleChainingBehavior
            };

            foreach (RuleBag ruleBag in rules)
            {
                if (ruleBag.Applications == null || ruleBag.Applications.Contains(application.Name))
                    ruleSet.Rules.Add(ruleBag.Rule);
            }

            try
            {
                _fileIOHelper.SetWritable(fullSavePath, true);
                using StreamWriter streamWriter = new(fullSavePath, false, Encoding.Unicode);
                streamWriter.Write(_rulesAssembler.SerializeRules(ruleSet, application.Runtime));
                streamWriter.Close();
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
        /// Full folder path to the rules file
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
