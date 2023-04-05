using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal partial class SelectConstructorTreeViewControl : UserControl, ISelectConstructorTreeViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISelectConstructorTreeViewBuilder _selectConstructorTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly ISelectConstructorControl selectConstructorControl;

        public SelectConstructorTreeViewControl(
            IConfigurationService configurationService,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            ISelectConstructorControl selectConstructorControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _selectConstructorTreeViewBuilder = treeViewBuilderFactory.GetSelectConstructorTreeViewBuilder(selectConstructorControl);
            _treeViewService = treeViewService;
            this.selectConstructorControl = selectConstructorControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string ConstructorName => radTreeView1.SelectedNode?.Text ?? string.Empty;

        public bool ItemSelected => radTreeView1.SelectedNode != null && _treeViewService.IsConstructorNode(radTreeView1.SelectedNode);

        public void SelectConstructor(string constructorName)
        {
            if (constructorName.Trim().Length == 0)
                return;

            if (!_configurationService.ConstructorList.Constructors.TryGetValue(constructorName, out Constructor? constructor))
            {
                selectConstructorControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredFormat, constructorName)
                );
                return;
            }

            var node = radTreeView1.Find(n => n.Text == constructor.Name);
            if (node != null)
                radTreeView1.SelectedNode = node;
        }

        private void Initialize()
        {
            radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            radTreeView1.SelectedNodeChanged += RadTreeView1_SelectedNodeChanged;
            radTreeView1.TreeViewElement.ShowNodeToolTips = true;
            radTreeView1.ShowRootLines = false;
            radTreeView1.HideSelection = false;
            LoadTreeview();
        }

        private void LoadTreeview()
        {
            _selectConstructorTreeViewBuilder.Build(radTreeView1);
            if (radTreeView1.Nodes.Count > 0)
                radTreeView1.SelectedNode ??= radTreeView1.Nodes[0];
        }

        #region Event Handlers
        private void RadTreeView1_NodeExpandedChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            if (!_treeViewService.IsFolderNode(e.Node))/*NodeExpandedChanged runs for non-folder nodes on double click*/
                return;

            if (e.Node.Expanded)
            {
                if (!selectConstructorControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectConstructorControl.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (selectConstructorControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectConstructorControl.ExpandedNodes.Remove(e.Node.Name);
            }

            e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }

        private void RadTreeView1_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            Changed?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
