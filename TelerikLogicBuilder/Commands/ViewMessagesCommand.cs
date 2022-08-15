using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewMessagesCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public ViewMessagesCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.Messages.Visible = true;
        }
    }
}
