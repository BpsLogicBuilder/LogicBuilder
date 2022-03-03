using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.IO;
using System.Reflection;
using System.Security;

namespace ABIS.LogicBuilder.FlowBuilder.Reflection
{
    internal class LoadAssemblyFromPathUtility
    {
        private readonly string activityAssemblyFullName;
        private readonly LogicBuilderAssemblyLoadContext assemblyLoadContext;

        public LoadAssemblyFromPathUtility(string activityAssemblyFullName, LogicBuilderAssemblyLoadContext assemblyLoadContext)
        {
            this.activityAssemblyFullName = activityAssemblyFullName;
            this.assemblyLoadContext = assemblyLoadContext;
        }

        internal Assembly? LoadAssembly()
        {
            if (String.IsNullOrEmpty(this.activityAssemblyFullName))
                return null;

            Assembly? assembly = null;
            try
            {
                if (File.Exists(this.activityAssemblyFullName))
                    assembly = assemblyLoadContext.LoadFromFileStream(this.activityAssemblyFullName);
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
