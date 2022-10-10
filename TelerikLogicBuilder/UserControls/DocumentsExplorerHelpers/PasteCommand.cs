using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
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
                IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
                if (selectedNodes.Count != 1)
                    return;

                if (_mainWindow.DocumentsExplorer.CutTreeNodes.Count == 0)
                    return;

                if (_treeViewService.CollectionIncludesNodeAndDescendant(_mainWindow.DocumentsExplorer.CutTreeNodes))
                    throw new ArgumentException($"{nameof(_mainWindow.DocumentsExplorer.CutTreeNodes)}: {{A80740CD-0542-4E70-8897-72CBE8A88B55}}");

                RadTreeNode destinationNode = _treeViewService.IsFileNode(selectedNodes[0])
                                                ? selectedNodes[0].Parent
                                                : selectedNodes[0];

                foreach (RadTreeNode cutTreeNode in _mainWindow.DocumentsExplorer.CutTreeNodes)
                {
                    RadTreeNode cutTreeNodeParent = cutTreeNode.Parent;
                    if (string.Compare(destinationNode.Name, cutTreeNodeParent.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
                        continue;

                    if (_treeViewService.IsFileNode(cutTreeNode))
                    {
                        PasteFile(destinationNode, cutTreeNode);
                    }
                    else if (_treeViewService.IsFolderNode(cutTreeNode))
                    {
                        PasteFolder(destinationNode, cutTreeNode);
                    }
                    else
                    {
                        throw _exceptionHelper.CriticalException("{12782526-5DE8-43BF-AFBE-B1A5B277A5B4}");
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
                _mainWindow.DocumentsExplorer.CutTreeNodes.Clear();
            }
        }

        private void PasteFile(RadTreeNode destinationNode, RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFileNode(destinationNode))
                throw new ArgumentException($"{nameof(destinationNode)}: {{DCA13685-888E-4336-915E-FA917A12E607}}");

            string newFileFullName = _pathHelper.CombinePaths
            (
                destinationNode.Name,
                _pathHelper.GetFileName(cutTreeNode.Name)
            );

            _moveFileOperations.MoveFile
            (
                cutTreeNode,
                newFileFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(destinationNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(destinationNode.Name, destinationNode.Text);
        }

        private void PasteFolder(RadTreeNode destinationNode, RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFileNode(destinationNode))
                throw new ArgumentException($"{nameof(destinationNode)}: {{88A79908-9EDB-486B-BCCF-C01C83289B4B}}");

            string newFolderFullName = _pathHelper.CombinePaths
            (
                destinationNode.Name,
                _pathHelper.GetFolderName(cutTreeNode.Name)
            );

            _moveFileOperations.MoveFolder
            (
                cutTreeNode,
                newFolderFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(destinationNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(destinationNode.Name, destinationNode.Text);

            if (_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(cutTreeNode.Name))
            {
                _mainWindow.DocumentsExplorer.ExpandedNodes.Remove(cutTreeNode.Name);
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(newFolderFullName, cutTreeNode.Text);
            }
        }
    }
}
