using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
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
        private readonly IAssemblyLoader _assemblyLoader;
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public AssemblyHelper(IContextProvider contextProvider, IAssemblyLoader assemblyLoader)
        {
            _configurationService = contextProvider.ConfigurationService;
            _pathHelper = contextProvider.PathHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _assemblyLoader = assemblyLoader;
        }

        public List<Assembly> GetReferencedAssembliesRecursively(Assembly assembly)
        {
            if (assembly.FullName == null)
                throw _exceptionHelper.CriticalException("{4E3CC43C-3B6F-4177-9D2A-1E6118D891F7}");

            Dictionary<string, Assembly> assemblies = new()
            {
                { assembly.FullName, assembly }
            };

            GetReferencedAssembliesRecursively(assembly, assemblies);

            return assemblies.Values.ToList();
        }

        private void GetReferencedAssembliesRecursively(Assembly assembly, Dictionary<string, Assembly> assemblies)
            => assembly.GetReferencedAssemblies()
                .ToList()
                .ForEach
                (
                    assemblyName =>
                    {
                        Assembly? referencedAssembly = null;
                        try
                        {
                            referencedAssembly = _assemblyLoader.LoadAssembly
                            (
                                assemblyName,
                                GetAssemblyFullName(_configurationService.GetSelectedApplication()), 
                                Array.Empty<string>()
                            );

                            string GetAssemblyFullName(Application application)
                                => _pathHelper.CombinePaths
                                (
                                    _configurationService.ProjectProperties.ProjectPath,
                                    ProjectPropertiesConstants.BINFOLDER,
                                    application.Name,
                                    application.ActivityAssembly
                                );
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
                );

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
                types.AddRange(typeof(string).Assembly.GetTypes());
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
