using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ConfigurationExplorer : UserControl, IConfigurationExplorer
    {
        private readonly IConfigurationExplorerTreeViewBuilder _configurationExplorerTreeViewBuilder;
        private readonly IImageListService _imageImageListService;
        private readonly ITreeViewService _treeViewService;

        private readonly Func<IConfigurationExplorer, EditConfigurationCommand> _getEditConfigurationCommand;
        private readonly Func<IConfigurationExplorer, RefreshConfigurationExplorerCommand> _getRefreshConfigurationExplorerCommand;


        private readonly RadMenuItem mnuItemEdit = new(Strings.mnuItemEditText) { ImageIndex = ImageIndexes.EDITIMAGEINDEX };
        private readonly RadMenuItem mnuItemRefresh = new(Strings.mnuItemRefreshText) { ImageIndex = ImageIndexes.REFRESHIMAGEINDEX };

        public ConfigurationExplorer(
            IConfigurationExplorerTreeViewBuilder configurationExplorerTreeViewBuilder,
            IImageListService imageImageListService,
            ITreeViewService treeViewService,
            Func<IConfigurationExplorer, EditConfigurationCommand> getEditConfigurationCommand,
            Func<IConfigurationExplorer, RefreshConfigurationExplorerCommand> getRefreshConfigurationExplorerCommand)
        {
            _configurationExplorerTreeViewBuilder = configurationExplorerTreeViewBuilder;
            _imageImageListService = imageImageListService;
            _treeViewService = treeViewService;
            _getEditConfigurationCommand = getEditConfigurationCommand;
            _getRefreshConfigurationExplorerCommand = getRefreshConfigurationExplorerCommand;

            InitializeComponent();
            Initialize();
        }

        public RadTreeView TreeView => this.radTreeView1;

        public void ClearProfile()
        {
            radTreeView1.Nodes.Clear();
        }

        public void CreateProfile()
        {
            BuildTreeView();
        }

        public void RefreshTreeView()
            => BuildTreeView();

        private void BuildTreeView()
            => _configurationExplorerTreeViewBuilder.Build(radTreeView1);

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private void CreateContextMenu()
        {
            AddClickCommand(mnuItemEdit, _getEditConfigurationCommand(this));
            AddClickCommand(mnuItemRefresh, _getRefreshConfigurationExplorerCommand(this));

            radTreeView1.RadContextMenu = new()
            {
                ImageList = _imageImageListService.ImageList,
                Items =
                {
                    new RadMenuSeparatorItem(),
                    mnuItemEdit,
                    new RadMenuSeparatorItem(),
                    mnuItemRefresh,
                    new RadMenuSeparatorItem(),
                }
            };
        }

        private void Initialize()
        {
            this.radTreeView1.MouseDown += RadTreeView1_MouseDown;
            this.radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
            CreateContextMenu();
        }

        private void SetContextMenuState(RadTreeNode selectedNode)
        {
            mnuItemEdit.Enabled = _treeViewService.IsFileNode(selectedNode);
        }

        #region Event Handlers
        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            this.radTreeView1.SelectedNode = e.Node;
            if (this.radTreeView1.SelectedNode != null)
                SetContextMenuState(this.radTreeView1.SelectedNode);
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
        #endregion Event Handlers
    }
}
