using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class CloseProjectCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public CloseProjectCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            ((IMDIParent)_mainWindow.Instance).CloseProject();
        }
    }
}
