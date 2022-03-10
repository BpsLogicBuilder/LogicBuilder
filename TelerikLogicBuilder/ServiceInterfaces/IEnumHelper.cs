using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IEnumHelper
    {
        string BuildValidReferenceDefinition(string referenceDefinition);
        bool CanBeInteger(LiteralVariableType variableType);
        IList<string> ConvertEnumListToStringList<T>(IList<T>? excludedItems = null);
        IList<T> ConvertToEnumList<T>(IEnumerable<string> enumNames);
        GenericConfigCategory GetGenericConfigCategory(string elementName);
        ListType GetListType(Type memberType);
        LiteralFunctionReturnType GetLiteralFunctionReturnType(Type functionReturnType);
        LiteralListElementType GetLiteralListElementType(Type literalType);
        LiteralParameterType GetLiteralParameterType(Type parameterType);
        LiteralType GetLiteralType(Type literalType);
        LiteralVariableType GetLiteralVariableType(Type variableType);
        ObjectCategory GetObjectCategory(string elementName);
        ParameterCategory GetParameterCategory(string elementName);
        ReturnTypeCategory GetReturnTypeCategory(string elementName);
        Type GetSystemType(LiteralFunctionReturnType functionReturnType);
        Type GetSystemType(LiteralListElementType literalType);
        Type GetSystemType(LiteralParameterType parameterType);
        Type GetSystemType(LiteralType literalType);
        Type GetSystemType(LiteralVariableType variableType);
        string GetTypeDescription(ListType listType, string elementType);
        VariableTypeCategory GetVariableTypeCategory(string elementName);
        string GetVisibleEnumText<T>(T enumType);
        bool IsValidCodeBinaryOperator(string item);
        T ParseEnumText<T>(string text);
        IDictionary<string, ValidIndirectReference> ToValidIndirectReferenceDictionary();
        string GetValidIndirectReferencesList();
        string GetValidCategoriesList();
    }
}
