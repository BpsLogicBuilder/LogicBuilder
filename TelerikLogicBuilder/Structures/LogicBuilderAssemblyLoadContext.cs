﻿using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class LogicBuilderAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly string activityAssemblyFullName;
        private readonly AssemblyDependencyResolver _resolver;

        #region Constants
        #endregion Constants


        public LogicBuilderAssemblyLoadContext(string activityAssemblyFullName) : base(isCollectible: true)
        {
            this.activityAssemblyFullName = activityAssemblyFullName;
            if (File.Exists(this.activityAssemblyFullName))
                _resolver = new AssemblyDependencyResolver(this.activityAssemblyFullName);
        }

        protected override Assembly Load(AssemblyName name)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(name);
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
