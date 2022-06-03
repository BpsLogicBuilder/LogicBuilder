using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface ISelectRulesTreeViewBuilder
    {
        void Build(RadTreeView treeView, string applicationName);
    }
}
