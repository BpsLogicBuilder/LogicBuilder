using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface IObjectListVariableElementInfoHelper
    {
        ObjectListVariableElementInfo GetDefaultObjectListElementInfo();
        ObjectListVariableElementInfo GetObjectListElementInfo(ListOfObjectsVariable objectListVariable);
        ObjectListVariableElementInfo GetObjectListElementInfo(ObjectListData listData);
    }
}
