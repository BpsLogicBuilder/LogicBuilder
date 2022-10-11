using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class RulesAssembler : IRulesAssembler
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly ILongStringManager _longStringManager;
        private readonly IPathHelper _pathHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public RulesAssembler(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            ILongStringManager longStringManager,
            IPathHelper pathHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _longStringManager = longStringManager;
            _pathHelper = pathHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Task AssembleResources(IList<string> sourceFiles, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => Task.Run
            (
                () => AssembleResources(progress, sourceFiles),
                cancellationTokenSource.Token
            );

        public Task AssembleRules(IList<string> sourceFiles, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => Task.Run
            (
                () => AssembleRules(progress, sourceFiles),
                cancellationTokenSource.Token
            );

        public RuleSet DeserializeRuleSet(string ruleSetXmlDefinition, string fullPath)
        {
            WorkflowMarkupSerializer serializer = new();
            try
            {
                using StringReader stringReader = new(_longStringManager.UpdateStrongNameToNetCore(ruleSetXmlDefinition));
                using XmlTextReader reader = new(stringReader);
                return (RuleSet)serializer.Deserialize(reader);
            }
            catch (WorkflowMarkupSerializationException ex)
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidRuleSetFormat, fullPath), ex);
            }
        }

        public string SerializeRules(object rules, RuntimeType platForm)
        {
            StringBuilder ruleDefinition = new();
            WorkflowMarkupSerializer serializer = new();
            using (XmlWriter writer = _xmlDocumentHelpers.CreateUnformattedXmlWriter(ruleDefinition))
            {
                serializer.Serialize(writer, rules);
                writer.Flush();
            }

            return _longStringManager.UpdateStrongNameByPlatForm(ruleDefinition.ToString(), platForm);
        }

        private void AssembleResources(IProgress<ProgressMessage> progress, IList<string> sourceFiles)
        {
            string rulesFolderPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.RULESFOLDER);

            IList<Application> applicationList = _configurationService
                .ProjectProperties
                .ApplicationList
                .Values
                .OrderBy(a => a.Nickname)
                .ToArray();

            foreach (Application application in applicationList)
            {
                string diagramRulesDirectory = _pathHelper.CombinePaths(rulesFolderPath, application.Name, ProjectPropertiesConstants.DIAGRAMFOLDER);
                string tableRulesDirectory = _pathHelper.CombinePaths(rulesFolderPath, application.Name, ProjectPropertiesConstants.TABLEFOLDER);
                string rulesBuildPath = _pathHelper.CombinePaths(rulesFolderPath, application.Name, ProjectPropertiesConstants.BUILDFOLDER);

                IDictionary<string, string> sourceFilesTable = new SortedDictionary<string, string>
                (
                    sourceFiles.ToDictionary
                    (
                        file => _pathHelper.GetFileName(file).ToLowerInvariant()
                    )
                );//sort ensures module.tbl wil be processed before module.vsdx or module.vsd

                if (!Directory.Exists(diagramRulesDirectory))
                    _fileIOHelper.CreateDirectory(diagramRulesDirectory);
                if (!Directory.Exists(tableRulesDirectory))
                    _fileIOHelper.CreateDirectory(tableRulesDirectory);
                if (!Directory.Exists(rulesBuildPath))
                    _fileIOHelper.CreateDirectory(rulesBuildPath);

                try
                {
                    int assembledFiles = 0;
                    int totalFiles = sourceFiles.Count;
                    foreach (string fileName in sourceFilesTable.Keys)
                    {
                        string extension = _pathHelper.GetExtension(fileName);

                        if
                        (
                            !new HashSet<string>
                            {
                                FileExtensions.VISIOFILEEXTENSION,
                                FileExtensions.VSDXFILEEXTENSION,
                                FileExtensions.TABLEFILEEXTENSION
                            }.Contains(extension)
                        )
                        {
                            throw _exceptionHelper.CriticalException("{336FEED6-214F-4A8A-BB99-B91B2AB8BF84}");
                        }

                        string module = _pathHelper.GetModuleName(fileName).ToLowerInvariant();
                        if (application.ExcludedModules.Contains(module))
                            continue;

                        string fileNameNoExtension = _pathHelper.GetFileNameNoExtention(fileName);

                        string moduleResourceFileName = $"{fileNameNoExtension}{FileExtensions.RESOURCEFILEEXTENSION}";
                        string diagramResourceFileFullPath = _pathHelper.CombinePaths(diagramRulesDirectory, moduleResourceFileName);
                        string tableResourceFileFullPath = _pathHelper.CombinePaths(tableRulesDirectory, moduleResourceFileName);

                        if (extension == FileExtensions.VISIOFILEEXTENSION
                            || extension == FileExtensions.VSDXFILEEXTENSION)
                        {
                            if (!File.Exists(tableResourceFileFullPath))
                                SaveResourceFile(diagramResourceFileFullPath);
                            else
                                CombineResourceFilesAndSave();
                        }
                        else if (extension == FileExtensions.TABLEFILEEXTENSION)
                        {
                            if (!File.Exists(diagramResourceFileFullPath))
                            {
                                SaveResourceFile(tableResourceFileFullPath);
                            }
                            else
                            {
                                if (!sourceFilesTable.ContainsKey($"{module}{FileExtensions.VSDXFILEEXTENSION}")
                                    && !sourceFilesTable.ContainsKey($"{module}{FileExtensions.VISIOFILEEXTENSION}"))
                                    CombineResourceFilesAndSave();//combine should only run once if both table and visio files are in the list
                            }
                        }

                        progress.Report
                        (
                            new ProgressMessage
                            (
                                (int)(++assembledFiles / (float)totalFiles * 100),
                                string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskAssemblingResourcesFormat, application.Nickname)
                            )
                        );

                        void CombineResourceFilesAndSave()
                        {
                            string resourceFileFullPath = _pathHelper.CombinePaths(rulesBuildPath, moduleResourceFileName);
                            string textFileFullPath = $"{_pathHelper.CombinePaths(rulesBuildPath, moduleResourceFileName)}{FileExtensions.RESOURCETEXTFILEEXTENSION}";
                            _fileIOHelper.SetWritable(resourceFileFullPath, true);
                            _fileIOHelper.SetWritable(textFileFullPath, true);

                            using Stream fs = new FileStream(resourceFileFullPath, FileMode.OpenOrCreate, FileAccess.Write);
                            using (IResourceWriter resourceWriter = new ResourceWriter(fs))
                            {
                                using (StreamWriter tr = new(textFileFullPath, false, Encoding.Unicode))
                                {
                                    using (IResourceReader reader = _fileIOHelper.GetResourceReader(diagramResourceFileFullPath))
                                    {
                                        foreach (DictionaryEntry entry in reader.OfType<DictionaryEntry>())
                                        {
                                            resourceWriter.AddResource((string)entry.Key, _longStringManager.GetLongStringForBinary((string)entry.Value!, application.Runtime));
                                            tr.WriteLine($"{entry.Key}{Strings.equalSign}{_longStringManager.GetLongStringForText((string)entry.Value!, application.Runtime)}");
                                        }
                                    }

                                    using (IResourceReader reader = _fileIOHelper.GetResourceReader(tableResourceFileFullPath))
                                    {
                                        foreach (DictionaryEntry entry in reader.OfType<DictionaryEntry>())
                                        {
                                            resourceWriter.AddResource((string)entry.Key, _longStringManager.GetLongStringForBinary((string)entry.Value!, application.Runtime));
                                            tr.WriteLine($"{entry.Key}{Strings.equalSign}{_longStringManager.GetLongStringForText((string)entry.Value!, application.Runtime)}");
                                        }
                                    }
                                }

                                resourceWriter.Generate();
                                resourceWriter.Close();
                            }

                            fs.Close();
                        }

                        void SaveResourceFile(string fullSourcePath)
                        {
                            string resourceFileFullPath = _pathHelper.CombinePaths(rulesBuildPath, moduleResourceFileName);
                            string textFileFullPath = $"{_pathHelper.CombinePaths(rulesBuildPath, moduleResourceFileName)}{FileExtensions.RESOURCETEXTFILEEXTENSION}";
                            _fileIOHelper.SetWritable(resourceFileFullPath, true);
                            _fileIOHelper.SetWritable(textFileFullPath, true);
                            _fileIOHelper.GetNewFileInfo(fullSourcePath).CopyTo(resourceFileFullPath, true);

                            using StreamWriter tr = new(textFileFullPath, false, Encoding.Unicode);
                            using (IResourceReader reader = _fileIOHelper.GetResourceReader(fullSourcePath))
                            {
                                foreach (DictionaryEntry entry in reader.OfType<DictionaryEntry>())
                                {
                                    tr.WriteLine($"{entry.Key}{Strings.equalSign}{_longStringManager.GetLongStringForText((string)entry.Value!, application.Runtime)}");
                                }
                            }

                            tr.Close();
                        }
                    }
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
        }

        private void AssembleRules(IProgress<ProgressMessage> progress, IList<string> sourceFiles)
        {
            string rulesFolderPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.RULESFOLDER);

            IList<Application> applicationList = _configurationService
                .ProjectProperties
                .ApplicationList
                .Values
                .OrderBy(a => a.Nickname)
                .ToArray();

            foreach (Application application in applicationList)
            {
                string diagramRulesDirectory = _pathHelper.CombinePaths(rulesFolderPath, application.Name, ProjectPropertiesConstants.DIAGRAMFOLDER);
                string tableRulesDirectory = _pathHelper.CombinePaths(rulesFolderPath, application.Name, ProjectPropertiesConstants.TABLEFOLDER);
                string rulesBuildPath = _pathHelper.CombinePaths(rulesFolderPath, application.Name, ProjectPropertiesConstants.BUILDFOLDER);

                IDictionary<string, string> sourceFilesTable = new SortedDictionary<string, string>
                (
                    sourceFiles.ToDictionary
                    (
                        file => _pathHelper.GetFileName(file).ToLowerInvariant()
                    )
                );//sort ensures module.tbl wil be processed before module.vsd

                if (!Directory.Exists(diagramRulesDirectory))
                    _fileIOHelper.CreateDirectory(diagramRulesDirectory);
                if (!Directory.Exists(tableRulesDirectory))
                    _fileIOHelper.CreateDirectory(tableRulesDirectory);
                if (!Directory.Exists(rulesBuildPath))
                    _fileIOHelper.CreateDirectory(rulesBuildPath);

                try
                {
                    int assembledFiles = 0;
                    int totalFiles = sourceFiles.Count;

                    foreach (string fileName in sourceFilesTable.Keys)
                    {
                        string extension = _pathHelper.GetExtension(fileName);

                        if
                        (
                            !new HashSet<string>
                            {
                                FileExtensions.VISIOFILEEXTENSION,
                                FileExtensions.VSDXFILEEXTENSION,
                                FileExtensions.TABLEFILEEXTENSION
                            }.Contains(extension)
                        )
                        {
                            throw _exceptionHelper.CriticalException("{336FEED6-214F-4A8A-BB99-B91B2AB8BF84}");
                        }

                        string module = _pathHelper.GetModuleName(fileName).ToLowerInvariant();
                        if (application.ExcludedModules.Contains(module))
                            continue;

                        string fileNameNoExtension = _pathHelper.GetFileNameNoExtention(fileName);

                        string moduleRulesFileName = string.Concat(fileNameNoExtension, FileExtensions.RULESFILEEXTENSION);
                        string diagramRulesFileFullPath = _pathHelper.CombinePaths(diagramRulesDirectory, moduleRulesFileName);
                        string tableRulesFileFullPath = _pathHelper.CombinePaths(tableRulesDirectory, moduleRulesFileName);

                        if (extension == FileExtensions.VISIOFILEEXTENSION
                            || extension == FileExtensions.VSDXFILEEXTENSION)
                        {
                            if (!File.Exists(tableRulesFileFullPath))
                                SaveRulesFile(diagramRulesFileFullPath);
                            else
                                CombineRulesFilesAndSave();
                        }
                        else if (extension == FileExtensions.TABLEFILEEXTENSION)
                        {
                            if (!File.Exists(diagramRulesFileFullPath))
                            {
                                SaveRulesFile(tableRulesFileFullPath);
                            }
                            else
                            {
                                if (!sourceFilesTable.ContainsKey($"{module}{FileExtensions.VSDXFILEEXTENSION}")
                                    && !sourceFilesTable.ContainsKey($"{module}{FileExtensions.VISIOFILEEXTENSION}"))
                                    CombineRulesFilesAndSave();//combine should only run once if both table and visio files are in the list
                            }
                        }

                        progress.Report
                        (
                            new ProgressMessage
                            (
                                (int)(++assembledFiles / (float)totalFiles * 100),
                                string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskAssemblingRulesFormat, application.Nickname)
                            )
                        );

                        void CombineRulesFilesAndSave()
                        {
                            RuleSet ruleSetAll;
                            RuleSet ruleSetTable;

                            using (StreamReader inStream = new(diagramRulesFileFullPath, Encoding.Unicode))
                                ruleSetAll = DeserializeRuleSet(inStream.ReadToEnd(), diagramRulesFileFullPath);

                            using (StreamReader inStream = new(tableRulesFileFullPath, Encoding.Unicode))
                                ruleSetTable = DeserializeRuleSet(inStream.ReadToEnd(), tableRulesFileFullPath);

                            ruleSetAll.ChainingBehavior = ruleSetTable.ChainingBehavior;
                            foreach (Rule rule in ruleSetTable.Rules)
                                ruleSetAll.Rules.Add(rule);

                            string ruleSetFullPath = _pathHelper.CombinePaths(rulesBuildPath, moduleRulesFileName);
                            _fileIOHelper.SetWritable(ruleSetFullPath, true);
                            using StreamWriter streamWriter = new(ruleSetFullPath, false, Encoding.Unicode);
                            streamWriter.Write(SerializeRules(ruleSetAll, application.Runtime));
                            streamWriter.Close();
                        }

                        void SaveRulesFile(string fullSourcePath)
                        {
                            string ruleSetFullPath = _pathHelper.CombinePaths(rulesBuildPath, moduleRulesFileName);
                            _fileIOHelper.SetWritable(ruleSetFullPath, true);
                            _fileIOHelper.GetNewFileInfo(fullSourcePath).CopyTo(ruleSetFullPath, true);
                        }
                    }
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
        }
    }
}
