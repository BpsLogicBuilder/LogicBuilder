using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal interface IServiceFactory
    {
        ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(RadTreeView treeView,
            SchemaName schema);
    }
}
