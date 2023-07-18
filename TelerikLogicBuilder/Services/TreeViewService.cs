using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TreeViewService : ITreeViewService
    {
        private readonly IExceptionHelper _exceptionHelper;

        public TreeViewService(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public RadTreeNode AddChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null, string? nodeIndex = null) 
            => AddChildTreeNode<RadTreeNode>
            (
                parentTreeNode,
                relativeXPath,
                idAttributeName,
                idAttributeValue,
                imageIndex,
                toolTipText,
                nodeIndex
            );

        public TNode AddChildTreeNode<TNode>(TNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null, string? nodeIndex = null) where TNode : RadTreeNode
        {
            TNode treeNode = (TNode?)Activator.CreateInstance(typeof(TNode)) ?? throw _exceptionHelper.CriticalException("{151235F6-4B13-4C57-AE1E-4BE798B8BC11}");
            treeNode.Text = idAttributeValue;
            treeNode.ImageIndex = imageIndex;
            treeNode.Name = $"{parentTreeNode.Name}/{relativeXPath}[@{idAttributeName}=\"{idAttributeValue}\"]";
            treeNode.ToolTipText = toolTipText ?? string.Empty;

            if (nodeIndex != null)
            {
                treeNode.Name = $"{treeNode.Name}[{nodeIndex}]";
                treeNode.Text = $"{treeNode.Text}[{nodeIndex}]";
            }

            parentTreeNode.Nodes.Add(treeNode);

            return treeNode;
        }

        public void ClearImageLists(RadTreeView radTreeView)
        {
            radTreeView.ImageList = null;
            if (radTreeView.RadContextMenu != null)
            {
                radTreeView.RadContextMenu.ImageList = null;
            }
        }

        public bool CollectionIncludesNodeAndDescendant(IList<RadTreeNode> treeNodes)
        {
            HashSet<RadTreeNode> hashSet = treeNodes.ToHashSet();
            foreach(RadTreeNode node in treeNodes)
            {
                if (AncestorPresent(node, hashSet))
                    return true;
            }

            return false;
        }

        public RadTreeNode CreateChildFolderTreeNode(RadTreeNode parentTreeNode, string folderXmlElementName, int imageIndex, string xmlNameAttributeValue, string? toolTipText = null) 
            => CreateChildFolderTreeNode<RadTreeNode>
            (
                parentTreeNode,
                folderXmlElementName,
                imageIndex,
                xmlNameAttributeValue,
                toolTipText
            );

        public TNode CreateChildFolderTreeNode<TNode>(TNode parentTreeNode, string folderXmlElementName, int imageIndex, string xmlNameAttributeValue, string? toolTipText = null) where TNode : RadTreeNode
        {
            TNode treeNode = (TNode?)Activator.CreateInstance(typeof(TNode)) ?? throw _exceptionHelper.CriticalException("{A144758D-EE5D-410B-977E-63A554BE7650}");
            treeNode.Text = xmlNameAttributeValue;
            treeNode.ImageIndex = imageIndex;
            treeNode.Name = $"{parentTreeNode.Name}/{folderXmlElementName}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{xmlNameAttributeValue}\"]";
            treeNode.ToolTipText = toolTipText ?? string.Empty;
            return treeNode;
        }

        public RadTreeNode CreateChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null) 
            => CreateChildTreeNode<RadTreeNode>
            (
                parentTreeNode,
                relativeXPath,
                idAttributeName,
                idAttributeValue,
                imageIndex,
                toolTipText
            );

        public TNode CreateChildTreeNode<TNode>(TNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null) where TNode : RadTreeNode
        {
            TNode treeNode = (TNode?)Activator.CreateInstance(typeof(TNode)) ?? throw _exceptionHelper.CriticalException("{6384085A-F8DC-48FF-BB4D-83914C80CFEE}");
            treeNode.Text = idAttributeValue;
            treeNode.ImageIndex = imageIndex;
            treeNode.Name = $"{parentTreeNode.Name}/{relativeXPath}[@{idAttributeName}=\"{idAttributeValue}\"]";
            treeNode.ToolTipText = toolTipText ?? string.Empty;

            return treeNode;
        }

        public RadTreeNode GetClosestNodeForSelectionAfterDelete(RadTreeNode treeNode)
        {
            if (treeNode.NextNode != null)
                return treeNode.NextNode;
            else if (treeNode.PrevNode != null)
                return treeNode.PrevNode;
            else if (treeNode.Parent != null)
                return treeNode.Parent;
            else
                throw _exceptionHelper.CriticalException("{87DF7DDA-A430-4A00-897F-FB590477A8D6}");
        }

        public int GetInsertPosition(RadTreeNode[] treeNodeArray, RadTreeNode newNode, IComparer<RadTreeNode> treeNodeComparer)
        {
            Array.Sort(treeNodeArray, treeNodeComparer);
            int position = Array.BinarySearch(treeNodeArray, newNode, treeNodeComparer);
            position = position < 0 ? ~position : position;
            return position;
        }

        public IList<RadTreeNode> GetSelectedNodes(RadTreeView treeView)
        {
            List<RadTreeNode> returnList = new();

            HashSet<RadTreeNode> hashSet = GetAllNodes().ToHashSet();
            foreach (RadTreeNode radTreeNode in hashSet)
            {
                if (!AncestorPresent(radTreeNode, hashSet))
                    returnList.Add(radTreeNode);
            }

            return returnList;

            List<RadTreeNode> GetAllNodes()
            {
                if (treeView.SelectedNodes.Count > 0)
                    return new List<RadTreeNode>(treeView.SelectedNodes);
                else if (treeView.SelectedNode != null)
                    return new List<RadTreeNode> { treeView.SelectedNode };
                return new List<RadTreeNode>();
            }
        }

        public RadTreeNode GetTreeNodeByName(RadTreeView treeView, string nodeName) 
            => treeView.Find(n => n.Name == nodeName) ?? throw _exceptionHelper.CriticalException("{6465F43E-B56B-4797-A278-5520A2126661}");

        public bool IsApplicationNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{92F0C74F-8DD6-41DA-A3F6-997D5386D3F0}");

            return treeNode.ImageIndex == ImageIndexes.APPLICATIONIMAGEINDEX;
        }

        public bool IsConstructorNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{B79A04F8-70AC-4734-8B56-48BACBA55253}");

            return treeNode.ImageIndex == ImageIndexes.CONSTRUCTORIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTCONSTRUCTORIMAGEINDEX;
        }

        public bool IsFileNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{AAC9BCBF-3675-4B9F-8C87-39492B9B7D98}");

            return treeNode.ImageIndex == ImageIndexes.VSDXFILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.VISIOFILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.TABLEFILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.FILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTVSDXFILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTVISIOFILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTTABLEFILEIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTFILEIMAGEINDEX;
        }

        public bool IsFolderNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{5AA2AB5A-1BFF-444B-83A9-DDF0700513D5}");

            return treeNode.ImageIndex == ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OPENEDFOLDERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX;
        }

        public bool IsGenericArgumentParameterNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{5249D22F-3191-4AF1-A4DC-C201AD65786C}");

            return treeNode.ImageIndex == ImageIndexes.LITERALPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OBJECTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX;
        }

        public bool IsGenericTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{10CE0D92-21B2-4BDB-BE06-79A417363924}");

            return treeNode.ImageIndex == ImageIndexes.GENERICPARAMETERIMAGEINDEX 
                || treeNode.ImageIndex == ImageIndexes.CUTGENERICPARAMETERIMAGEINDEX;
        }

        public bool IsListOfGenericsTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{B9AEC1D2-0986-4031-B92B-8124515DEEF7}");

            return treeNode.ImageIndex == ImageIndexes.GENERICLISTPARAMETERIMAGEINDEX 
                || treeNode.ImageIndex == ImageIndexes.CUTGENERICLISTPARAMETERIMAGEINDEX;
        }

        public bool IsListOfLiteralsTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{35BD93C1-776A-46F0-9652-4F390293780D}");

            return treeNode.ImageIndex == ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTLITERALLISTPARAMETERIMAGEINDEX;
        }

        public bool IsListOfObjectsTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{469B4116-CBDA-416B-8555-8C8CEAA22612}");

            return treeNode.ImageIndex == ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTOBJECTLISTPARAMETERIMAGEINDEX;
        }

        public bool IsLiteralTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{59EA9B27-F660-4C66-8472-86A55434EED8}");

            return treeNode.ImageIndex == ImageIndexes.LITERALPARAMETERIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTLITERALPARAMETERIMAGEINDEX;
        }

        public bool IsMethodNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{A0B43A22-1119-44E6-8B88-8C97210F6617}");

            return treeNode.ImageIndex == ImageIndexes.METHODIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTMETHODIMAGEINDEX;
        }

        public bool IsObjectTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{7037291D-B25C-4401-B6EB-D03E897EB992}");

            return treeNode.ImageIndex == ImageIndexes.OBJECTPARAMETERIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTOBJECTPARAMETERIMAGEINDEX;
        }

        public bool IsParameterNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{CFDF8341-5B16-4AE5-9BDA-D072C1B78D57}");

            return treeNode.ImageIndex == ImageIndexes.LITERALPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OBJECTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.GENERICPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.GENERICLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTLITERALPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTOBJECTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTGENERICPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTLITERALLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTOBJECTLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTGENERICLISTPARAMETERIMAGEINDEX;
        }

        public bool IsProjectRootNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{547E56C0-5168-4F19-8EB6-66CE301580E0}");

            return treeNode.Parent == null 
                && treeNode.ImageIndex == ImageIndexes.PROJECTFOLDERIMAGEINDEX;
        }

        public bool IsRootNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{D21F608D-8F19-42EF-9260-D0F9AC42F2B1}");

            return treeNode.Parent == null;
        }

        public bool IsVariableNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{F005F8A5-4883-4D8B-B867-E9D66B5CAB36}");

            return treeNode.ImageIndex == ImageIndexes.VARIABLEIMAGEINDEX || treeNode.ImageIndex == ImageIndexes.CUTVARIABLEIMAGEINDEX;
        }

        public bool IsVariableTypeNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{21CE133C-3B37-4E54-9AE4-D45CBAA58183}");

            return treeNode.ImageIndex == ImageIndexes.LITERALPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OBJECTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTLITERALPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTOBJECTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTLITERALLISTPARAMETERIMAGEINDEX
                || treeNode.ImageIndex == ImageIndexes.CUTOBJECTLISTPARAMETERIMAGEINDEX;
        }

        public void MakeVisible(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{CFC98C76-EDF9-4568-8C47-807899C3A228}");

            if (treeNode.Parent == null)
                return;

            if (!treeNode.Parent.Expanded)
                treeNode.Parent.Expand();

            treeNode.EnsureVisible();
        }

        public void RenameChildFolderTreeNode(RadTreeNode parentTreeNode, RadTreeNode childNode, string xmlNameAttributeValue)
            => childNode.Name = $"{parentTreeNode.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{xmlNameAttributeValue}\"]";

        public void RenameChildTreeNode(RadTreeNode parentTreeNode, RadTreeNode childNode, string relativeXPath, string idAttributeName, string idAttributeValue)
            => childNode.Name = $"{parentTreeNode.Name}/{relativeXPath}[@{idAttributeName}=\"{idAttributeValue}\"]";

        public void ScrollToPreviousPosition(RadTreeView treeView, System.Drawing.Point point)
        {
            if (point.X < treeView.HScrollBar.Minimum || point.X > treeView.HScrollBar.Maximum)
                return;

            if (point.Y < treeView.VScrollBar.Minimum || point.Y > treeView.VScrollBar.Maximum)
                return;

            treeView.VScrollBar.Value = point.Y;
            treeView.HScrollBar.Value = point.X;
        }

        public void SelectTreeNode(RadTreeView treeView, string? nodeName)
        {
            if (nodeName == null)
                return;

            var node = treeView.Find(n => n.Name == nodeName);
            if (node != null)
                treeView.SelectedNode = node;
        }

        public void SelectTreeNode(RadTreeView treeView, Func<RadTreeNode, bool> predicate)
        {
            var node = treeView.Find(n => predicate(n));
            if (node != null)
                treeView.SelectedNode = node;
        }

        public void SelectTreeNodes(RadTreeView treeView, IList<string> nodeNames)
        {
            foreach (string name in nodeNames)
            {
                var node = treeView.Find(n => n.Name == name);
                if (node != null && node.Selected == false)
                    node.Selected = true;
            }
        }

        private bool AncestorPresent(RadTreeNode treeNode, HashSet<RadTreeNode> allNodesHashSet)
        {
            if (treeNode.Parent == null)
                return false;

            if (allNodesHashSet.Contains(treeNode.Parent))
                return true;

            return AncestorPresent(treeNode.Parent, allNodesHashSet);
        }
    }
}
