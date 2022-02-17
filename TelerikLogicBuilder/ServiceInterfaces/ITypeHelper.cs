using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.CodeDom;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITypeHelper
    {
        bool AreCompatibleForOperation(Type t1, Type t2, CodeBinaryOperatorType op);

        /// <summary>
        /// "to" is assignable from "from"
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        bool AssignableFrom(Type to, Type from);
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
