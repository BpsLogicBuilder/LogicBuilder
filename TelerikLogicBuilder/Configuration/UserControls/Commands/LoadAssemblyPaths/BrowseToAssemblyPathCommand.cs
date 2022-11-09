using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths
{
    internal class BrowseToAssemblyPathCommand : ClickCommandBase
    {
        private readonly HelperButtonTextBox txtPath;

        public BrowseToAssemblyPathCommand(ILoadAssemblyPathsControl loadAssemblyPathsControl)
        {
            txtPath = loadAssemblyPathsControl.TxtPath;
        }

        public override void Execute()
        {
            using RadOpenFolderDialog openFolderDialog = new();
            openFolderDialog.MultiSelect = false;
            openFolderDialog.InitialDirectory = Directory.Exists(txtPath.Text) ? txtPath.Text : string.Empty;

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openFolderDialog.FileName;
            }
        }
    }
}
