using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
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

        public bool TryGetSystemType(GenericConfigBase config, ApplicationTypeInfo application, out Type type)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSystemType(ParameterBase paramter, ApplicationTypeInfo application, out Type type)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSystemType(ReturnTypeBase returnType, IList<GenericConfigBase> GenericArguments, ApplicationTypeInfo application, out Type? type)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSystemType(string typeName, ApplicationTypeInfo application, out Type? type)
        {
            if (application.AssemblyAvailable
                    && application.AllTypes.TryGetValue(typeName, out type))
                return true;

            try
            {
                if ((type = Type.GetType(typeName, ResolveAssembly, ResolveType)) != null)
                    return true;
            }
            catch (FileLoadException)
            {
                type = null;
                return false;
            }

            return false;

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

        public bool TryGetSystemType(VariableBase variable, ApplicationTypeInfo application, out Type? variableType)
        {
            throw new NotImplementedException();
        }

        public bool TryGetSystemTypeForNonGeneric(ReturnTypeBase returnType, ApplicationTypeInfo application, out Type? type)
        {
            throw new NotImplementedException();
        }
    }
}
