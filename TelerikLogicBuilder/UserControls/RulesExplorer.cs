using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RulesExplorer : UserControl, IRulesExplorer
    {
        private readonly IMainWindow _mainWindow;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewBuilderFactory _treeViewBuiilderFactory;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;
        private readonly Dictionary<string, string> expandedNodes = new();

        private readonly IDisposable refreshTreeViewSubscription;

        private readonly RadMenuItem mnuItemDeleteAllRules = new(Strings.mnuItemDeleteAllRulesText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly RadMenuItem mnuItemValidate = new(Strings.mnuItemValidateRulesText);
        private readonly RadMenuItem mnuItemView = new(Strings.mnuItemViewRulesText);
        private readonly RadMenuItem mnuItemRefresh = new(Strings.mnuItemRefreshText) { ImageIndex = ImageIndexes.REFRESHIMAGEINDEX };

        private readonly DeleteAllRulesCommand _deleteAllRulesCommand;
        private readonly DeleteRulesExplorerFileCommand _deleteRulesExplorerFileCommand;
        private readonly ValidateCommand _validateCommand;
        private readonly ViewCommand _viewCommand;
        private readonly RefreshRulesExplorerCommand _refreshRulesExplorerCommand;
        private EventHandler mnuItemDeleteAllRulesClickHandler;
        private EventHandler mnuItemDeleteClickHandler;
        private EventHandler mnuItemValidateClickHandler;
        private EventHandler mnuItemViewClickHandler;
        private EventHandler mnuItemRefreshClickHandler;

        public RadTreeView TreeView => this.radTreeView1;

        public IDictionary<string, string> ExpandedNodes => expandedNodes;

        public RulesExplorer(
            IMainWindow mainWindow,
            IImageListService imageListService,
            ITreeViewBuilderFactory treeViewBuiilderFactory,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService,
            DeleteAllRulesCommand deleteAllRulesCommand,
            DeleteRulesExplorerFileCommand deleteRulesExplorerFileCommand,
            ValidateCommand validateCommand,
            ViewCommand viewCommand,
            RefreshRulesExplorerCommand refreshRulesExplorerCommand)
        {
            _mainWindow = mainWindow;
            _imageListService = imageListService;
            _treeViewBuiilderFactory = treeViewBuiilderFactory;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _deleteAllRulesCommand = deleteAllRulesCommand;
            _deleteRulesExplorerFileCommand = deleteRulesExplorerFileCommand;
            _validateCommand = validateCommand;
            _viewCommand = viewCommand;
            _refreshRulesExplorerCommand = refreshRulesExplorerCommand;

            refreshTreeViewSubscription = _uiNotificationService.RulesExplorerRefreshSubject.Subscribe(RefreshTreeView);

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
            mnuItemDeleteAllRules.Click += mnuItemDeleteAllRulesClickHandler;
            mnuItemDelete.Click += mnuItemDeleteClickHandler;
            mnuItemValidate.Click += mnuItemValidateClickHandler;
            mnuItemView.Click += mnuItemViewClickHandler;
            mnuItemRefresh.Click += mnuItemRefreshClickHandler;
        }

        private void BuildTreeView()
            => _treeViewBuiilderFactory.GetRulesExplorerTreeViewBuilder(expandedNodes).Build
            (
                radTreeView1
            );

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemDeleteAllRulesClickHandler),
            nameof(mnuItemDeleteClickHandler),
            nameof(mnuItemValidateClickHandler),
            nameof(mnuItemViewClickHandler),
            nameof(mnuItemRefreshClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void CreateContextMenu()
        {
            mnuItemDeleteAllRulesClickHandler = AddClickCommand(_deleteAllRulesCommand);
            mnuItemDeleteClickHandler = AddClickCommand(_deleteRulesExplorerFileCommand);
            mnuItemValidateClickHandler = AddClickCommand(_validateCommand);
            mnuItemViewClickHandler = AddClickCommand(_viewCommand);
            mnuItemRefreshClickHandler = AddClickCommand(_refreshRulesExplorerCommand);

            radTreeView1.RadContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items =
                {
                    mnuItemDeleteAllRules,
                    mnuItemDelete,
                    mnuItemValidate,
                    mnuItemView,
                    new RadMenuSeparatorItem(),
                    mnuItemRefresh
                }
            };
        }

        private static void Dispose(IDisposable disposable)
        {
            disposable?.Dispose();
        }

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
        [MemberNotNull(nameof(mnuItemDeleteAllRulesClickHandler),
            nameof(mnuItemDeleteClickHandler),
            nameof(mnuItemValidateClickHandler),
            nameof(mnuItemViewClickHandler),
            nameof(mnuItemRefreshClickHandler))]
#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
        private void Initialize()
        {
            this.radTreeView1.MouseDown += RadTreeView1_MouseDown;
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            this.radTreeView1.NodeMouseDoubleClick += RadTreeView1_NodeMouseDoubleClick;
            this.Disposed += RulesExplorer_Disposed;
            this.Load += RulesExplorer_Load;

            CreateContextMenu();
            AddClickCommands();
        }

        private void RefreshTreeView(bool refresh) 
            => BuildTreeView();

        private void RemoveClickCommands()
        {
            mnuItemDeleteAllRules.Click -= mnuItemDeleteAllRulesClickHandler;
            mnuItemDelete.Click -= mnuItemDeleteClickHandler;
            mnuItemValidate.Click -= mnuItemValidateClickHandler;
            mnuItemView.Click -= mnuItemViewClickHandler;
            mnuItemRefresh.Click -= mnuItemRefreshClickHandler;
        }

        private void RemoveEventHandlers()
        {
            this.radTreeView1.MouseDown -= RadTreeView1_MouseDown;
            this.radTreeView1.NodeExpandedChanged -= RadTreeView1_NodeExpandedChanged;
            this.radTreeView1.NodeMouseClick -= RadTreeView1_NodeMouseClick;
            this.radTreeView1.NodeMouseDoubleClick -= RadTreeView1_NodeMouseDoubleClick;
            this.Load -= RulesExplorer_Load;
        }

        private void SetContextMenuState(RadTreeNode selectedNode)
        {
            mnuItemView.Enabled = _treeViewService.IsFileNode(selectedNode);
            mnuItemValidate.Enabled = CanValidate();
            mnuItemDelete.Enabled = _treeViewService.IsFileNode(selectedNode);

            bool CanValidate()
            {
                if (!_treeViewService.IsFileNode(selectedNode))
                    return false;

                if (!selectedNode.Name.EndsWith(FileExtensions.RULESFILEEXTENSION, true, CultureInfo.InvariantCulture))
                    return false;

                return true;
            }
        }

        #region Event Handlers
        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.radTreeView1.GetNodeAt(e.Location);
            if (treeNode == null && this.radTreeView1.Nodes.Count > 0)
            {
                //this.radTreeView1.SelectedNode = this.radTreeView1.Nodes[0];
                //SetContextMenuState(this.radTreeView1.SelectedNode);
            }
        }

        private void RadTreeView1_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e) 
            => _viewCommand.Execute();

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

            e.Node.ImageIndex = e.Node.Expanded
                ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            this.radTreeView1.SelectedNode = e.Node;
            if (this.radTreeView1.SelectedNode != null)
                SetContextMenuState(this.radTreeView1.SelectedNode);
        }

        private void RulesExplorer_Disposed(object? sender, EventArgs e)
        {
            RemoveClickCommands();
            RemoveEventHandlers();
            Dispose(refreshTreeViewSubscription);
        }

        private void RulesExplorer_Load(object? sender, EventArgs e)
        {
            _mainWindow.Instance.Activated += MainWind_Activated;
        }

        private void MainWind_Activated(object? sender, EventArgs e)
        {
            //RefreshTreeView();
            //seems like overkill - causes flickering
        }
        #endregion Event Handlers
    }
}
