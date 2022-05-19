using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectDocumentsForm : Telerik.WinControls.UI.RadForm
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodeNames _getAllCheckedNodeNames;
        private readonly ISelectDocunentsTreeViewBuilder _selectDocunentsTreeViewBuilder;

        public SelectDocumentsForm(IFormInitializer formInitializer, IGetAllCheckedNodeNames getAllCheckedNodeNames, ISelectDocunentsTreeViewBuilder selectDocunentsTreeViewBuilder)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectDocunentsTreeViewBuilder = selectDocunentsTreeViewBuilder;
            InitializeComponent();
            Initialize();
        }

        internal IList<string> SourceFiles => _getAllCheckedNodeNames.GetNames(radTreeView.Nodes[0]);

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 648);
            _selectDocunentsTreeViewBuilder.Build(radTreeView);

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
