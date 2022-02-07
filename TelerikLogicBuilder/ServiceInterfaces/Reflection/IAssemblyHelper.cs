using System;
using System.Collections.Generic;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface IAssemblyHelper
    {
        List<Assembly> GetReferencedAssembliesRecursively(Assembly assembly);
        Type GetType(Assembly assembly, string className, bool throwOnError);
        Type[] GetTypes(Assembly assembly, Dictionary<string, Exception> failedTypes);
    }
}
