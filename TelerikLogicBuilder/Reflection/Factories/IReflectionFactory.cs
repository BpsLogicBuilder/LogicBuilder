using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Reflection.Factories
{
    internal interface IReflectionFactory
    {
        ILoadAssemblyFromName GetLoadAssemblyFromName(string activityAssemblyFullName, string[] paths);
    }
}
