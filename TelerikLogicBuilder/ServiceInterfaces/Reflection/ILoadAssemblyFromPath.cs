using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface ILoadAssemblyFromPath
    {
        Assembly? LoadAssembly(string activityAssemblyFullName);
    }
}
