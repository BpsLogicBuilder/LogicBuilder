using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers
{
    internal class ConfigureFragmentsDragDropService : TreeViewDragDropService
    {
        private RadTreeNode? draggedNode;

        public ConfigureFragmentsDragDropService(RadTreeViewElement owner) : base(owner)
        {
        }

        private IConfigureFragmentsDragDropHandler ConfigureFragmentsDragDropHandler
        {
            get
            {
                if (this.draggedNode?.TreeView is not ConfigureFragmentsTreeView configureFragmentsTreeView)
                    throw new ArgumentException($"{nameof(this.draggedNode)}: {{712BAF2D-ACF3-4C75-A460-6E06962A8B86}}");

                return configureFragmentsTreeView.ConfigureFragmentsDragDropHandler;
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

            ConfigureFragmentsDragDropHandler.DragDrop
            (
                targetNodeElement.Data,
                this.GetDraggedNodes(this.draggedNode)
            );
        }
    }
}
