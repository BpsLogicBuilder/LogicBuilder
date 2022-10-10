using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class CutCommand : ClickCommandBase
    {
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;

        public CutCommand(
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

            foreach (RadTreeNode node in _mainWindow.DocumentsExplorer.CutTreeNodes)
                SetNormalImage(node);

            _mainWindow.DocumentsExplorer.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                _mainWindow.DocumentsExplorer.CutTreeNodes.Add(node);
                SetCutImage(node);
            }
        }

        private void SetCutImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFileNode(cutTreeNode) && cutTreeNode.Text.ToLower(CultureInfo.CurrentCulture).EndsWith(FileExtensions.VSDXFILEEXTENSION))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTVSDXFILEIMAGEINDEX;
            }
            else if (_treeViewService.IsFileNode(cutTreeNode) && cutTreeNode.Text.ToLower(CultureInfo.CurrentCulture).EndsWith(FileExtensions.VISIOFILEEXTENSION))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTVISIOFILEIMAGEINDEX;
            }
            else if (_treeViewService.IsFileNode(cutTreeNode) && cutTreeNode.Text.ToLower(CultureInfo.CurrentCulture).EndsWith(FileExtensions.TABLEFILEEXTENSION))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTTABLEFILEIMAGEINDEX;
            }
            else if (cutTreeNode.Expanded)
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX;
            }
            else if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
        }

        private void SetNormalImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFileNode(cutTreeNode) && cutTreeNode.Text.ToLower(CultureInfo.CurrentCulture).EndsWith(FileExtensions.VSDXFILEEXTENSION))
            {
                cutTreeNode.ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX;
            }
            else if (_treeViewService.IsFileNode(cutTreeNode) && cutTreeNode.Text.ToLower(CultureInfo.CurrentCulture).EndsWith(FileExtensions.VISIOFILEEXTENSION))
            {
                cutTreeNode.ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX;
            }
            else if (_treeViewService.IsFileNode(cutTreeNode) && cutTreeNode.Text.ToLower(CultureInfo.CurrentCulture).EndsWith(FileExtensions.TABLEFILEEXTENSION))
            {
                cutTreeNode.ImageIndex = ImageIndexes.TABLEFILEIMAGEINDEX;
            }
            else if (cutTreeNode.Expanded)
            {
                cutTreeNode.ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX;
            }
            else if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
            }
        }
    }
}
