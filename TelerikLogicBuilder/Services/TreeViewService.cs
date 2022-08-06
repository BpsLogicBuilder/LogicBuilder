using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Drawing;
using System.Windows.Forms;
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

        public bool IsFileNode(RadTreeNode treeNode)
        {
            return !IsRootNode(treeNode) && !IsFolderNode(treeNode);
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

        public bool IsMethodNode(RadTreeNode treeNode)
        {
            if (treeNode == null)
                throw _exceptionHelper.CriticalException("{A0B43A22-1119-44E6-8B88-8C97210F6617}");

            return treeNode.ImageIndex == ImageIndexes.METHODIMAGEINDEX;
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

        public void ScrollToPreviousPosition(RadTreeView treeView, Point point)
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
    }
}
