using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers
{
    internal class RefreshConfigurationExplorerCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public RefreshConfigurationExplorerCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.ConfigurationExplorer.RefreshTreeView();
        }
    }
}
