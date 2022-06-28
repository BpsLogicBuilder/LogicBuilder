using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class DocumentsExplorer : UserControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IDocumentsExplorerTreeViewBuilder _documentsExplorerTreeViewBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly DocumentExplorerErrorsList documentProfileErrors = new();
        private readonly Dictionary<string, string> documentNames = new();
        private readonly Dictionary<string, string> expandedNodes = new();
        private FileSystemWatcher? fileSystemWatcher;

        public DocumentsExplorer(
            IConfigurationService configurationService,
            IDocumentsExplorerTreeViewBuilder documentsExplorerTreeViewBuilder,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _documentsExplorerTreeViewBuilder = documentsExplorerTreeViewBuilder;
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
            DisposeFileSystemWatcher();
        }

        public void CreateProfile()
        {
            if (fileSystemWatcher != null)
                throw _exceptionHelper.CriticalException("{905A8EA2-4C2D-4B99-AAB4-5B0D00CB4E03}");

            //Can't create until the project is opened and CreateProfile is called.
            //It should always be null if CreateProfile is called only when the project opens.
            CreateFileSystemWatcher();

            BuildTreeView();
        }

        private void BuildTreeView() 
            => _documentsExplorerTreeViewBuilder.Build
            (
                radTreeView1, 
                documentProfileErrors, 
                documentNames, 
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
            string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
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
            this.radTreeView1.CreateNodeElement += RadTreeView1_CreateNodeElement;
            this.radTreeView1.NodeFormatting += RadTreeView1_NodeFormatting;
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            documentProfileErrors.ErrorCountChanged += DocumentProfileErrors_ErrorCountChanged;
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += DocumentsExplorer_Disposed;
        }

        private void SetTreeViewBorderColor(int errorCount)
        {
            this.radTreeView1.TreeViewElement.BorderColor = errorCount > 0
                ? ForeColorUtility.GetTreeViewBorderErrorColor()
                : ForeColorUtility.GetTreeViewBorderColor(ThemeResolutionService.ApplicationThemeName);
        }

        #region Event Handlers
        private void DocumentProfileErrors_ErrorCountChanged(int errorCount)
        {
            SetTreeViewBorderColor(errorCount);
            _uiNotificationService.NotifyDocumentExplorerErrorCountChanged(errorCount);
        }

        private void DocumentsExplorer_Disposed(object? sender, EventArgs e) => DisposeFileSystemWatcher();

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e) 
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e) 
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e) 
            => BuildTreeViewThreadSafe();

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e) { }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e) 
            => BuildTreeViewThreadSafe();

        private void RadTreeView1_CreateNodeElement(object sender, CreateTreeNodeElementEventArgs e)
        {
            e.NodeElement = new StateImageTreeNodeElement(_exceptionHelper);
        }

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

        private void RadTreeView1_NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            if (e.Node is not StateImageRadTreeNode treeNode)
                throw _exceptionHelper.CriticalException("{ED1BDBAD-20EC-4DA9-879A-00CAD2FFA3D3}");

            if (e.NodeElement is not StateImageTreeNodeElement treeNodeElement)
                throw _exceptionHelper.CriticalException("{9ED2FB8F-C952-4F5C-B39C-91544C947BDB}");

            treeNodeElement.StateImage = treeNode.StateImage;
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args) 
            => SetTreeViewBorderColor(documentProfileErrors.Count);
        #endregion Event Handlers
    }
}
