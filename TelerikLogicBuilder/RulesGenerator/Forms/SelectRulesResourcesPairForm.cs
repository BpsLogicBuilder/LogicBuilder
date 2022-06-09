using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectRulesResourcesPairForm : Telerik.WinControls.UI.RadForm
    {
        private readonly string _applicationName;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodeNames;
        private readonly ISelectModulesForDeploymentTreeViewBuilder _selectModulesForDeploymentTreeViewBuilder;

        public SelectRulesResourcesPairForm(IFormInitializer formInitializer, IGetAllCheckedNodes getAllCheckedNodeNames, ISelectModulesForDeploymentTreeViewBuilder selectModulesForDeploymentTreeViewBuilder, string applicationName)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectModulesForDeploymentTreeViewBuilder = selectModulesForDeploymentTreeViewBuilder;
            _applicationName = applicationName;
            InitializeComponent();
            Initialize();
        }

        internal IList<RulesResourcesPair> SourceFiles => _getAllCheckedNodeNames.GetNodes(radTreeView.Nodes[0])
                                                                .Select(n => (RulesResourcesPair)n.Tag)
                                                                .ToArray();

        internal void SetTitle(string title)
        {
            this.Text = title;
            this.radGroupBoxTop.Text = title;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 648);
            _selectModulesForDeploymentTreeViewBuilder.Build(radTreeView, _applicationName);

            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;
            radButtonOk.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonCancel.Anchor = AnchorConstants.AnchorsLeftTopRight;
        }
    }
}
