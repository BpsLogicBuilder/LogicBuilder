using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal class ConfigureFunctionsDragDropService : TreeViewDragDropService
    {
        private RadTreeNode? draggedNode;

        public ConfigureFunctionsDragDropService(RadTreeViewElement owner) : base(owner)
        {
        }

        private IConfigureFunctionsDragDropHandler ConfigureFunctionsDragDropHandler
        {
            get
            {
                if (this.draggedNode?.TreeView is not ConfigureFunctionsTreeView configureFunctionsTreeView)
                    throw new ArgumentException($"{nameof(this.draggedNode)}: {{2F8BFC13-A781-4A14-A089-B542196BABA4}}");

                return configureFunctionsTreeView.ConfigureFunctionsDragDropHandler;
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

            ConfigureFunctionsDragDropHandler.DragDrop
            (
                targetNodeElement.Data,
                this.GetDraggedNodes(this.draggedNode)
            );
        }
    }
}
