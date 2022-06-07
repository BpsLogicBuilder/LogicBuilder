using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface IGetAllCheckedNodes
    {
        IList<string> GetNames(RadTreeNode rootTreeNode);
        IList<RadTreeNode> GetNodes(RadTreeNode rootTreeNode);
    }
}
