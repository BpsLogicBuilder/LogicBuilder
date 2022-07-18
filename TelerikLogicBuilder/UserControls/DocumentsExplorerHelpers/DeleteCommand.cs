using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class DeleteCommand : ClickCommandBase
    {
        private readonly IDeleteOperations _deleteOperations;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly IDocumentsExplorer _documentsExplorer;

        public DeleteCommand(
            IDeleteOperations deleteOperations,
            ITreeViewService treeViewService,
            UiNotificationService notificationService,
            IDocumentsExplorer documentsExplorer)
        {
            _treeViewService = treeViewService;
            _deleteOperations = deleteOperations;
            _uiNotificationService = notificationService;
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode? selectedNode = _documentsExplorer.TreeView.SelectedNode;
                if (selectedNode == null)
                    return;

                if (_treeViewService.IsFileNode(selectedNode))
                {
                    _deleteOperations.DeleteFile(selectedNode);
                    _documentsExplorer.RefreshTreeView();
                }
                else if (_treeViewService.IsRootNode(selectedNode))
                {
                    _deleteOperations.DeleteProject(selectedNode);
                }
                else if (_treeViewService.IsFolderNode(selectedNode))
                {
                    _deleteOperations.DeleteFolder(selectedNode);

                    if (_documentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
                        _documentsExplorer.ExpandedNodes.Remove(selectedNode.Name);

                    _documentsExplorer.RefreshTreeView();
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}
