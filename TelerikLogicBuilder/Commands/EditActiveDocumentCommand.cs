using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditActiveDocumentCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public EditActiveDocumentCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.MDIParent.EditControl?.Edit();
        }
    }
}
