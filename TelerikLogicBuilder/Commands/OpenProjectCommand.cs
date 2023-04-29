using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class OpenProjectCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public OpenProjectCommand(
            IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.MDIParent.ChangeCursor(Cursors.WaitCursor);
            using RadOpenFileDialog openFileDialog = new();
            openFileDialog.Filter = $"{ApplicationProperties.Name}|*{FileExtensions.PROJECTFILEEXTENSION}";
            openFileDialog.MultiSelect = false;
            if (openFileDialog.ShowDialog(_mainWindow.Instance) != System.Windows.Forms.DialogResult.OK)
            {
                _mainWindow.MDIParent.ChangeCursor(Cursors.Default);
                return;
            }

            _mainWindow.MDIParent.OpenProject(openFileDialog.FileName);//async call whcih sets cursor to default upon completion.
        }
    }
}
