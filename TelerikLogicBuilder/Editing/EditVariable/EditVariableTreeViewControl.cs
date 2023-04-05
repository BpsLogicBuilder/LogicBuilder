using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable
{
    internal partial class EditVariableTreeViewControl : UserControl, IEditVariableTreeViewControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditVariableTreeViewBuilder _selectVariableTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly IEditVariableControl editVariableControl;

        public EditVariableTreeViewControl(
            IConfigurationService configurationService,
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            IEditVariableControl editVariableControl)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _selectVariableTreeViewBuilder = treeViewBuilderFactory.GetEditVariableTreeViewBuilder(editVariableControl);
            _treeViewService = treeViewService;
            this.editVariableControl = editVariableControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public new event EventHandler? Validated;

        public string VariableName => radTreeView1.SelectedNode?.Text ?? string.Empty;

        public bool ItemSelected => radTreeView1.SelectedNode != null && _treeViewService.IsVariableNode(radTreeView1.SelectedNode);

        public void SelectVariable(string variableName)
        {
            if (variableName.Trim().Length == 0)
                return;

            if (!_configurationService.VariableList.Variables.TryGetValue(variableName, out VariableBase? variable))
            {
                editVariableControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.decisionNotConfiguredFormat2, variableName)
                );
                return;
            }

            var node = radTreeView1.Find(n => n.Text == variable.Name);
            if (node != null)
                radTreeView1.SelectedNode = node;
        }

        private void Initialize()
        {
            radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            radTreeView1.SelectedNodeChanged += RadTreeView1_SelectedNodeChanged;
            radTreeView1.Validated += RadTreeView1_Validated;
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
                if (!editVariableControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    editVariableControl.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (editVariableControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    editVariableControl.ExpandedNodes.Remove(e.Node.Name);
            }

            e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }

        private void RadTreeView1_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private void RadTreeView1_Validated(object? sender, EventArgs e)
        {
            Validated?.Invoke(this, e);
        }
        #endregion Event Handlers
    }
}
