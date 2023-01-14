using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal class ConfigureConstructorsDragDropService : TreeViewDragDropService
    {
        private RadTreeNode? draggedNode;

        public ConfigureConstructorsDragDropService(RadTreeViewElement owner) : base(owner)
        {
        }

        private IConfigureConstructorsDragDropHandler ConfigureConstructorsDragDropHandler
        {
            get
            {
                if (this.draggedNode?.TreeView is not ConfigureConstructorsTreeView configureConstructorsTreeView)
                    throw new ArgumentException($"{nameof(this.draggedNode)}: {{9A6FC76C-50C0-4F1F-937A-B0DB7F3E1F59}}");

                return configureConstructorsTreeView.ConfigureConstructorsDragDropHandler;
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

            ConfigureConstructorsDragDropHandler.DragDrop
            (
                targetNodeElement.Data,
                this.GetDraggedNodes(this.draggedNode)
            );
        }
    }
}
