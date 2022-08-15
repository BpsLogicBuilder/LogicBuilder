using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class CutDocumentCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;

        public CutDocumentCommand(
            IMainWindow mainWindow,
            ITreeViewService treeViewService)
        {
            _mainWindow = mainWindow;
            _treeViewService = treeViewService;
        }

        public override void Execute()
        {
            if (_mainWindow.DocumentsExplorer.TreeView.SelectedNode == null
                || _treeViewService.IsRootNode(_mainWindow.DocumentsExplorer.TreeView.SelectedNode))
                return;

            _mainWindow.DocumentsExplorer.CutTreeNode = _mainWindow.DocumentsExplorer.TreeView.SelectedNode;
        }
    }
}
