using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal interface IConfigureFunctionsDragDropHandler
    {
        void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes);
    }
}
