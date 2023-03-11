using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectListVariableElementInfoHelper : IObjectListVariableElementInfoHelper
    {
        private readonly IEnumHelper _enumHelper;

        public ObjectListVariableElementInfoHelper(
            IEnumHelper enumHelper)
        {
            _enumHelper = enumHelper;
        }

        public ObjectListVariableElementInfo GetDefaultObjectListElementInfo()
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    ListType.GenericList,
                    MiscellaneousConstants.DEFAULT_OBJECT_TYPE
                ),
                ListType.GenericList,
                MiscellaneousConstants.DEFAULT_OBJECT_TYPE,
                ListVariableInputStyle.ListForm,
                string.Empty
            );

        public ObjectListVariableElementInfo GetObjectListElementInfo(ListOfObjectsVariable objectListVariable)
            => new
            (
                objectListVariable.Name,
                objectListVariable.ListType,
                objectListVariable.ObjectType,
                objectListVariable.Control,
                objectListVariable.Comments,
                objectListVariable
            );

        public ObjectListVariableElementInfo GetObjectListElementInfo(ObjectListData listData)
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    listData.ListType,
                    listData.ObjectType
                ),
                listData.ListType,
                listData.ObjectType,
                ListVariableInputStyle.ListForm,
                string.Empty
            );
    }
}
