using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories
{
    internal class LoadAssemblyPathsItemFactory : ILoadAssemblyPathsItemFactory
    {
        private readonly Func<string, AssemblyPath> _getAssemblyPath;

        public LoadAssemblyPathsItemFactory(Func<string, AssemblyPath> getAssemblyPath)
        {
            _getAssemblyPath = getAssemblyPath;
        }

        public AssemblyPath GetAssemblyPath(string path)
            => _getAssemblyPath(path);
    }
}
