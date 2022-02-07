using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface IAssemblyLoader
    {
        Assembly LoadAssembly(string activityAssemblyFullName);
        Assembly LoadAssembly(AssemblyName assemblyName, string activityAssemblyFullName, string[] paths);
    }
}
