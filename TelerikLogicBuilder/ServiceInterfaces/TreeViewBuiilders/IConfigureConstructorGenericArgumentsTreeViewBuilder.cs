using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface IConfigureConstructorGenericArgumentsTreeViewBuilder
    {
        void Build(RadTreeView treeView, XmlDocument xmlDocument);
    }
}
