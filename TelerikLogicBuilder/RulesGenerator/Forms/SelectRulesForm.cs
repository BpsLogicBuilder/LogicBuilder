using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectRulesForm : RadForm, ISelectRulesForm
    {
        private readonly string _applicationName;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodeNames;
        private readonly ISelectRulesTreeViewBuilder _selectRulesTreeViewBuilder;
        private readonly ITreeViewService _treeViewService;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public SelectRulesForm(
            IFormInitializer formInitializer,
            IGetAllCheckedNodes getAllCheckedNodeNames,
            ISelectRulesTreeViewBuilder selectRulesTreeViewBuilder,
            ITreeViewService treeViewService,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            string applicationName)
        {
            InitializeComponent();
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectRulesTreeViewBuilder = selectRulesTreeViewBuilder;
            _treeViewService = treeViewService;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();
            _applicationName = applicationName;
            Initialize();
        }

        public IList<string> SourceFiles => _getAllCheckedNodeNames.GetNames(radTreeView.Nodes[0]);

        public void SetTitle(string title)
        {
            this.Text = title;
            this.radGroupBoxTop.Text = title;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            Disposed += SelectRulesForm_Disposed;
            InitializeDialogFormMessageControl();
            ControlsLayoutUtility.LayoutGroupBox(radPanelTop, radGroupBoxTop);
            _formInitializer.SetFormDefaults(this, 648);
            _selectRulesTreeViewBuilder.Build(radTreeView, _applicationName);

            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelTop);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        #region Event Handlers
        private void RadTreeView_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node)
                || _treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for file nodes on double click*/
                return;

            e.Node.ImageIndex = e.Node.Expanded
                ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }

        private void SelectRulesForm_Disposed(object? sender, System.EventArgs e)
        {
            _treeViewService.ClearImageLists(radTreeView);
        }
        #endregion Event Handlers
    }
}
