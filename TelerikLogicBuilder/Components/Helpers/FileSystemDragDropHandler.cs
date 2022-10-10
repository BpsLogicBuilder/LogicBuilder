using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using System;
using System.Collections.Generic;
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

        public void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes)
        {
            if (draggingTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(draggingTreeNodes))
                throw new ArgumentException($"{nameof(draggingTreeNodes)}: {{C45085F5-9509-4BA2-B39C-FFF633B301FC}}");

            if (_treeViewService.IsFileNode(destinationNode))
                destinationNode = destinationNode.Parent;

            try
            {
                foreach (RadTreeNode draggingTreeNode in draggingTreeNodes)
                {
                    RadTreeNode draggingTreeNodeParent = draggingTreeNode.Parent;
                    if (string.Compare(destinationNode.Name, draggingTreeNodeParent.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                        continue;

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
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                _mainWindow.DocumentsExplorer.RefreshTreeView();
            }
        }

        private void MoveFile(RadTreeNode destinationNode, RadTreeNode draggingTreeNode)
        {
            if (_treeViewService.IsFileNode(destinationNode))
                throw new ArgumentException($"{nameof(destinationNode)}: {{2E9DF5AC-D019-4807-940B-FB2B73648A69}}");

            string newFileFullName = _pathHelper.CombinePaths
            (
                destinationNode.Name,
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
            if (_treeViewService.IsFileNode(destinationNode))
                throw new ArgumentException($"{nameof(destinationNode)}: {{1C6B2F71-0E33-40D2-8542-523A7FC704C9}}");

            string newFolderFullName = _pathHelper.CombinePaths
            (
                destinationNode.Name,
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
            {
                _mainWindow.DocumentsExplorer.ExpandedNodes.Remove(draggingTreeNode.Name);
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(newFolderFullName, draggingTreeNode.Text);
            }
        }
    }
}
