using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal partial class SelectVariableTreeViewControl : UserControl, ISelectVariableTreeViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly ISelectVariableTreeViewBuilder _selectVariableTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly ISelectVariableControl selectVariableControl;

        public SelectVariableTreeViewControl(
            IConfigurationService configurationService,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            ISelectVariableControl selectVariableControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _selectVariableTreeViewBuilder = treeViewBuilderFactory.GetSelectVariableTreeViewBuilder(selectVariableControl);
            _treeViewService = treeViewService;
            this.selectVariableControl = selectVariableControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string VariableName => radTreeView1.SelectedNode.Text;

        public bool ItemSelected => radTreeView1.SelectedNode != null && _treeViewService.IsVariableNode(radTreeView1.SelectedNode);

        public void SelectVariable(string variableName)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableName, out VariableBase? variable))
            {
                selectVariableControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.decisionNotConfiguredFormat2, variableName)
                );
                return;
            }

            var node = radTreeView1.Find(n => n.Text == variableName);
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
            _selectVariableTreeViewBuilder.Build(radTreeView1);
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
                if (!selectVariableControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectVariableControl.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (selectVariableControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectVariableControl.ExpandedNodes.Remove(e.Node.Name);
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
