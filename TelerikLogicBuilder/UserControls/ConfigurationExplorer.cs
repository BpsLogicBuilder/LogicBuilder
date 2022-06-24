using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System;
using System.IO;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ConfigurationExplorer : UserControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConfigurationExplorerTreeViewBuilder _configurationExplorerTreeViewBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly UiNotificationService _uiNotificationService;
        private FileSystemWatcher? fileSystemWatcher;

        public ConfigurationExplorer(
            IConfigurationService configurationService,
            IConfigurationExplorerTreeViewBuilder configurationExplorerTreeViewBuilder,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _configurationExplorerTreeViewBuilder = configurationExplorerTreeViewBuilder;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _uiNotificationService = uiNotificationService;
            InitializeComponent();
            Initialize();
        }

        public void ClearProfile()
        {
            radTreeView1.Nodes.Clear();
            DisposeFileSystemWatcher();
        }

        public void CreateProfile()
        {
            if (fileSystemWatcher != null)
                throw _exceptionHelper.CriticalException("{07ABE789-5BBA-4952-9125-FF470DD4E95B}");

            //Can't create until the project is opened and CreateProfile is called.
            //It should always be null if CreateProfile is called only when the project opens.
            CreateFileSystemWatcher();

            BuildTreeView();
        }

        private void BuildTreeView()
            => _configurationExplorerTreeViewBuilder.Build(radTreeView1);

        private void BuildTreeViewThreadSafe()
        {
            if (InvokeRequired)
            {
                this.Invoke(BuildTreeView);
            }
            else
            {
                BuildTreeView();
            }
        }

        private void CreateFileSystemWatcher()
        {
            string documentPath = _configurationService.ProjectProperties.ProjectPath;
            try
            {
                if (!Directory.Exists(documentPath))
                    _fileIOHelper.CreateDirectory(documentPath);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
                return;
            }

            fileSystemWatcher = new(documentPath);

            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            fileSystemWatcher.Error += FileSystemWatcher_Error;
            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void DisposeFileSystemWatcher()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.Dispose();
                fileSystemWatcher = null;
            }
        }

        private void Initialize()
        {
            this.Disposed += ConfigurationExplorer_Disposed;
        }

        #region Event Handlers
        private void ConfigurationExplorer_Disposed(object? sender, EventArgs e) => DisposeFileSystemWatcher();

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e) { }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
            => BuildTreeViewThreadSafe();
        #endregion Event Handlers
    }
}
