using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class DocumentsExplorer : UserControl, IDocumentsExplorer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewBuilderFactory _treeViewBuiilderFactory;
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
        private EventHandler mnuItemOpenFileClickHandler;
        private EventHandler mnuItemRenameClickHandler;
        private EventHandler mnuItemDeleteClickHandler;
        private EventHandler mnuItemAddNewFileClickHandler;
        private EventHandler mnuItemAddExistingFileClickHandler;
        private EventHandler mnuItemCreateDirectoryClickHandler;
        private EventHandler mnuItemCutClickHandler;
        private EventHandler mnuItemPasteClickHandler;
        private EventHandler mnuItemCloseProjectClickHandler;
        private EventHandler mnuItemRefreshClickHandler;

        public IList<RadTreeNode> CutTreeNodes { get; } = new List<RadTreeNode>();

        public RadTreeView TreeView => this.radTreeView1;

        public IDictionary<string, string> DocumentNames => documentNames;

        public IDictionary<string, string> ExpandedNodes => expandedNodes;

        public DocumentsExplorer(
            IComponentFactory componentFactory,
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            IMainWindow mainWindow,
            ITreeViewBuilderFactory treeViewBuiilderFactory,
            ITreeViewService treeViewService,
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
            _imageListService = imageListService;
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

            this.radTreeView1 = (RadTreeView)componentFactory.GetFileSystemTreeView();

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

        private static EventHandler AddClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            mnuItemOpenFile.Click += mnuItemOpenFileClickHandler;
            mnuItemRename.Click += mnuItemRenameClickHandler;
            mnuItemDelete.Click += mnuItemDeleteClickHandler;
            mnuItemAddNewFile.Click += mnuItemAddNewFileClickHandler;
            mnuItemAddExistingFile.Click += mnuItemAddExistingFileClickHandler;
            mnuItemCreateDirectory.Click += mnuItemCreateDirectoryClickHandler;
            mnuItemCut.Click += mnuItemCutClickHandler;
            mnuItemPaste.Click += mnuItemPasteClickHandler;
            mnuItemCloseProject.Click += mnuItemCloseProjectClickHandler;
            mnuItemRefresh.Click += mnuItemRefreshClickHandler;
        }

        private void BuildTreeView() 
            => _treeViewBuiilderFactory.GetDocumentsExplorerTreeViewBuilder
            (
                documentNames,
                documentProfileErrors,
                expandedNodes
            ).Build(radTreeView1);

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemOpenFileClickHandler),
            nameof(mnuItemRenameClickHandler),
            nameof(mnuItemDeleteClickHandler),
            nameof(mnuItemAddNewFileClickHandler),
            nameof(mnuItemAddExistingFileClickHandler),
            nameof(mnuItemCreateDirectoryClickHandler),
            nameof(mnuItemCutClickHandler),
            nameof(mnuItemPasteClickHandler),
            nameof(mnuItemCloseProjectClickHandler),
            nameof(mnuItemRefreshClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenu()
        {
            mnuItemOpenFileClickHandler = AddClickCommand(_openFileCommand);
            mnuItemRenameClickHandler = AddClickCommand(_renameDocumentCommand);
            mnuItemDeleteClickHandler = AddClickCommand(_deleteDocumentCommand);
            mnuItemAddNewFileClickHandler = AddClickCommand(_addNewFileCommand);
            mnuItemAddExistingFileClickHandler = AddClickCommand(_addExistingFileCommand);
            mnuItemCreateDirectoryClickHandler = AddClickCommand(_createDirectoryCommand);
            mnuItemCutClickHandler = AddClickCommand(_cutCommand);
            mnuItemPasteClickHandler = AddClickCommand(_pasteDocumentCommand);
            mnuItemCloseProjectClickHandler = AddClickCommand(_closeProjectCommand);
            mnuItemRefreshClickHandler = AddClickCommand(_refreshDocumentsExplorerCommand);

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
                ImageList = _imageListService.ImageList,
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

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemOpenFileClickHandler),
            nameof(mnuItemRenameClickHandler),
            nameof(mnuItemDeleteClickHandler),
            nameof(mnuItemAddNewFileClickHandler),
            nameof(mnuItemAddExistingFileClickHandler),
            nameof(mnuItemCreateDirectoryClickHandler),
            nameof(mnuItemCutClickHandler),
            nameof(mnuItemPasteClickHandler),
            nameof(mnuItemCloseProjectClickHandler),
            nameof(mnuItemRefreshClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
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
            AddClickCommands();
        }

        private void RemoveClickCommands()
        {
            mnuItemOpenFile.Click -= mnuItemOpenFileClickHandler;
            mnuItemRename.Click -= mnuItemRenameClickHandler;
            mnuItemDelete.Click -= mnuItemDeleteClickHandler;
            mnuItemAddNewFile.Click -= mnuItemAddNewFileClickHandler;
            mnuItemAddExistingFile.Click -= mnuItemAddExistingFileClickHandler;
            mnuItemCreateDirectory.Click -= mnuItemCreateDirectoryClickHandler;
            mnuItemCut.Click -= mnuItemCutClickHandler;
            mnuItemPaste.Click -= mnuItemPasteClickHandler;
            mnuItemCloseProject.Click -= mnuItemCloseProjectClickHandler;
            mnuItemRefresh.Click -= mnuItemRefreshClickHandler;
        }

        private void RemoveEventHandlers()
        {
            this.radTreeView1.CreateNodeElement -= RadTreeView1_CreateNodeElement;
            this.radTreeView1.MouseDown -= RadTreeView1_MouseDown;
            this.radTreeView1.NodeFormatting -= RadTreeView1_NodeFormatting;
            this.radTreeView1.NodeExpandedChanged -= RadTreeView1_NodeExpandedChanged;
            this.radTreeView1.NodeMouseClick -= RadTreeView1_NodeMouseClick;
            this.radTreeView1.NodeMouseDoubleClick -= RadTreeView1_NodeMouseDoubleClick;
            documentProfileErrors.ErrorCountChanged -= DocumentProfileErrors_ErrorCountChanged;
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
            this.Load -= DocumentsExplorer_Load;
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
            RemoveClickCommands();
            RemoveEventHandlers();
        }

        private void DocumentsExplorer_Load(object? sender, EventArgs e)
        {
            _mainWindow.Instance.Activated += MainWindow_Activated;//_mainWindow.Instance unavailable in the constructor
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            //RefreshTreeView(); causes exception if project is closed
        }

        private void RadTreeView1_CreateNodeElement(object sender, CreateTreeNodeElementEventArgs e)
        {
            e.NodeElement = new StateImageTreeNodeElement();
        }

        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.radTreeView1.GetNodeAt(e.Location);
            if (treeNode == null && this.radTreeView1.Nodes.Count > 0)
            {
                //this.radTreeView1.SelectedNode = this.radTreeView1.Nodes[0];
                //SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
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
                //There's no need to guard Dictionary.Remove(key) with Dictionary.ContainsKey(key). Dictionary<TKey,TValue>.Remove(TKey) already checks whether the key exists and doesn't throw if it doesn't exist.
                //https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1853.
                expandedNodes.Remove(e.Node.Name);
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
            SetContextMenuState(_treeViewService.GetSelectedNodes(TreeView));
        }

        private void RadTreeView1_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e) 
            => _openFileCommand.Execute();

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args) 
            => SetTreeViewBorderColor(documentProfileErrors.Count);
        #endregion Event Handlers
    }
}
