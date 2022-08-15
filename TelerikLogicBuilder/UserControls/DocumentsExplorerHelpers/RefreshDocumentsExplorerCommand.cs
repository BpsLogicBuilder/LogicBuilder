using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class RefreshDocumentsExplorerCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;

        public RefreshDocumentsExplorerCommand(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public override void Execute()
        {
            _mainWindow.DocumentsExplorer.RefreshTreeView();
        }
    }
}
