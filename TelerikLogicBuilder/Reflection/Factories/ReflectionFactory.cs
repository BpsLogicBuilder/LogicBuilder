using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Reflection.Factories
{
    internal class ReflectionFactory : IReflectionFactory
    {
        private readonly Func<string, string[], ILoadAssemblyFromName> _getLoadAssemblyFromName;

        public ReflectionFactory(Func<string, string[], ILoadAssemblyFromName> getLoadAssemblyFromName)
        {
            _getLoadAssemblyFromName = getLoadAssemblyFromName;
        }

        public ILoadAssemblyFromName GetLoadAssemblyFromName(string activityAssemblyFullName, string[] paths)
            => _getLoadAssemblyFromName(activityAssemblyFullName, paths);
    }
}
