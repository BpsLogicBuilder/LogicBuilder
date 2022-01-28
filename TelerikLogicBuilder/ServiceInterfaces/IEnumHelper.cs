using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IEnumHelper
    {
        ListType GetListType(Type memberType);
        LiteralFunctionReturnType GetLiteralFunctionReturnType(Type functionReturnType);
        LiteralParameterType GetLiteralParameterType(Type parameterType);
        LiteralType GetLiteralType(Type literalType);
        LiteralVariableType GetLiteralVariableType(Type variableType);
        ParameterCategory GetParameterCategory(string elementName);
        Type GetSystemType(LiteralFunctionReturnType functionReturnType);
        Type GetSystemType(LiteralParameterType parameterType);
        Type GetSystemType(LiteralType literalType);
        Type GetSystemType(LiteralVariableType variableType);
        string GetTypeDescription(ListType listType, string elementType);
        string GetVisibleEnumText<T>(T enumType);
        T ParseEnumText<T>(string text);
    }
}
