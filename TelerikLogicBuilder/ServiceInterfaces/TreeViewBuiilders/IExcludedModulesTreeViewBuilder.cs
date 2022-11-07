using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface IExcludedModulesTreeViewBuilder
    {
        void Build(RadTreeView treeView, IList<string> selectedModules);
    }
}
