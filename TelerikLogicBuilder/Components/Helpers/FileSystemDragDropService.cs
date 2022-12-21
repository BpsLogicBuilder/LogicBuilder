using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Helpers
{
    internal class FileSystemDragDropService : TreeViewDragDropService
    {
        private RadTreeNode? draggedNode;

        public FileSystemDragDropService(
            RadTreeViewElement owner) : base(owner)
        {
        }

        //can't inject because FileSystemDragDropService is created before FileSystemTreeViewElement(passed in)'s constructor runs
        private IFileSystemDragDropHandler FileSystemDragDropHandler
        {
            get
            {
                if (this.draggedNode?.TreeView is not FileSystemTreeView fileSystemTreeView)
                    throw new ArgumentException($"{nameof(this.draggedNode)}: {{CD614B2D-7EC1-4911-BCFE-029574186B7E}}");

                return fileSystemTreeView.FileSystemDragDropHandler;
            }
        }

        protected override void PerformStart()
        {
            base.PerformStart();
            TreeNodeElement draggedNodeElement = (TreeNodeElement)this.Context;
            this.draggedNode = draggedNodeElement.Data;
        }

        protected override void PerformStop()
        {
            base.PerformStop();
            this.draggedNode = null;
        }

        protected override void OnPreviewDragOver(RadDragOverEventArgs e)
        {
            base.OnPreviewDragOver(e);
            if (e.HitTarget is RadTreeViewElement targetElement
                && targetElement == this.Owner
                && this.draggedNode != null
                && this.draggedNode != this.Owner.Nodes[0])
            {
                e.CanDrop = true;
            }
        }

        protected override void OnPreviewDragDrop(RadDropEventArgs e)
        {
            TreeNodeElement? targetNodeElement = e.HitTarget as TreeNodeElement;
            RadTreeViewElement? targetTreeView = targetNodeElement == null
                ? e.HitTarget as RadTreeViewElement
                : targetNodeElement.TreeViewElement;

            if (targetTreeView == null
                || targetTreeView != this.Owner
                || targetNodeElement == null
                || this.draggedNode == null)
            {
                return;
            }

            if (this.draggedNode.TreeView.Nodes[0].Selected)
                return;

            FileSystemDragDropHandler.DragDrop(targetNodeElement.Data, this.GetDraggedNodes(this.draggedNode));
        }
    }
}
