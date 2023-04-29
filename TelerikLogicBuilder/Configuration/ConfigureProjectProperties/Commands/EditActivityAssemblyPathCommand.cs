using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands
{
    internal class EditActivityAssemblyPathCommand : ClickCommandBase
    {
        private readonly IApplicationControl applicationControl;
        private readonly HelperButtonTextBox txtActivityAssemblyPath;

        public EditActivityAssemblyPathCommand(IApplicationControl applicationControl)
        {
            txtActivityAssemblyPath = applicationControl.TxtActivityAssemblyPath;
            this.applicationControl = applicationControl;
        }

        public override void Execute()
        {
            ((Control)applicationControl).Cursor = Cursors.WaitCursor;
            using RadOpenFolderDialog openFolderDialog = new();
            openFolderDialog.MultiSelect = false;
            openFolderDialog.InitialDirectory = Directory.Exists(txtActivityAssemblyPath.Text) ? txtActivityAssemblyPath.Text : string.Empty;

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtActivityAssemblyPath.Text = openFolderDialog.FileName;
            }
            ((Control)applicationControl).Cursor = Cursors.Default;
        }
    }
}
