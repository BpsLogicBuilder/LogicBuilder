using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
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
        private readonly DialogFormMessageControl _dialogFormMessageControl;

        public SelectRulesResourcesPairForm(
            IFormInitializer formInitializer,
            IGetAllCheckedNodes getAllCheckedNodeNames,
            ISelectModulesForDeploymentTreeViewBuilder selectModulesForDeploymentTreeViewBuilder,
            DialogFormMessageControl dialogFormMessageControl,
            string applicationName)
        {
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectModulesForDeploymentTreeViewBuilder = selectModulesForDeploymentTreeViewBuilder;
            _dialogFormMessageControl = dialogFormMessageControl;
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
            InitializeDialogFormMessageControl();
            radButtonOk.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonCancel.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 648);
            _selectModulesForDeploymentTreeViewBuilder.Build(radTreeView, _applicationName);

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
            this.radPanelMessages.Controls.Add(_dialogFormMessageControl);

            ((System.ComponentModel.ISupportInitialize)(this.radSplitContainerMain)).EndInit();
            this.radSplitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelBottom)).EndInit();
            this.splitPanelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
