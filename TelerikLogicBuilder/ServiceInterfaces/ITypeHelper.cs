using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITypeHelper
    {
        string GetTypeDescription(Type type);
        Type GetUndelyingTypeForValidList(Type type);
        bool IsLiteralType(Type type);
        bool IsNullable(Type type);
        bool IsValidConnectorList(Type type);
        bool IsValidList(Type type);
        bool IsValidLiteralReturnType(Type type);
        string ToId(Type type);
        Type TryGetType(string typeName, ApplicationTypeInfo application);
        bool TryParse(string toParse, Type type, out object result);
    }
}
