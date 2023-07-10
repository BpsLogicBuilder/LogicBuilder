using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectRulesResourcesPairForm : RadForm, ISelectRulesResourcesPairForm
    {
        private readonly string _applicationName;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodeNames;
        private readonly ISelectModulesForDeploymentTreeViewBuilder _selectModulesForDeploymentTreeViewBuilder;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public SelectRulesResourcesPairForm(
            IFormInitializer formInitializer,
            IGetAllCheckedNodes getAllCheckedNodeNames,
            ISelectModulesForDeploymentTreeViewBuilder selectModulesForDeploymentTreeViewBuilder,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            string applicationName)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectModulesForDeploymentTreeViewBuilder = selectModulesForDeploymentTreeViewBuilder;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();
            _applicationName = applicationName;
            InitializeComponent();
            Initialize();
        }

        public IList<RulesResourcesPair> SourceFiles => _getAllCheckedNodeNames.GetNodes(radTreeView.Nodes[0])
                                                                .Select(n => (RulesResourcesPair)n.Tag)
        .ToArray();

        public void SetTitle(string title)
        {
            this.Text = title;
            this.radGroupBoxTop.Text = title;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            ControlsLayoutUtility.LayoutGroupBox(radPanelTop, radGroupBoxTop);
            _formInitializer.SetFormDefaults(this, 648);
            _selectModulesForDeploymentTreeViewBuilder.Build(radTreeView, _applicationName);

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
    }
}
