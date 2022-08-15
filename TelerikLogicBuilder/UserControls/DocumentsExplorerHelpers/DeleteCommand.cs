using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class DeleteCommand : ClickCommandBase
    {
        private readonly IDeleteOperations _deleteOperations;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public DeleteCommand(
            IDeleteOperations deleteOperations,
            IMainWindow mainWindow,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _deleteOperations = deleteOperations;
            _mainWindow = mainWindow;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode? selectedNode = _mainWindow.DocumentsExplorer.TreeView.SelectedNode;
                if (selectedNode == null)
                    return;

                if (_treeViewService.IsFileNode(selectedNode))
                {
                    _deleteOperations.DeleteFile(selectedNode);
                    _mainWindow.DocumentsExplorer.RefreshTreeView();
                }
                else if (_treeViewService.IsRootNode(selectedNode))
                {
                    _deleteOperations.DeleteProject(selectedNode);
                }
                else if (_treeViewService.IsFolderNode(selectedNode))
                {
                    _deleteOperations.DeleteFolder(selectedNode);

                    if (_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
                        _mainWindow.DocumentsExplorer.ExpandedNodes.Remove(selectedNode.Name);

                    _mainWindow.DocumentsExplorer.RefreshTreeView();
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}
