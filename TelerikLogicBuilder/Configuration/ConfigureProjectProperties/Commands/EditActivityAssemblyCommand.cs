using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands
{
    internal class EditActivityAssemblyCommand : ClickCommandBase
    {
        private readonly IPathHelper _pathHelper;
        private readonly HelperButtonTextBox txtActivityAssembly;
        private readonly HelperButtonTextBox txtActivityAssemblyPath;

        private readonly IApplicationControl applicationControl;

        public EditActivityAssemblyCommand(
            IPathHelper pathHelper,
            IApplicationControl applicationControl)
        {
            _pathHelper = pathHelper;
            txtActivityAssembly = applicationControl.TxtActivityAssembly;
            txtActivityAssemblyPath = applicationControl.TxtActivityAssemblyPath;
            this.applicationControl = applicationControl;
        }

        public override void Execute()
        {
            ((Control)applicationControl).Cursor = Cursors.WaitCursor;
            using RadOpenFileDialog openFileDialog = new();
            openFileDialog.MultiSelect = false;
            if (Directory.Exists(txtActivityAssemblyPath.Text))
                openFileDialog.InitialDirectory = txtActivityAssemblyPath.Text;

            if (openFileDialog.ShowDialog((IWin32Window)applicationControl) == DialogResult.OK)
            {
                txtActivityAssembly.Text = _pathHelper.GetFileName(openFileDialog.FileName);
            }
            ((Control)applicationControl).Cursor = Cursors.Default;
        }
    }
}
