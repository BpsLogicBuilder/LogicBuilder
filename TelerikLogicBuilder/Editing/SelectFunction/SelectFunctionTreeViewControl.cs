using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal partial class SelectFunctionTreeViewControl : UserControl, ISelectFunctionTreeViewControl
    {
        private readonly ISelectFunctionTreeViewBuilder _selectFunctionTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly ISelectFunctionControl selectFunctionControl;

        public SelectFunctionTreeViewControl(
            ITreeViewBuilderFactory treeViewBuilderFactory,
            ITreeViewService treeViewService,
            ISelectFunctionControl selectFunctionControl)
        {
            InitializeComponent();
            _selectFunctionTreeViewBuilder = treeViewBuilderFactory.GetSelectFunctionTreeViewBuilder(selectFunctionControl);
            _treeViewService = treeViewService;
            this.selectFunctionControl = selectFunctionControl;
            Initialize();
        }

        public event EventHandler? Changed;

        public string FunctionName => radTreeView1.SelectedNode?.Text ?? string.Empty;

        public bool ItemSelected => radTreeView1.SelectedNode != null && _treeViewService.IsMethodNode(radTreeView1.SelectedNode);

        public void SelectFunction(string functionName)
        {
            if (functionName.Trim().Length == 0)
                return;

            if (!selectFunctionControl.FunctionDictionary.TryGetValue(functionName, out Function? function))
            {
                selectFunctionControl.SetErrorMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionName)
                );
                return;
            }

            var node = radTreeView1.Find(n => n.Text == function.Name);
            if (node != null)
                radTreeView1.SelectedNode = node;
        }

        private void Initialize()
        {
            Disposed += SelectFunctionTreeViewControl_Disposed;
            radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;
            radTreeView1.SelectedNodeChanged += RadTreeView1_SelectedNodeChanged;
            radTreeView1.TreeViewElement.ShowNodeToolTips = true;
            radTreeView1.ShowRootLines = false;
            radTreeView1.HideSelection = false;
            LoadTreeview();
        }

        private void LoadTreeview()
        {
            _selectFunctionTreeViewBuilder.Build(radTreeView1);
            if (radTreeView1.Nodes.Count > 0)
                radTreeView1.SelectedNode ??= radTreeView1.Nodes[0];
        }

        private void RemoveEventHandlers()
        {
            radTreeView1.NodeExpandedChanged -= RadTreeView1_NodeExpandedChanged;
            radTreeView1.SelectedNodeChanged -= RadTreeView1_SelectedNodeChanged;
        }

        #region Event Handlers
        private void RadTreeView1_NodeExpandedChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            if (!_treeViewService.IsFolderNode(e.Node))/*NodeExpandedChanged runs for non-folder nodes on double click*/
                return;

            if (e.Node.Expanded)
            {
                if (!selectFunctionControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectFunctionControl.ExpandedNodes.Add(e.Node.Name, e.Node.Text);
            }
            else
            {
                if (selectFunctionControl.ExpandedNodes.ContainsKey(e.Node.Name))
                    selectFunctionControl.ExpandedNodes.Remove(e.Node.Name);
            }

            e.Node.ImageIndex = e.Node.Expanded
                    ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }

        private void RadTreeView1_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        private void SelectFunctionTreeViewControl_Disposed(object? sender, EventArgs e)
        {
            RemoveEventHandlers();
            _treeViewService.ClearImageLists(radTreeView1);
        }
        #endregion Event Handlers
    }
}
