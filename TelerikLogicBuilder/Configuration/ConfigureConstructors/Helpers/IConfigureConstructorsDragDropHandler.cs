using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal interface IConfigureConstructorsDragDropHandler
    {
        void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes);
    }
}
