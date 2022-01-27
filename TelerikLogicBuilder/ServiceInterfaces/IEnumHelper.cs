using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IEnumHelper
    {
        ListType GetListType(Type memberType);
        ParameterType GetParameterType(Type parameterType);
        string GetTypeDescription(ListType listType, string elementType);
        string GetVisibleEnumText<T>(T enumType);
    }
}
