using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface IGetAllCheckedNodeNames
    {
        IList<string> GetNames(RadTreeNode rootTreeNode);
    }
}
