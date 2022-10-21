using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface ILoadAssemblyFromName
    {
        Assembly? LoadAssembly(AssemblyName assemblyName);
    }
}
