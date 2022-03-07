using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITypeLoadHelper
    {
        Type? TryGetType(string typeName, ApplicationTypeInfo application);
    }
}
