using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ConfigureVariablesDragDropService : TreeViewDragDropService
    {
        private RadTreeNode? draggedNode;

        public ConfigureVariablesDragDropService(RadTreeViewElement owner) : base(owner)
        {
        }

        private IConfigureVariablesDragDropHandler ConfigureVariablesDragDropHandler
        { 
            get
            {
                if (this.draggedNode?.TreeView is not ConfigureVariablesTreeView configureVariablesTreeView)
                    throw new ArgumentException($"{nameof(this.draggedNode)}: {{D2593D76-675D-4B6C-BF24-07A7B106F781}}");

                return configureVariablesTreeView.ConfigureVariablesDragDropHandler;
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

            ConfigureVariablesDragDropHandler.DragDrop
            (
                targetNodeElement.Data, 
                this.GetDraggedNodes(this.draggedNode)
            );
        }
    }
}
