using System.Collections.Generic;
using System.Drawing;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewService
    {
        RadTreeNode AddChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null, string? nodeIndex = null);
        bool CollectionIncludesNodeAndDescendant(IList<RadTreeNode> treeNodes);
        RadTreeNode GetChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null);
        int GetInsertPosition(RadTreeNode[] treeNodeArray, RadTreeNode newNode, IComparer<RadTreeNode> treeNodeComparer);
        IList<RadTreeNode> GetSelectedNodes(RadTreeView treeView);
        bool IsApplicationNode(RadTreeNode treeNode);
        bool IsFileNode(RadTreeNode treeNode);
        bool IsFolderNode(RadTreeNode treeNode);
        bool IsMethodNode(RadTreeNode treeNode);
        bool IsProjectRootNode(RadTreeNode treeNode);
        bool IsRootNode(RadTreeNode treeNode);
        void MakeVisible(RadTreeNode treeNode);
        void ScrollToPreviousPosition(RadTreeView treeView, Point point);
        void SelectClosestNodeOnDelete(RadTreeNode treeNode);
        void SelectTreeNode(RadTreeView treeView, string? nodeName);
        void SelectTreeNodes(RadTreeView treeView, IList<string> nodeNames);
    }
}
