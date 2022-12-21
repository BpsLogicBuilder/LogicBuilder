using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal interface IConfigureVariablesDragDropHandler
    {
        void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes);
    }
}
