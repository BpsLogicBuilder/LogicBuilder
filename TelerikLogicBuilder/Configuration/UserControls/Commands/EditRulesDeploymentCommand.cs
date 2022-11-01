using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands
{
    internal class EditRulesDeploymentCommand : ClickCommandBase
    {
        private readonly HelperButtonTextBox txtRulesDeployment;

        public EditRulesDeploymentCommand(IApplicationControl applicationControl)
        {
            this.txtRulesDeployment = applicationControl.TxtRulesDeployment;
        }

        public override void Execute()
        {
            using RadOpenFolderDialog openFolderDialog = new();
            openFolderDialog.MultiSelect = false;
            openFolderDialog.InitialDirectory = Directory.Exists(txtRulesDeployment.Text) ? txtRulesDeployment.Text : string.Empty;

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtRulesDeployment.Text = openFolderDialog.FileName;
            }
        }
    }
}
