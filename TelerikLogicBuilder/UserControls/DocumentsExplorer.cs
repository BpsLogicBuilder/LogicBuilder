using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class DocumentsExplorer : UserControl, IDocumentsExplorer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageImageListService;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewBuiilderFactory _treeViewBuiilderFactory;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;
        private readonly DocumentExplorerErrorsList documentProfileErrors = new();
        private readonly Dictionary<string, string> documentNames = new();
        private readonly Dictionary<string, string> expandedNodes = new();

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

        private readonly AddExistingFileCommand _addExistingFileCommand;
        private readonly AddNewFileCommand _addNewFileCommand;
        private readonly CloseProjectCommand _closeProjectCommand;
        private readonly CreateDirectoryCommand _createDirectoryCommand;
        private readonly CutCommand _cutCommand;
        private readonly DeleteCommand _deleteDocumentCommand;
        private readonly OpenFileCommand _openFileCommand;
        private readonly PasteCommand _pasteDocumentCommand;
        private readonly RefreshDocumentsExplorerCommand _refreshDocumentsExplorerCommand;
        private readonly RenameCommand _renameDocumentCommand;

        private readonly RadTreeView radTreeView1;

        public IList<RadTreeNode> CutTreeNodes { get; } = new List<RadTreeNode>();

        public RadTreeView TreeView => this.radTreeView1;

        public IDictionary<string, string> DocumentNames => documentNames;

        public IDictionary<string, string> ExpandedNodes => expandedNodes;

        public DocumentsExplorer(
            IExceptionHelper exceptionHelper,
            IImageListService imageImageListService,
            IMainWindow mainWindow,
            ITreeViewBuiilderFactory treeViewBuiilderFactory,
            ITreeViewService treeViewService,
            FileSystemTreeView fileSystemTreeView,
            IUiNotificationService uiNotificationService,
            AddExistingFileCommand addExistingFileCommand,
            AddNewFileCommand addNewFileCommand,
            CloseProjectCommand closeProjectCommand,
            CreateDirectoryCommand createDirectoryCommand,
            CutCommand cutCommand,
            DeleteCommand deleteDocumentCommand,
            OpenFileCommand openFileCommand,
            PasteCommand pasteDocumentCommand,
            RefreshDocumentsExplorerCommand refreshDocumentsExplorerCommand,
            RenameCommand renameDocumentCommand)
        {
            _exceptionHelper = exceptionHelper;
            _imageImageListService = imageImageListService;
            _mainWindow = mainWindow;
            _treeViewBuiilderFactory = treeViewBuiilderFactory;
            _treeViewService = treeViewService;           
            _uiNotificationService = uiNotificationService;
            _addExistingFileCommand = addExistingFileCommand;
            _addNewFileCommand = addNewFileCommand;
            _closeProjectCommand = closeProjectCommand;
            _createDirectoryCommand = createDirectoryCommand;
            _cutCommand = cutCommand;
            _deleteDocumentCommand = deleteDocumentCommand;
            _openFileCommand = openFileCommand;
            _pasteDocumentCommand = pasteDocumentCommand;
            _refreshDocumentsExplorerCommand = refreshDocumentsExplorerCommand;
            _renameDocumentCommand = renameDocumentCommand;

            this.radTreeView1 = fileSystemTreeView;

            InitializeComponent();
            Initialize();
        }

        public void ClearProfile()
        {
            radTreeView1.Nodes.Clear();
            expandedNodes.Clear();
        }

        public void CreateProfile()
        {
            BuildTreeView();
        }

        public void RefreshTreeView() 
            => BuildTreeView();

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private void BuildTreeView() 
            => _treeViewBuiilderFactory.GetDocumentsExplorerTreeViewBuilder
            (
                documentNames,
                documentProfileErrors,
                expandedNodes
            ).Build(radTreeView1);

        private void CreateContextMenu()
        {
            AddClickCommand(mnuItemOpenFile, _openFileCommand);
            AddClickCommand(mnuItemRename, _renameDocumentCommand);
            AddClickCommand(mnuItemDelete, _deleteDocumentCommand);
            AddClickCommand(mnuItemAddNewFile, _addNewFileCommand);
            AddClickCommand(mnuItemAddExistingFile, _addExistingFileCommand);
            AddClickCommand(mnuItemCreateDirectory, _createDirectoryCommand);
            AddClickCommand(mnuItemCut, _cutCommand);
            AddClickCommand(mnuItemPaste, _pasteDocumentCommand);
            AddClickCommand(mnuItemCloseProject, _closeProjectCommand);
            AddClickCommand(mnuItemRefresh, _refreshDocumentsExplorerCommand);

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

        private void Initialize()
        {
            ((ISupportInitialize)this.radTreeView1).BeginInit();
            this.SuspendLayout();
            this.radTreeView1.Dock = DockStyle.Fill;
            this.radTreeView1.Location = new System.Drawing.Point(0, 0);
            this.radTreeView1.Name = "radTreeView1";
            this.radTreeView1.Size = new System.Drawing.Size(450, 635);
            this.radTreeView1.SpacingBetweenNodes = -1;
            this.radTreeView1.TabIndex = 0;
            this.Controls.Add(this.radTreeView1);
            ((ISupportInitialize)this.radTreeView1).EndInit();
            this.ResumeLayout(false);

            this.radTreeView1.AllowDragDrop = true;
            this.radTreeView1.MultiSelect = true;

            this.radTreeView1.CreateNodeElement += RadTreeView1_CreateNodeElement;
            this.radTreeView1.MouseDown += RadTreeView1_MouseDown;
            this.radTreeView1.NodeFormatting += RadTreeView1_NodeFormatting;
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            this.radTreeView1.NodeMouseDoubleClick += RadTreeView1_NodeMouseDoubleClick;
            documentProfileErrors.ErrorCountChanged += DocumentProfileErrors_ErrorCountChanged;
            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.Disposed += DocumentsExplorer_Disposed;
            this.Load += DocumentsExplorer_Load;

            CreateContextMenu();
        }

        private void SetContextMenuState(IList<RadTreeNode> selectedNodes)
        {
            mnuItemOpenFile.Enabled = EnableOpenFile();
            mnuItemRename.Enabled = selectedNodes.Count == 1;
            mnuItemDelete.Enabled = selectedNodes.Count > 0;
            mnuItemAddFile.Enabled = selectedNodes.Count == 1 && documentProfileErrors.Count == 0;
            mnuItemCreateDirectory.Enabled = selectedNodes.Count == 1;
            mnuItemCut.Enabled = selectedNodes.Count > 0 && this.radTreeView1.Nodes[0].Selected == false;
            mnuItemPaste.Enabled = CutTreeNodes.Count > 0 && selectedNodes.Count == 1;

            bool EnableOpenFile()
            {
                if (selectedNodes.Count != 1 || documentProfileErrors.Count > 0)
                {
                    return false;
                }

                return _treeViewService.IsFileNode(selectedNodes[0]);
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

        private void DocumentsExplorer_Disposed(object? sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
        }

        private void DocumentsExplorer_Load(object? sender, EventArgs e)
        {
            _mainWindow.Instance.Activated += MainWindow_Activated;//_mainWindow.Instance unavailable in the constructor
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            RefreshTreeView();
        }

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
                SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
            }
        }

        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node)
                || _treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for file nodes on double click*/
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

            if (CutTreeNodes.ToHashSet().Contains(e.Node))
            {
                e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
            else
            {
                e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
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
            SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
        }

        private void RadTreeView1_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e) 
            => _openFileCommand.Execute();

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args) 
            => SetTreeViewBorderColor(documentProfileErrors.Count);
        #endregion Event Handlers
    }
}
