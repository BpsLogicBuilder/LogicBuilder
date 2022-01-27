using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITypeHelper
    {
        string GetTypeDescription(Type type);
        Type GetUndelyingTypeForValidList(Type type);
        bool IsLiteralType(Type type);
        bool IsNullable(Type type);
        bool IsValidList(Type type);
        string ToId(Type type);
    }
}
