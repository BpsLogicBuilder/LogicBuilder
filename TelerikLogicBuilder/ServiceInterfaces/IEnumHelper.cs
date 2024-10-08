﻿using ABIS.LogicBuilder.FlowBuilder.Enums;
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
        IList<string> ConvertVisibleDropDownValuesToEnumNames<T>(ICollection<string> array);
        ListType GetConcreteListType(ListType listType);
        string GetEnumResourceString(string? enumName);
        GenericConfigCategory GetGenericConfigCategory(string elementName);
        ValidIndirectReference GetIndexReferenceDefinition(Type indexType);
        VariableCategory GetIndexVariableCategory(Type indexType);
        Type GetVariableCategoryIndexType(VariableCategory variableCategory);
        ListType GetListType(Type memberType);
        LiteralFunctionReturnType GetLiteralFunctionReturnType(Type functionReturnType);
        LiteralListElementType GetLiteralListElementType(LiteralParameterType literalType);
        LiteralListElementType GetLiteralListElementType(Type literalType);
        LiteralListElementType GetLiteralListElementType(LiteralVariableType literalType);
        LiteralParameterType GetLiteralParameterType(LiteralListElementType literalType);
        LiteralParameterType GetLiteralParameterType(Type parameterType);
        LiteralType GetLiteralType(Type literalType);
        LiteralVariableType GetLiteralVariableType(LiteralListElementType literalType);
        LiteralVariableType GetLiteralVariableType(Type variableType);
        ObjectCategory GetObjectCategory(string elementName);
        ParameterCategory GetParameterCategory(string elementName);
        ReturnTypeCategory GetReturnTypeCategory(string elementName);
        Type GetSystemType(ListType listType, Type elementType);
        Type GetSystemType(LiteralFunctionReturnType functionReturnType);
        Type GetSystemType(LiteralListElementType literalType);
        Type GetSystemType(LiteralParameterType parameterType);
        Type GetSystemType(LiteralType literalType);
        Type GetSystemType(LiteralVariableType variableType);
        string GetTypeDescription(ListType listType, string elementType);
        ValidIndirectReference GetValidIndirectReference(VariableCategory variableCategory);
        VariableCategory GetVariableCategory(ValidIndirectReference validIndirectReference);
        VariableTypeCategory GetVariableTypeCategory(string elementName);
        string GetValidIndirectReferencesList();
        string GetValidCategoriesList();
        string GetVisibleEnumText<T>(T enumType);
        bool IsValidCodeBinaryOperator(string item);
        T ParseEnumText<T>(string text);
        IDictionary<string, ValidIndirectReference> ToValidIndirectReferenceDictionary();
        IList<string> ToVisibleDropdownValues(ICollection<string> array);
    }
}
