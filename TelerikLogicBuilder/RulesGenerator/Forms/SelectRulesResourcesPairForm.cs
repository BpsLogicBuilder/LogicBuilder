using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            IDialogFormMessageControl dialogFormMessageControl,
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
            ((ISupportInitialize)(this.radPanelMessages)).BeginInit();
            this.radPanelMessages.SuspendLayout();

            _dialogFormMessageControl.Dock = DockStyle.Fill;
            _dialogFormMessageControl.Location = new Point(0, 0);
            this.radPanelMessages.Controls.Add((Control)_dialogFormMessageControl);

            ((ISupportInitialize)(this.radPanelMessages)).EndInit();
            this.radPanelMessages.ResumeLayout(true);
        }
    }
}
