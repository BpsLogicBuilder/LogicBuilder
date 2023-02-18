using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface IObjectListElementInfoHelper
    {
        ObjectListElementInfo GetDefaultObjectListElementInfo();
        ObjectListElementInfo GetObjectListElementInfo(ListOfObjectsParameter objectListParameter);
        ObjectListElementInfo GetObjectListElementInfo(ObjectListData listData);
    }
}
