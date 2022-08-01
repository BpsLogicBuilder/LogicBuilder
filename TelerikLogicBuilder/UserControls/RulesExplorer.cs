using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class RulesExplorer : UserControl, IRulesExplorer
    {
        private readonly IMainWindow _mainWindow;
        private readonly IImageListService _imageImageListService;
        private readonly IRulesExplorerTreeViewBuilder _rulesExplorerTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly Dictionary<string, string> expandedNodes = new();

        private readonly IDisposable refreshTreeViewSubscription;

        private readonly RadMenuItem mnuItemDeleteAllRules = new(Strings.mnuItemDeleteAllRulesText);
        private readonly RadMenuItem mnuItemDelete = new(Strings.mnuItemDeleteText) { ImageIndex = ImageIndexes.DELETEIMAGEINDEX };
        private readonly RadMenuItem mnuItemValidate = new(Strings.mnuItemValidateRulesText);
        private readonly RadMenuItem mnuItemView = new(Strings.mnuItemViewRulesText);
        private readonly RadMenuItem mnuItemRefresh = new(Strings.mnuItemRefreshText) { ImageIndex = ImageIndexes.REFRESHIMAGEINDEX };

        private readonly Func<IRulesExplorer, DeleteAllRulesCommand> _getDeleteAllRulesCommand;
        private readonly Func<IRulesExplorer, DeleteRulesExplorerFileCommand> _getDeleteRulesExplorerFileCommand;
        private readonly Func<IRulesExplorer, ValidateCommand> _getValidateCommand;
        private readonly Func<IRulesExplorer, ViewCommand> _getViewCommand;
        private readonly Func<IRulesExplorer, RefreshRulesExplorerCommand> _getRefreshRulesExplorerCommand;

        public RadTreeView TreeView => this.radTreeView1;

        public IDictionary<string, string> ExpandedNodes => expandedNodes;

        public RulesExplorer(
            IMainWindow mainWindow,
            IImageListService imageImageListService,
            IRulesExplorerTreeViewBuilder rulesExplorerTreeViewBuilder,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            Func<IRulesExplorer, DeleteAllRulesCommand> getDeleteAllRulesCommand,
            Func<IRulesExplorer, DeleteRulesExplorerFileCommand> getDeleteRulesExplorerFileCommand,
            Func<IRulesExplorer, ValidateCommand> getValidateCommand,
            Func<IRulesExplorer, ViewCommand> getViewCommand,
            Func<IRulesExplorer, RefreshRulesExplorerCommand> getRefreshRulesExplorerCommand)
        {
            _mainWindow = mainWindow;
            _imageImageListService = imageImageListService;
            _rulesExplorerTreeViewBuilder = rulesExplorerTreeViewBuilder;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _getDeleteAllRulesCommand = getDeleteAllRulesCommand;
            _getDeleteRulesExplorerFileCommand = getDeleteRulesExplorerFileCommand;
            _getValidateCommand = getValidateCommand;
            _getViewCommand = getViewCommand;
            _getRefreshRulesExplorerCommand = getRefreshRulesExplorerCommand;

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

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private void BuildTreeView()
            => _rulesExplorerTreeViewBuilder.Build
            (
                radTreeView1,
                expandedNodes
            );

        private void CreateContextMenu()
        {
            AddClickCommand(mnuItemDeleteAllRules, _getDeleteAllRulesCommand(this));
            AddClickCommand(mnuItemDelete, _getDeleteRulesExplorerFileCommand(this));
            AddClickCommand(mnuItemValidate, _getValidateCommand(this));
            AddClickCommand(mnuItemView, _getViewCommand(this));
            AddClickCommand(mnuItemRefresh, _getRefreshRulesExplorerCommand(this));

            radTreeView1.RadContextMenu = new()
            {
                ImageList = _imageImageListService.ImageList,
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
            if (disposable != null)
                disposable.Dispose();
        }

        private void Initialize()
        {
            this.radTreeView1.MouseDown += RadTreeView1_MouseDown;
            this.radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            this.radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            this.Disposed += RulesExplorer_Disposed;
            this.Load += RulesExplorer_Load;

            CreateContextMenu();
        }

        private void RefreshTreeView(bool refresh) 
            => BuildTreeView();

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

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            this.radTreeView1.SelectedNode = e.Node;
            if (this.radTreeView1.SelectedNode != null)
                SetContextMenuState(this.radTreeView1.SelectedNode);
        }

        private void RulesExplorer_Disposed(object? sender, EventArgs e)
        {
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
