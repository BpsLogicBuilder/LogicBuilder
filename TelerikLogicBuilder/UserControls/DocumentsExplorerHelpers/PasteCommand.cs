using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class PasteCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMoveFileOperations _moveFileOperations;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly IDocumentsExplorer _documentsExplorer;

        public PasteCommand(
            IExceptionHelper exceptionHelper,
            IMoveFileOperations moveFileOperations,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            IDocumentsExplorer documentsExplorer)
        {
            _exceptionHelper = exceptionHelper;
            _moveFileOperations = moveFileOperations;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            try
            {
                if (_documentsExplorer.TreeView.SelectedNode == null)
                    return;

                if (_documentsExplorer.CutTreeNode == null)
                    return;

                if (_treeViewService.IsFileNode(_documentsExplorer.CutTreeNode))
                {
                    PasteFile(_documentsExplorer.TreeView.SelectedNode, _documentsExplorer.CutTreeNode);
                }
                else if (_treeViewService.IsFolderNode(_documentsExplorer.CutTreeNode))
                {
                    PasteFolder(_documentsExplorer.TreeView.SelectedNode, _documentsExplorer.CutTreeNode);
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{12782526-5DE8-43BF-AFBE-B1A5B277A5B4}");
                }

                _documentsExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                _documentsExplorer.CutTreeNode = null;
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

            if (!_documentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
                _documentsExplorer.ExpandedNodes.Add(selectedNode.Name, selectedNode.Text);
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

            if (!_documentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
                _documentsExplorer.ExpandedNodes.Add(selectedNode.Name, selectedNode.Text);

            if (_documentsExplorer.ExpandedNodes.ContainsKey(cutTreeNode.Name))
                _documentsExplorer.ExpandedNodes.Remove(cutTreeNode.Name);
        }
    }
}
