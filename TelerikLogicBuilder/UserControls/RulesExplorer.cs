using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RulesExplorer : UserControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IRulesExplorerTreeViewBuilder _rulesExplorerTreeViewBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly Dictionary<string, string> expandedNodes = new();
        private FileSystemWatcher? fileSystemWatcher;

        public RulesExplorer(
            IConfigurationService configurationService,
            IRulesExplorerTreeViewBuilder rulesExplorerTreeViewBuilder,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _rulesExplorerTreeViewBuilder = rulesExplorerTreeViewBuilder;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            InitializeComponent();
            Initialize();
        }

        public void ClearProfile()
        {
            radTreeView1.Nodes.Clear();
            expandedNodes.Clear();
            DisposeFileSystemWatcher();
        }

        public void CreateProfile()
        {
            if (fileSystemWatcher != null)
                throw _exceptionHelper.CriticalException("{06424D44-70CD-45FB-9E82-6822B7B7489D}");

            //Can't create until the project is opened and CreateProfile is called.
            //It should always be null if CreateProfile is called only when the project opens.
            CreateFileSystemWatcher();

            BuildTreeView();
        }

        private void BuildTreeView()
            => _rulesExplorerTreeViewBuilder.Build
            (
                radTreeView1,
                expandedNodes
            );

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
            string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.RULESFOLDER);
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

            fileSystemWatcher.IncludeSubdirectories = true;
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
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.Disposed += RulesExplorer_Disposed;
        }

        #region Event Handlers
        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e) { }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
            => BuildTreeViewThreadSafe();

        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node))
                return;

            if (e.Node.Expanded)
            {
                if (!expandedNodes.ContainsKey(e.Node.Name))
                {
                    expandedNodes.Add(e.Node.Name, e.Node.Text);
                }
            }
            else
            {
                if (expandedNodes.ContainsKey(e.Node.Name))
                {
                    expandedNodes.Remove(e.Node.Name);
                }
            }
        }

        private void RulesExplorer_Disposed(object? sender, EventArgs e) => DisposeFileSystemWatcher();
        #endregion Event Handlers
    }
}
