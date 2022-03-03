using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IAssemblyLoadContextManager
    {
        Dictionary<string, LogicBuilderAssemblyLoadContext>? GetAssemblyLoadContextDictionary();
        LogicBuilderAssemblyLoadContext GetAssemblyLoadContext();
        void UnloadLoadContexts();
        void CreateLoadContexts();
    }
}
