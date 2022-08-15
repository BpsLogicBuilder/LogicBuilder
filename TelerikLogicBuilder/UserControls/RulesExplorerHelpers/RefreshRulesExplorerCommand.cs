using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers
{
    internal class RefreshRulesExplorerCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public RefreshRulesExplorerCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.RulesExplorer.RefreshTreeView();
        }
    }
}
