using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class DeleteCommand : ClickCommandBase
    {
        private readonly IDeleteOperations _deleteOperations;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        public DeleteCommand(
            IDeleteOperations deleteOperations,
            IMainWindow mainWindow,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
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
                IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
                if (selectedNodes.Count == 0)
                    return;

                if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                    throw new ArgumentException($"{nameof(selectedNodes)}: {{DCACA44D-651B-4609-A3D2-842D5D1D2FA2}}");

                foreach (RadTreeNode selectedNode in selectedNodes)
                {
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
    }
}
