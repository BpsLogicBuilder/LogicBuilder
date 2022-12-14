using System.Collections.Generic;
using System.Drawing;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewService
    {
        RadTreeNode AddChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null, string? nodeIndex = null);
        TNode AddChildTreeNode<TNode>(TNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null, string? nodeIndex = null) where TNode : RadTreeNode;
        bool CollectionIncludesNodeAndDescendant(IList<RadTreeNode> treeNodes);
        RadTreeNode CreateChildFolderTreeNode(RadTreeNode parentTreeNode, string folderXmlElementName, int imageIndex, string xmlNameAttributeValue, string? toolTipText = null);
        TNode CreateChildFolderTreeNode<TNode>(TNode parentTreeNode, string folderXmlElementName, int imageIndex, string xmlNameAttributeValue, string? toolTipText = null) where TNode : RadTreeNode;
        RadTreeNode CreateChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null);
        TNode CreateChildTreeNode<TNode>(TNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null) where TNode : RadTreeNode;
        RadTreeNode GetClosestNodeForSelectionAfterDelete(RadTreeNode treeNode);
        int GetInsertPosition(RadTreeNode[] treeNodeArray, RadTreeNode newNode, IComparer<RadTreeNode> treeNodeComparer);
        IList<RadTreeNode> GetSelectedNodes(RadTreeView treeView);
        bool IsApplicationNode(RadTreeNode treeNode);
        bool IsConstructorNode(RadTreeNode treeNode);
        bool IsFileNode(RadTreeNode treeNode);
        bool IsFolderNode(RadTreeNode treeNode);
        bool IsGenericArgumentParameterNode(RadTreeNode treeNode);
        bool IsGenericTypeNode(RadTreeNode treeNode);
        bool IsListOfGenericsTypeNode(RadTreeNode treeNode);
        bool IsListOfLiteralsTypeNode(RadTreeNode treeNode);
        bool IsListOfObjectsTypeNode(RadTreeNode treeNode);
        bool IsLiteralTypeNode(RadTreeNode treeNode);
        bool IsMethodNode(RadTreeNode treeNode);
        bool IsObjectTypeNode(RadTreeNode treeNode);
        bool IsParameterNode(RadTreeNode treeNode);
        bool IsProjectRootNode(RadTreeNode treeNode);
        bool IsRootNode(RadTreeNode treeNode);
        bool IsVariableNode(RadTreeNode treeNode);
        bool IsVariableTypeNode(RadTreeNode treeNode);
        void MakeVisible(RadTreeNode treeNode);
        void RenameChildFolderTreeNode(RadTreeNode parentTreeNode, RadTreeNode childNode, string xmlNameAttributeValue);
        void RenameChildTreeNode(RadTreeNode parentTreeNode, RadTreeNode childNode, string relativeXPath, string idAttributeName, string idAttributeValue);
        void ScrollToPreviousPosition(RadTreeView treeView, Point point);
        void SelectTreeNode(RadTreeView treeView, string? nodeName);
        void SelectTreeNodes(RadTreeView treeView, IList<string> nodeNames);
    }
}
