using System.Collections.Generic;
using System.Drawing;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewService
    {
        bool CollectionIncludesNodeAndDescendant(IList<RadTreeNode> treeNodes);
        IList<RadTreeNode> GetSelectedNodes(RadTreeView treeView);
        bool IsRootNode(RadTreeNode treeNode);
        bool IsFileNode(RadTreeNode treeNode);
        bool IsFolderNode(RadTreeNode treeNode);
        bool IsMethodNode(RadTreeNode treeNode);
        void MakeVisible(RadTreeNode treeNode);
        void ScrollToPreviousPosition(RadTreeView treeView, Point point);
        void SelectTreeNode(RadTreeView treeView, string? nodeName);
        void SelectTreeNodes(RadTreeView treeView, IList<string> nodeNames);
    }
}
