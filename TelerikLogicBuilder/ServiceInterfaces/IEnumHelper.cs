using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IEnumHelper
    {
        string BuildValidReferenceDefinition(string referenceDefinition);
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
        VariableTypeCategory GetVariableTypeCategory(string elementName);
        string GetVisibleEnumText<T>(T enumType);
        T ParseEnumText<T>(string text);
        IDictionary<string, ValidIndirectReference> ToValidIndirectReferenceDictionary();
    }
}
