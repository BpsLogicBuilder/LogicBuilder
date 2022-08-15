using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class PasteCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IMoveFileOperations _moveFileOperations;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public PasteCommand(
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

        public override void Execute()
        {
            try
            {
                if (_mainWindow.DocumentsExplorer.TreeView.SelectedNode == null)
                    return;

                if (_mainWindow.DocumentsExplorer.CutTreeNode == null)
                    return;

                if (_treeViewService.IsFileNode(_mainWindow.DocumentsExplorer.CutTreeNode))
                {
                    PasteFile(_mainWindow.DocumentsExplorer.TreeView.SelectedNode, _mainWindow.DocumentsExplorer.CutTreeNode);
                }
                else if (_treeViewService.IsFolderNode(_mainWindow.DocumentsExplorer.CutTreeNode))
                {
                    PasteFolder(_mainWindow.DocumentsExplorer.TreeView.SelectedNode, _mainWindow.DocumentsExplorer.CutTreeNode);
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{12782526-5DE8-43BF-AFBE-B1A5B277A5B4}");
                }

                _mainWindow.DocumentsExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                _mainWindow.DocumentsExplorer.CutTreeNode = null;
            }
        }

        private void PasteFile(RadTreeNode selectedNode, RadTreeNode cutTreeNode)
        {
            string newFileFullName = _pathHelper.CombinePaths
            (
                _treeViewService.IsFileNode(selectedNode)
                    ? selectedNode.Parent.Name
                    : selectedNode.Name,
                _pathHelper.GetFileName(cutTreeNode.Name)
            );

            _moveFileOperations.MoveFile
            (
                cutTreeNode,
                newFileFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(selectedNode.Name, selectedNode.Text);
        }

        private void PasteFolder(RadTreeNode selectedNode, RadTreeNode cutTreeNode)
        {
            string newFolderFullName = _pathHelper.CombinePaths
            (
                _treeViewService.IsFileNode(selectedNode)
                    ? selectedNode.Parent.Name
                    : selectedNode.Name,
                _pathHelper.GetFolderName(cutTreeNode.Name)
            );

            _moveFileOperations.MoveFolder
            (
                cutTreeNode,
                newFolderFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(selectedNode.Name, selectedNode.Text);

            if (_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(cutTreeNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Remove(cutTreeNode.Name);
        }
    }
}
