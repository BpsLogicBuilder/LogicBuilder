using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface ICheckSelectedTreeNodes
    {
        void CheckListedNodes(RadTreeNode parentNode, HashSet<string> nodeNames);
    }
}
