using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface IConfigureFunctionGenericArgumentsTreeViewBuilder
    {
        void Build(RadTreeView treeView, XmlDocument xmlDocument);
    }
}
