using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class DeleteSelectionCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public DeleteSelectionCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.MDIParent.EditControl?.Delete();
        }
    }
}
