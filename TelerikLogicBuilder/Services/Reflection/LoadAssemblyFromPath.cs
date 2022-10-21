using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System;
using System.IO;
using System.Reflection;
using System.Security;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Reflection
{
    internal class LoadAssemblyFromPath : ILoadAssemblyFromPath
    {
        private readonly IAssemblyLoadContextManager _assemblyLoadContextManager;

        public LoadAssemblyFromPath(IAssemblyLoadContextManager assemblyLoadContextManager)
        {
            _assemblyLoadContextManager = assemblyLoadContextManager;
        }

        public Assembly? LoadAssembly(string activityAssemblyFullName)
        {
            if (string.IsNullOrEmpty(activityAssemblyFullName))
                return null;

            LogicBuilderAssemblyLoadContext assemblyLoadContext = _assemblyLoadContextManager.GetAssemblyLoadContext();

            Assembly? assembly = null;
            try
            {
                if (File.Exists(activityAssemblyFullName))
                    assembly = assemblyLoadContext.LoadFromFileStream(activityAssemblyFullName);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }

            return assembly;
        }
    }
}
