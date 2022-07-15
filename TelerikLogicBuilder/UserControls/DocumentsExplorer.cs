using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class DocumentsExplorer : UserControl, IDocumentsExplorer
    {
        private readonly IConfigurationService _configurationService;
        private readonly IDocumentsExplorerTreeViewBuilder _documentsExplorerTreeViewBuilder;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageImageListService;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly DocumentExplorerErrorsList documentProfileErrors = new();
        private readonly Dictionary<string, string> documentNames = new();
        private readonly Dictionary<string, string> expandedNodes = new();
        private FileSystemWatcher? fileSystemWatcher;
        private RadTreeNode? _cutTreeNode;

        private readonly RadMenuItem mnuItemOpenFile = new(Strings.mnuItemOpenFileText) { ImageIndex = ImageIndexes.OPENIMAGEINDEX };
        private readonly RadMenuItem mnuItemRename = new(Strings.mnuItemRenameText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly RadMenuItem mnuItemAddFile = new(Strings.mnuItemAddFileText);
        private readonly RadMenuItem mnuItemAddNewFile = new(Strings.mnuItemAddNewFileText);
        private readonly RadMenuItem mnuItemAddExistingFile = new(Strings.mnuItemAddExistingFileText);
        private readonly RadMenuItem mnuItemCreateDirectory = new(Strings.mnuItemCreateDirectoryText) { ImageIndex = ImageIndexes.NEWFOLDERIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemCloseProject = new(Strings.mnuItemCloseProjectText);
        private readonly RadMenuItem mnuItemRefresh = new(Strings.mnuItemRefreshText) { ImageIndex = ImageIndexes.REFRESHIMAGEINDEX };

        private readonly Func<IDocumentsExplorer, AddExistingFileCommand> _getAddExistingFileCommand;
        private readonly Func<IDocumentsExplorer, AddNewFileCommand> _getAddNewFileCommand;
        private readonly Func<CloseProjectCommand> _getCloseProjectCommand;
        private readonly Func<IDocumentsExplorer, CreateDirectoryCommand> _getCreateDirectoryCommand;
        private readonly Func<IDocumentsExplorer, CutDocumentCommand> _getCutDocumentCommand;
        private readonly Func<IDocumentsExplorer, DeleteCommand> _getDeleteDocumentCommand;
        private readonly Func<IDocumentsExplorer, OpenFileCommand> _getOpenFileCommand;
        private readonly Func<IDocumentsExplorer, PasteCommand> _getPasteDocumentCommand;
        private readonly Func<IDocumentsExplorer, RefreshDocumentsExplorerCommand> _getRefreshDocumentsExplorerCommand;
        private readonly Func<IDocumentsExplorer, RenameCommand> _getRenameDocumentCommand;

        public RadTreeNode? CutTreeNode { get => _cutTreeNode; set => _cutTreeNode = value; }

        public RadTreeView TreeView => this.radTreeView1;

        public IDictionary<string, string> DocumentNames => documentNames;

        public IDictionary<string, string> ExpandedNodes => expandedNodes;

        public DocumentsExplorer(
            IConfigurationService configurationService,
            IDocumentsExplorerTreeViewBuilder documentsExplorerTreeViewBuilder,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IImageListService imageImageListService,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            Func<IDocumentsExplorer, AddExistingFileCommand> getAddExistingFileCommand,
            Func<IDocumentsExplorer, AddNewFileCommand> getAddNewFileCommand,
            Func<CloseProjectCommand> getCloseProjectCommand,
            Func<IDocumentsExplorer, CreateDirectoryCommand> getCreateDirectoryCommand,
            Func<IDocumentsExplorer, CutDocumentCommand> getCutDocumentCommand,
            Func<IDocumentsExplorer, DeleteCommand> getDeleteDocumentCommand,
            Func<IDocumentsExplorer, OpenFileCommand> getOpenFileCommand,
            Func<IDocumentsExplorer, PasteCommand> getPasteDocumentCommand,
            Func<IDocumentsExplorer, RefreshDocumentsExplorerCommand> getRefreshDocumentsExplorerCommand,
            Func<IDocumentsExplorer, RenameCommand> getRenameDocumentCommand)
        {
            _configurationService = configurationService;
            _documentsExplorerTreeViewBuilder = documentsExplorerTreeViewBuilder;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _imageImageListService = imageImageListService;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _getAddExistingFileCommand = getAddExistingFileCommand;
            _getAddNewFileCommand = getAddNewFileCommand;
            _getCloseProjectCommand = getCloseProjectCommand;
            _getCreateDirectoryCommand = getCreateDirectoryCommand;
            _getCutDocumentCommand = getCutDocumentCommand;
            _getDeleteDocumentCommand = getDeleteDocumentCommand;
            _getOpenFileCommand = getOpenFileCommand;
            _getPasteDocumentCommand = getPasteDocumentCommand;
            _getRefreshDocumentsExplorerCommand = getRefreshDocumentsExplorerCommand;
            _getRenameDocumentCommand = getRenameDocumentCommand;

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
                throw _exceptionHelper.CriticalException("{905A8EA2-4C2D-4B99-AAB4-5B0D00CB4E03}");

            //Can't create until the project is opened and CreateProfile is called.
            //It should always be null if CreateProfile is called only when the project opens.
            CreateFileSystemWatcher();

            BuildTreeView();
        }

        public void RefreshTreeView()
        {
            //if (InvokeRequired)
            //{
            //    this.Invoke(BuildTreeView);
            //}
            //else
            //{
            //    BuildTreeView();
            //}
            BuildTreeView();
        }

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
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

        private void CreateContextMenu()
        {
            AddClickCommand(mnuItemOpenFile, _getOpenFileCommand(this));
            AddClickCommand(mnuItemRename, _getRenameDocumentCommand(this));
            AddClickCommand(mnuItemDelete, _getDeleteDocumentCommand(this));
            AddClickCommand(mnuItemAddNewFile, _getAddNewFileCommand(this));
            AddClickCommand(mnuItemAddExistingFile, _getAddExistingFileCommand(this));
            AddClickCommand(mnuItemCreateDirectory, _getCreateDirectoryCommand(this));
            AddClickCommand(mnuItemCut, _getCutDocumentCommand(this));
            AddClickCommand(mnuItemPaste, _getPasteDocumentCommand(this));
            AddClickCommand(mnuItemCloseProject, _getCloseProjectCommand());
            AddClickCommand(mnuItemRefresh, _getRefreshDocumentsExplorerCommand(this));

            mnuItemAddFile.Items.AddRange
            (
                new RadItem[] 
                {
                    mnuItemAddNewFile,
                    mnuItemAddExistingFile
                }
            );

            radTreeView1.RadContextMenu = new()
            {
                ImageList = _imageImageListService.ImageList,
                Items =
                {
                    mnuItemOpenFile,
                    mnuItemRename,
                    mnuItemDelete,
                    mnuItemAddFile,
                    mnuItemCreateDirectory,
                    new RadMenuSeparatorItem(),
                    mnuItemCut,
                    mnuItemPaste,
                    new RadMenuSeparatorItem(),
                    mnuItemCloseProject,
                    mnuItemRefresh
                }
            };
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
            this.radTreeView1.MouseDown += RadTreeView1_MouseDown;
            this.radTreeView1.NodeFormatting += RadTreeView1_NodeFormatting;
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            documentProfileErrors.ErrorCountChanged += DocumentProfileErrors_ErrorCountChanged;
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += DocumentsExplorer_Disposed;

            CreateContextMenu();
        }

        private void SetContextMenuState(RadTreeNode selectedNode)
        {
            mnuItemOpenFile.Enabled = EnableOpenFile();
            mnuItemPaste.Enabled = _cutTreeNode != null;
            mnuItemCut.Enabled = selectedNode != this.radTreeView1.Nodes[0];

            mnuItemAddFile.Enabled = documentProfileErrors.Count == 0;

            bool EnableOpenFile()
            {
                if (documentProfileErrors.Count > 0)
                    return false;

                return !_treeViewService.IsFolderNode(selectedNode)
                    && !_treeViewService.IsRootNode(selectedNode);
            }
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

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e) { RadMessageBox.Show("AAAA" + e.GetException().Message); ; }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e) 
            => BuildTreeViewThreadSafe();

        private void RadTreeView1_CreateNodeElement(object sender, CreateTreeNodeElementEventArgs e)
        {
            e.NodeElement = new StateImageTreeNodeElement(_exceptionHelper);
        }

        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.radTreeView1.GetNodeAt(e.Location);
            if (treeNode == null && this.radTreeView1.Nodes.Count > 0)
            {
                this.radTreeView1.SelectedNode = this.radTreeView1.Nodes[0];
                SetContextMenuState(this.radTreeView1.SelectedNode);
            }
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

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            this.radTreeView1.SelectedNode = e.Node;
            if (this.radTreeView1.SelectedNode != null)
                SetContextMenuState(this.radTreeView1.SelectedNode);

        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args) 
            => SetTreeViewBorderColor(documentProfileErrors.Count);
        #endregion Event Handlers
    }
}
