using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal partial class SelectFragmentTreeViewControl : UserControl, ISelectFragmentTreeViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISelectFragmentTreeViewBuilder _selectFragmentTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly ISelectFragmentControl selectFragmentControl;

        public SelectFragmentTreeViewControl(
            IConfigurationService configurationService,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            ISelectFragmentControl selectFragmentControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _selectFragmentTreeViewBuilder = treeViewBuilderFactory.GetSelectFragmentTreeViewBuilder(selectFragmentControl);
            _treeViewService = treeViewService;
            this.selectFragmentControl = selectFragmentControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string FragmentName => radTreeView1.SelectedNode?.Text ?? string.Empty;

        public bool ItemSelected => radTreeView1.SelectedNode != null && _treeViewService.IsFileNode(radTreeView1.SelectedNode);

        public void SelectFragment(string fragmentName)
        {
            if (!_configurationService.FragmentList.Fragments.TryGetValue(fragmentName, out Fragment? fragment))
            {
                selectFragmentControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.fragmentNotConfiguredFormat, fragmentName)
                );
                return;
            }

            var node = radTreeView1.Find(n => n.Text == fragment.Name);
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
            _selectFragmentTreeViewBuilder.Build(radTreeView1);
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
                if (!selectFragmentControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectFragmentControl.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (selectFragmentControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectFragmentControl.ExpandedNodes.Remove(e.Node.Name);
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
