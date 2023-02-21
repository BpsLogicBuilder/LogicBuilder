using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IConstructorTypeHelper
    {
        ClosedConstructor? GetConstructor(Type constructorType, ApplicationTypeInfo application);
        ClosedConstructor? GetConstructor(string objectType, ApplicationTypeInfo application);
        IDictionary<string, Constructor> GetConstructors(Type constructorType, ApplicationTypeInfo application);
        IDictionary<string, Constructor> GetConstructors(string objectType, ApplicationTypeInfo application);
    }
}
