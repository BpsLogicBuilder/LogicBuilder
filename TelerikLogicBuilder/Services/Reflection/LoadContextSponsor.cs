using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Reflection
{
    internal class LoadContextSponsor : ILoadContextSponsor
    {
        private readonly IPathHelper _pathHelper;
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IAssemblyLoadContextManager _assemblyLoadContextService;
        private readonly IApplicationTypeInfoManager _applicationTypeInfoService;

        public LoadContextSponsor(IPathHelper pathHelper, IConfigurationService configurationService, IFileIOHelper fileIOHelper, IAssemblyLoadContextManager assemblyLoadContextService, IApplicationTypeInfoManager applicationTypeInfoService)
        {
            _pathHelper = pathHelper;
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _assemblyLoadContextService = assemblyLoadContextService;
            _applicationTypeInfoService = applicationTypeInfoService;
        }

        private bool AssemblyLoadNeeded
            => !_applicationTypeInfoService.HasApplications || _assemblyLoadContextService.GetAssemblyLoadContextDictionary()?.Any() != true;

        public void LoadAssembiesIfNeeded()
        {
            EnsureAssembliesAreCurrent();
            if (AssemblyLoadNeeded)
            {
                //Send UI Notification "Loading"
            }

            InitializeLoadContexts();
            InitializeApplicationInfos();

            //Send UI Notification "Ready"
        }

        public async Task LoadAssembiesIfNeededAsync()
        {
            EnsureAssembliesAreCurrent();
            if (AssemblyLoadNeeded)
            {
                //Send UI Notification "Loading"
            }

            await Task.Run
            (
                () =>
                {
                    InitializeLoadContexts();
                    InitializeApplicationInfos();
                }
            );

            //Send UI Notification "Ready"
        }

        public Task LoadAssembiesOnOpenProject()
        {
            return Task.Run
            (
                () =>
                {
                    EnsureAssembliesAreCurrent();
                    InitializeLoadContexts();
                    InitializeApplicationInfos();
                }
            );
        }

        public void Run(Action action)
        {
            LoadAssembiesIfNeeded();
            action();
        }

        public async Task RunAsync(Action action)
        {
            await LoadAssembiesIfNeededAsync();
            action();
        }

        public void UnloadAssembliesOnCloseProject()
        {
            _applicationTypeInfoService.ClearApplications();
            _assemblyLoadContextService.UnloadLoadContexts();
        }

        private void InitializeLoadContexts()
        {
            if (_assemblyLoadContextService.GetAssemblyLoadContextDictionary()?.Any() != true)
                _assemblyLoadContextService.CreateLoadContexts();
        }

        private void InitializeApplicationInfos()
        {
            foreach (var kvp in _configurationService.ProjectProperties.ApplicationList)
                _applicationTypeInfoService.GetApplicationTypeInfo(kvp.Key);
        }

        private void EnsureAssembliesAreCurrent()
        {
            if (!AllBinFoldersCurrent())
            {
                _assemblyLoadContextService.UnloadLoadContexts();
                RefreshAllBinFolders();
            }
        }

        private bool AllBinFoldersCurrent()
        {
            foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
            {
                bool current = true;
                string binPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.BINFOLDER, application.Name);
                string assemblyFile = _pathHelper.CombinePaths(application.ActivityAssemblyPath, application.ActivityAssembly);
                if (File.Exists(assemblyFile))
                    current = IsBinFolderCurrent(application.ActivityAssemblyPath, binPath);

                if (!current)
                    return false;
            }

            return true;
        }

        private bool IsBinFolderCurrent(string directoryPath, string binPath)
        {
            try
            {
                if (!Directory.Exists(binPath))
                    _fileIOHelper.CreateDirectory(binPath);

                DirectoryInfo directoryInfo = new(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly))
                {
                    FileInfo binFileInfo = new(_pathHelper.CombinePaths(binPath, fileInfo.Name));
                    if (fileInfo.LastWriteTime != binFileInfo.LastWriteTime)
                        return false;
                }

                foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
                {
                    string binSubFolder = _pathHelper.CombinePaths(binPath, subDirectoryInfo.Name);
                    if (!Directory.Exists(binSubFolder))
                        _fileIOHelper.CreateDirectory(binSubFolder);

                    bool current = IsBinFolderCurrent(subDirectoryInfo.FullName, binSubFolder);
                    if (!current)
                        return false;
                }
            }
            catch (ArgumentException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (IOException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }

            return true;
        }

        private void RefreshBinFolder(string directoryPath, string binPath)
        {
            try
            {
                if (!Directory.Exists(binPath))
                    _fileIOHelper.CreateDirectory(binPath);

                DirectoryInfo directoryInfo = new(directoryPath);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly))
                {
                    FileInfo binFileInfo = new(_pathHelper.CombinePaths(binPath, fileInfo.Name));
                    if (fileInfo.LastWriteTime != binFileInfo.LastWriteTime)
                        _fileIOHelper.CopyFile(fileInfo, binFileInfo);
                }

                foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
                {
                    string binSubFolder = _pathHelper.CombinePaths(binPath, subDirectoryInfo.Name);
                    if (!Directory.Exists(binSubFolder))
                        _fileIOHelper.CreateDirectory(binSubFolder);
                    RefreshBinFolder(subDirectoryInfo.FullName, binSubFolder);
                }
            }
            catch (ArgumentException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (IOException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
            }
            catch (LogicBuilderException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
                throw;
            }
        }

        private void RefreshAllBinFolders()
        {
            foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
            {
                string binPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.BINFOLDER, application.Name);
                string assemblyFile = _pathHelper.CombinePaths(application.ActivityAssemblyPath, application.ActivityAssembly);
                if (File.Exists(assemblyFile))
                    RefreshBinFolder(application.ActivityAssemblyPath, binPath);
            }
        }
    }
}
