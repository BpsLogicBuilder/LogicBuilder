using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ConfigurationExplorer : UserControl, IConfigurationExplorer
    {
        private readonly IConfigurationExplorerTreeViewBuilder _configurationExplorerTreeViewBuilder;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;

        private readonly EditConfigurationCommand _editConfigurationCommand;
        private readonly RefreshConfigurationExplorerCommand _refreshConfigurationExplorerCommand;

        private readonly RadMenuItem mnuItemEdit = new(Strings.mnuItemEditText) { ImageIndex = ImageIndexes.EDITIMAGEINDEX };
        private readonly RadMenuItem mnuItemRefresh = new(Strings.mnuItemRefreshText) { ImageIndex = ImageIndexes.REFRESHIMAGEINDEX };

        public ConfigurationExplorer(
            IConfigurationExplorerTreeViewBuilder configurationExplorerTreeViewBuilder,
            IImageListService imageListService,
            ITreeViewService treeViewService,
            EditConfigurationCommand editConfigurationCommand,
            RefreshConfigurationExplorerCommand refreshConfigurationExplorerCommand)
        {
            _configurationExplorerTreeViewBuilder = configurationExplorerTreeViewBuilder;
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _editConfigurationCommand = editConfigurationCommand;
            _refreshConfigurationExplorerCommand = refreshConfigurationExplorerCommand;

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
            AddClickCommand(mnuItemEdit, _editConfigurationCommand);
            AddClickCommand(mnuItemRefresh, _refreshConfigurationExplorerCommand);

            radTreeView1.RadContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
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
            this.radTreeView1.NodeMouseDoubleClick += RadTreeView1_NodeMouseDoubleClick;
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

        private void RadTreeView1_NodeMouseDoubleClick(object sender, RadTreeViewEventArgs e) 
            => _editConfigurationCommand.Execute();

        private void RadTreeView1_MouseDown(object? sender, MouseEventArgs e)
        {//handles case in which clicked area doesn't have a node
            RadTreeNode treeNode = this.radTreeView1.GetNodeAt(e.Location);
            if (treeNode == null && this.radTreeView1.Nodes.Count > 0)
            {
                //this.radTreeView1.SelectedNode = this.radTreeView1.Nodes[0];
                //SetContextMenuState(this.radTreeView1.SelectedNode);
            }
        }
        #endregion Event Handlers
    }
}
