using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;
using WinRT;

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
        {
            RadTreeNode treeNode = new()
            {
                ImageIndex = imageIndex,
                Text = idAttributeValue,
                Name = $"{parentTreeNode.Name}/{relativeXPath}[@{idAttributeName}=\"{idAttributeValue}\"]",
                ToolTipText = toolTipText ?? string.Empty
            };

            if (nodeIndex != null)
            {
                treeNode.Name = $"{treeNode.Name}[{nodeIndex}]";
                treeNode.Text = $"{treeNode.Text}[{nodeIndex}]";
            }

            parentTreeNode.Nodes.Add(treeNode);

            return treeNode;
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

        public RadTreeNode GetChildTreeNode(RadTreeNode parentTreeNode, string relativeXPath, string idAttributeName, string idAttributeValue, int imageIndex, string? toolTipText = null)
        {
            return new()
            {
                ImageIndex = imageIndex,
                Text = idAttributeValue,
                Name = $"{parentTreeNode.Name}/{relativeXPath}[@{idAttributeName}=\"{idAttributeValue}\"]",
                ToolTipText = toolTipText ?? string.Empty
            };
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

        public bool IsApplicationNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{92F0C74F-8DD6-41DA-A3F6-997D5386D3F0}");

            return treeNode.ImageIndex == ImageIndexes.APPLICATIONIMAGEINDEX;
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
                || treeNode.ImageIndex == ImageIndexes.CUTTABLEFILEIMAGEINDEX;
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

        public bool IsMethodNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{A0B43A22-1119-44E6-8B88-8C97210F6617}");

            return treeNode.ImageIndex == ImageIndexes.METHODIMAGEINDEX;
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
