using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Helpers
{
    internal interface IFileSystemDragDropHandler
    {
        void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes);
    }
}
