using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Reflection
{
    internal class AssemblyLoader : IAssemblyLoader
    {
        private readonly IAssemblyLoadContextManager _assemblyLoadContextService;
        private readonly IPathHelper _pathHelper;

        public AssemblyLoader(IAssemblyLoadContextManager assemblyLoadContextService, IPathHelper pathHelper)
        {
            _assemblyLoadContextService = assemblyLoadContextService;
            _pathHelper = pathHelper;
        }

        public Assembly? LoadAssembly(string activityAssemblyFullName) 
            => new LoadAssemblyFromPathUtility
            (
                activityAssemblyFullName,
                _assemblyLoadContextService.GetAssemblyLoadContext()
            ).LoadAssembly();

        public Assembly? LoadAssembly(AssemblyName assemblyName, string activityAssemblyFullName, string[] paths)
        {
            return new LoadAssemblyFromNameUtility
            (
                _assemblyLoadContextService.GetAssemblyLoadContext(),
                _pathHelper,
                activityAssemblyFullName,
                paths
            ).LoadAssembly(assemblyName);
        }
    }
}
