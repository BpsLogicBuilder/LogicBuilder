using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers
{
    internal interface IConfigureFragmentsDragDropHandler
    {
        void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes);
    }
}
