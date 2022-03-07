using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.IO;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TypeLoadHelper : ITypeLoadHelper
    {
        private readonly IAssemblyLoadContextManager _assemblyLoadContextService;

        public TypeLoadHelper(IAssemblyLoadContextManager assemblyLoadContextService)
        {
            _assemblyLoadContextService = assemblyLoadContextService;
        }

        public Type? TryGetType(string typeName, ApplicationTypeInfo application)
        {
            if (application.AssemblyAvailable
                    && application.AllTypes.TryGetValue(typeName, out Type? type))
                return type;

            try
            {
                if ((type = Type.GetType(typeName, ResolveAssembly, ResolveType)) != null)
                    return type;
            }
            catch (FileLoadException)
            {
                return null;
            }

            return null;

            Assembly? ResolveAssembly(AssemblyName assemblyName)
            {
                if (application.AllAssembliesDictionary.TryGetValue(assemblyName.FullName, out Assembly? assembly))
                    return assembly;

                if (typeof(string).Assembly.GetName().Name == assemblyName.Name)
                    return typeof(string).Assembly;

                return LoadAssembly(assemblyName);
            }

            static Type? ResolveType(Assembly? assembly, string typeName, bool ignoreCase)
            {
                if (assembly != null)
                    return assembly.GetType(typeName);

                return Type.GetType(typeName, false, ignoreCase);
            }

            Assembly? LoadAssembly(AssemblyName assemblyName)
            {
                try
                {
                    return _assemblyLoadContextService.GetAssemblyLoadContext().LoadFromAssemblyName(assemblyName);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
