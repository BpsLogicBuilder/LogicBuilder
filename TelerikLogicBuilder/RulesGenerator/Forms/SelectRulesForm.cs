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
        private readonly string _applicationName;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodeNames;
        private readonly ISelectRulesTreeViewBuilder _selectRulesTreeViewBuilder;

        public SelectRulesForm(IFormInitializer formInitializer, IGetAllCheckedNodes getAllCheckedNodeNames, ISelectRulesTreeViewBuilder selectRulesTreeViewBuilder, string applicationName)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectRulesTreeViewBuilder = selectRulesTreeViewBuilder;
            _applicationName = applicationName;
            InitializeComponent();
            Initialize();
        }

        internal IList<string> SourceFiles => _getAllCheckedNodeNames.GetNames(radTreeView.Nodes[0]);

        internal void SetTitle(string title)
        {
            this.Text = title;
            this.radGroupBoxTop.Text = title;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 648);
            _selectRulesTreeViewBuilder.Build(radTreeView, _applicationName);

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
