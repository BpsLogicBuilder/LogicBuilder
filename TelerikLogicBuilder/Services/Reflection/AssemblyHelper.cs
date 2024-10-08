﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Reflection.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Reflection
{
    internal class AssemblyHelper : IAssemblyHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IPathHelper _pathHelper;
        private readonly IReflectionFactory _reflectionFactory;

        private ILoadAssemblyFromName? loadAssemblyFromName;

        public AssemblyHelper(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IPathHelper pathHelper,
            IReflectionFactory reflectionFactory)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _pathHelper = pathHelper;
            _reflectionFactory = reflectionFactory;
        }

        public List<Assembly> GetReferencedAssembliesRecursively(Assembly assembly)
        {
            if (assembly.FullName == null)
                throw _exceptionHelper.CriticalException("{4E3CC43C-3B6F-4177-9D2A-1E6118D891F7}");

            Application application = _configurationService.GetSelectedApplication();
            loadAssemblyFromName = _reflectionFactory.GetLoadAssemblyFromName
            (
                GetAssemblyFullName(),
                application.LoadAssemblyPaths.ToArray()
            );

            Dictionary<string, Assembly> assemblies = new()
            {
                { assembly.FullName, assembly }
            };

            GetReferencedAssembliesRecursively(assembly, assemblies);

            return assemblies.Values.ToList();

            string GetAssemblyFullName() => _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ProjectPropertiesConstants.BINFOLDER,
                application.Name,
                application.ActivityAssembly
            );
        }

        private void GetReferencedAssembliesRecursively(Assembly assembly, Dictionary<string, Assembly> assemblies)
        {
            if (this.loadAssemblyFromName == null)
                throw _exceptionHelper.CriticalException("{9E61DB32-A9C5-408E-8950-7CA053D49346}");

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                Assembly? referencedAssembly = null;
                try
                {
                    referencedAssembly = loadAssemblyFromName.LoadAssembly(assemblyName);
                }
                catch (Exception)
                {
                }

                if (referencedAssembly == null || referencedAssembly.FullName == null)
                    return;

                if (!assemblies.ContainsKey(referencedAssembly.FullName))
                {
                    assemblies.Add(referencedAssembly.FullName, referencedAssembly);
                    GetReferencedAssembliesRecursively(referencedAssembly, assemblies);
                }
            }
        }

        public Type? GetType(Assembly assembly, string className, bool throwOnError)
        {
            try
            {
                return assembly.GetType(className, throwOnError);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (FileLoadException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (TypeLoadException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ReflectionTypeLoadException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public Type[] GetTypes(Assembly assembly, Dictionary<string, Exception> failedTypes)
        {
            List<Type?> types = new();
            try
            {
                types.AddRange(assembly.GetTypes());
            }
            catch (ReflectionTypeLoadException e)
            {
                failedTypes = e.LoaderExceptions.Aggregate(failedTypes, (dictionary, next) =>
                {
                    if (next != null && !dictionary.ContainsKey(next.Message))
                        dictionary.Add(next.Message, next);
                    return dictionary;
                });

                types.AddRange(e.Types);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (FileLoadException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (TypeLoadException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }

            return types
                .Where(t => t != null)
                .Cast<Type>()
                .ToArray();
        }
    }
}
