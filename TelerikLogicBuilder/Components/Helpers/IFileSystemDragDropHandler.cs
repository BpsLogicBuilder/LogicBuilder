using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Helpers
{
    internal interface IFileSystemDragDropHandler
    {
        void DragDrop(RadTreeNode destinationNode, RadTreeNode draggingTreeNode);
    }
}
