using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Forms.Commands
{
    internal class SelectFolderCommand : ClickCommandBase
    {
        private readonly INewProjectForm newProjectForm;

        public SelectFolderCommand(INewProjectForm newProjectForm)
        {
            this.newProjectForm = newProjectForm;
        }

        private HelperButtonTextBox TxtProjectPath => newProjectForm.TxtProjectPath;

        public override void Execute()
        {
            ((Control)newProjectForm).Cursor = Cursors.WaitCursor;
            using RadOpenFolderDialog openFolderDialog = new();
            openFolderDialog.MultiSelect = false;
            openFolderDialog.InitialDirectory = Directory.Exists(TxtProjectPath.Text) ? TxtProjectPath.Text : string.Empty;

            if (openFolderDialog.ShowDialog() == DialogResult.OK)
            {
                TxtProjectPath.Text = openFolderDialog.FileName;
            }
            ((Control)newProjectForm).Cursor = Cursors.Default;
        }
    }
}
