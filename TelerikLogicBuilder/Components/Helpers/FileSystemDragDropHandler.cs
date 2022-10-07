using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Helpers
{
    internal class FileSystemDragDropHandler : IFileSystemDragDropHandler
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IMoveFileOperations _moveFileOperations;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public FileSystemDragDropHandler(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IMoveFileOperations moveFileOperations,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _moveFileOperations = moveFileOperations;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public void DragDrop(RadTreeNode destinationNode, RadTreeNode draggingTreeNode)
        {
            try
            {
                if (_treeViewService.IsFileNode(draggingTreeNode))
                {
                    MoveFile(destinationNode, draggingTreeNode);
                }
                else if (_treeViewService.IsFolderNode(draggingTreeNode))
                {
                    MoveFolder(destinationNode, draggingTreeNode);
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{AB0ED335-A566-416D-8ED0-D233E25D2644}");
                }

                _mainWindow.DocumentsExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void MoveFile(RadTreeNode destinationNode, RadTreeNode draggingTreeNode)
        {
            string newFileFullName = _pathHelper.CombinePaths
            (
                _treeViewService.IsFileNode(destinationNode)
                    ? destinationNode.Parent.Name
                    : destinationNode.Name,
                _pathHelper.GetFileName(draggingTreeNode.Name)
            );

            _moveFileOperations.MoveFile
            (
                draggingTreeNode,
                newFileFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(destinationNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(destinationNode.Name, destinationNode.Text);
        }

        private void MoveFolder(RadTreeNode destinationNode, RadTreeNode draggingTreeNode)
        {
            string newFolderFullName = _pathHelper.CombinePaths
            (
                _treeViewService.IsFileNode(destinationNode)
                    ? destinationNode.Parent.Name
                    : destinationNode.Name,
                _pathHelper.GetFolderName(draggingTreeNode.Name)
            );

            _moveFileOperations.MoveFolder
            (
                draggingTreeNode,
                newFolderFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(destinationNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(destinationNode.Name, destinationNode.Text);

            if (_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(draggingTreeNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Remove(draggingTreeNode.Name);
        }
    }
}
