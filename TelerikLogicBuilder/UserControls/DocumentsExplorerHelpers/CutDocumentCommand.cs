using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

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
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
            if (selectedNodes.Count == 0
                || _mainWindow.DocumentsExplorer.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{CD67FCB0-32ED-4571-9CCE-24F81AA36BAD}}");

            _mainWindow.DocumentsExplorer.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
                _mainWindow.DocumentsExplorer.CutTreeNodes.Add(node);
        }
    }
}
