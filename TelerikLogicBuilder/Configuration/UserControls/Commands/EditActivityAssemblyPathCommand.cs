using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands
{
    internal class EditActivityAssemblyPathCommand : ClickCommandBase
    {
        private readonly HelperButtonTextBox txtActivityAssemblyPath;

        public EditActivityAssemblyPathCommand(IApplicationControl applicationControl)
        {
            this.txtActivityAssemblyPath = applicationControl.TxtActivityAssemblyPath;
        }

        public override void Execute()
        {
            using RadOpenFolderDialog openFolderDialog = new();
            openFolderDialog.MultiSelect = false;
            openFolderDialog.InitialDirectory = Directory.Exists(txtActivityAssemblyPath.Text) ? txtActivityAssemblyPath.Text : string.Empty;

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtActivityAssemblyPath.Text = openFolderDialog.FileName;
            }
        }
    }
}
