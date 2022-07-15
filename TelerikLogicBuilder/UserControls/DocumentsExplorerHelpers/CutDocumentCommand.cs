using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class CutDocumentCommand : ClickCommandBase
    {
        private readonly IDocumentsExplorer _documentsExplorer;
        private readonly ITreeViewService _treeViewService;

        public CutDocumentCommand(ITreeViewService treeViewService, IDocumentsExplorer documentsExplorer)
        {
            _documentsExplorer = documentsExplorer;
            _treeViewService = treeViewService;
        }

        public override void Execute()
        {
            if (_documentsExplorer.TreeView.SelectedNode == null
                || _treeViewService.IsRootNode(_documentsExplorer.TreeView.SelectedNode))
                return;

            _documentsExplorer.CutTreeNode = _documentsExplorer.TreeView.SelectedNode;
        }
    }
}
