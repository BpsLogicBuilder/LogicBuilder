using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
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
