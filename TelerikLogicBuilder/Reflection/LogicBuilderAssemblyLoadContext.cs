using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ABIS.LogicBuilder.FlowBuilder.Reflection
{
    internal class LogicBuilderAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string activityAssemblyFullName;
        private readonly AssemblyDependencyResolver _resolver;

        #region Constants
        #endregion Constants


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public LogicBuilderAssemblyLoadContext(string activityAssemblyFullName) : base(isCollectible: true)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.activityAssemblyFullName = activityAssemblyFullName;
            if (File.Exists(this.activityAssemblyFullName))
                _resolver = new AssemblyDependencyResolver(this.activityAssemblyFullName);
        }

        protected override Assembly? Load(AssemblyName name)
        {
            if (_resolver == null)
                return null;

            string? assemblyPath = _resolver.ResolveAssemblyToPath(name);
            if (assemblyPath != null)
                return LoadFromFileStream(assemblyPath);

            return null;
        }

        internal Assembly LoadFromFileStream(string assemblyPath)
        {
            using Stream assemblyStream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return LoadFromStream(assemblyStream);
        }
    }
}
