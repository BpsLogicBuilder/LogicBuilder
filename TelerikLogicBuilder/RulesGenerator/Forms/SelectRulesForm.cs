using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectRulesForm : Telerik.WinControls.UI.RadForm
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodeNames _getAllCheckedNodeNames;
        private readonly ISelectRulesTreeViewBuilder _selectRulesTreeViewBuilder;

        public SelectRulesForm(IConfigurationService configurationService, IFormInitializer formInitializer, IGetAllCheckedNodeNames getAllCheckedNodeNames, ISelectRulesTreeViewBuilder selectRulesTreeViewBuilder)
        {
            _configurationService = configurationService;
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectRulesTreeViewBuilder = selectRulesTreeViewBuilder;
            InitializeComponent();
            Initialize();
        }

        internal IList<string> SourceFiles => _getAllCheckedNodeNames.GetNames(radTreeView.Nodes[0]);

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 648);
            _selectRulesTreeViewBuilder.Build(radTreeView, _configurationService.GetSelectedApplication().Name);

            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;
            radButtonOk.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonCancel.Anchor = AnchorConstants.AnchorsLeftTopRight;
        }

        private void RadTreeView_NodeExpandedChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            if (e.Node == radTreeView.Nodes[0])
                return;

            e.Node.ImageIndex = e.Node.Expanded
                ? TreeNodeImageIndexes.OPENEDFOLDERIMAGEINDEX
                : TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }
    }
}
