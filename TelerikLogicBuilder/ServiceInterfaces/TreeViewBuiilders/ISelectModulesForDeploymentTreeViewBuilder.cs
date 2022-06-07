using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface ISelectModulesForDeploymentTreeViewBuilder
    {
        void Build(RadTreeView treeView, string applicationName);
    }
}
