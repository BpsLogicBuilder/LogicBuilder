using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface IObjectListParameterElementInfoHelper
    {
        ObjectListParameterElementInfo GetDefaultObjectListElementInfo();
        ObjectListParameterElementInfo GetObjectListElementInfo(ListOfObjectsParameter objectListParameter);
        ObjectListParameterElementInfo GetObjectListElementInfo(ObjectListData listData);
    }
}
