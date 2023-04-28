using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class OpenProjectCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;
        private readonly IUiNotificationService _uiNotificationService;

        public OpenProjectCommand(
            IMainWindow mainWindow,
            IUiNotificationService uiNotificationService)
        {
            _mainWindow = mainWindow;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
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
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}
