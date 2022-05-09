using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class EmptyFolderRemover : IEmptyFolderRemover
    {
        private readonly ITreeViewService _treeViewService;

        public EmptyFolderRemover(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public void RemoveEmptyFolders(RadTreeNode treeNode)
        {
            List<RadTreeNode> foldersToRemove = new();

            foreach (RadTreeNode childNode in treeNode.Nodes)
            {
                if (_treeViewService.IsFolderNode(childNode))
                {
                    RemoveEmptyFolders(childNode);
                    if (!HasFileDescendants(childNode))
                        foldersToRemove.Add(childNode);
                }
            }

            foreach (RadTreeNode node in foldersToRemove)
            {
                treeNode.Nodes.Remove(node);
            }
        }

        private bool HasFileDescendants(RadTreeNode treeNode)
        {
            foreach (RadTreeNode childNode in treeNode.Nodes)
            {
                if (!_treeViewService.IsFolderNode(childNode))
                    return true;

                if (_treeViewService.IsFolderNode(childNode) && HasFileDescendants(childNode))
                    return true;
            }

            return false;
        }
    }
}
