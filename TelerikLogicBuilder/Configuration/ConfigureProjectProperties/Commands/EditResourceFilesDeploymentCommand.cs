using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands
{
    internal class EditResourceFilesDeploymentCommand : ClickCommandBase
    {
        private readonly HelperButtonTextBox txtResourceFilesDeployment;

        public EditResourceFilesDeploymentCommand(IApplicationControl applicationControl)
        {
            txtResourceFilesDeployment = applicationControl.TxtResourceFilesDeployment;
        }

        public override void Execute()
        {
            using RadOpenFolderDialog openFolderDialog = new();
            openFolderDialog.MultiSelect = false;
            openFolderDialog.InitialDirectory = Directory.Exists(txtResourceFilesDeployment.Text) ? txtResourceFilesDeployment.Text : string.Empty;

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtResourceFilesDeployment.Text = openFolderDialog.FileName;
            }
        }
    }
}
