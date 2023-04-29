using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class SaveActiveDocumentCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public SaveActiveDocumentCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.MDIParent.EditControl?.Save();
        }
    }
}
