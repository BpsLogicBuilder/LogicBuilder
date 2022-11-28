using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectRulesForm : Telerik.WinControls.UI.RadForm, ISelectRulesForm
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
            IDialogFormMessageControl dialogFormMessageControl,
            string applicationName)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectRulesTreeViewBuilder = selectRulesTreeViewBuilder;
            _treeViewService = treeViewService;
            _dialogFormMessageControl = dialogFormMessageControl;
            _applicationName = applicationName;
            InitializeComponent();
            Initialize();
        }

        public IList<string> SourceFiles => _getAllCheckedNodeNames.GetNames(radTreeView.Nodes[0]);

        public void SetTitle(string title)
        {
            this.Text = title;
            this.radGroupBoxTop.Text = title;
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            radButtonOk.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonCancel.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 648);
            _selectRulesTreeViewBuilder.Build(radTreeView, _applicationName);

            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeDialogFormMessageControl()
        {
            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainerMain)).BeginInit();
            this.radSplitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelBottom)).BeginInit();
            this.splitPanelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).BeginInit();
            this.radPanelMessages.SuspendLayout();
            this.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new System.Drawing.Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainerMain)).EndInit();
            this.radSplitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelBottom)).EndInit();
            this.splitPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void RadTreeView_NodeExpandedChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node)
                || _treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for file nodes on double click*/
                return;

            e.Node.ImageIndex = e.Node.Expanded
                ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }
    }
}
