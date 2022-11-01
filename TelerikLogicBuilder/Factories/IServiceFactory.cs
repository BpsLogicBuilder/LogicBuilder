using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal interface IServiceFactory
    {
        ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema);
    }
}
