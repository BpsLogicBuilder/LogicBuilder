using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewProjectExplorerCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public ViewProjectExplorerCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.ProjectExplorer.Visible = true;
        }
    }
}
