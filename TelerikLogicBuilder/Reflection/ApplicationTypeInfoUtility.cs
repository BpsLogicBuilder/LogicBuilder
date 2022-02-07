using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Reflection
{
    internal class ApplicationTypeInfoUtility
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private readonly IAssemblyLoader _assemblyLoader;
        private readonly IAssemblyHelper _assemblyHelper;
        private readonly ITypeHelper _typeHelper;

        public ApplicationTypeInfoUtility(IConfigurationService configurationService, IPathHelper pathHelper, IAssemblyLoader assemblyLoader, IAssemblyHelper assemblyHelper, ITypeHelper typeHelper)
        {
            _configurationService = configurationService;
            _pathHelper = pathHelper;
            _assemblyLoader = assemblyLoader;
            _assemblyHelper = assemblyHelper;
            _typeHelper = typeHelper;
        }

        internal ApplicationTypeInfo CreateApplicationTypeInfo()
        {
            Application application = _configurationService.GetSelectedApplication();
            Assembly activityAssembly = null;
            HashSet<Assembly> allAssemblies = new();
            Type activityType = null;
            SortedDictionary<string, Type> allTypes = new();

            if (string.IsNullOrEmpty(application.ActivityAssemblyPath) || string.IsNullOrEmpty(application.ActivityAssembly))
            {
                return GetUnavailableApplicationTypeInfo();
            }

            string assemblyFile = _pathHelper.CombinePaths(application.ActivityAssemblyPath, application.ActivityAssembly);
            if (!File.Exists(assemblyFile))
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadAssemblyFormat, assemblyFile), EventLogEntryType.Error);
                return GetUnavailableApplicationTypeInfo();
            }

            string binPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.BINFOLDER, application.Name);
            string assemblyFullName = _pathHelper.CombinePaths(binPath, application.ActivityAssembly);

            try
            {
                activityAssembly = _assemblyLoader.LoadAssembly(assemblyFullName);

                if (activityAssembly != null)
                {
                    activityType = _assemblyHelper.GetType(activityAssembly, application.ActivityClass, true);
                }
                else
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadAssemblyFormat, assemblyFullName);
                    EventLogger.WriteEntry(ApplicationProperties.Name, errorMessage, EventLogEntryType.Error);
                    return GetUnavailableApplicationTypeInfo();
                }

                allAssemblies.Add(activityAssembly);
                Dictionary<string, Exception> failedTypes = new();
                allTypes = new SortedDictionary<string, Type>();

                AppendTypes(_assemblyHelper.GetTypes(activityAssembly, failedTypes));

                _assemblyHelper.GetReferencedAssembliesRecursively(activityAssembly).ForEach(assembly =>
                {
                    allAssemblies.Add(assembly);
                    AppendTypes(_assemblyHelper.GetTypes(assembly, failedTypes));
                });

                if (failedTypes.Any())
                {
                    EventLogger.WriteEntry
                    (
                        ApplicationProperties.Name,
                        string.Join(Environment.NewLine, failedTypes.Keys),
                        EventLogEntryType.Warning
                    );
                }

                if (activityAssembly == null)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadAssemblyFormat, assemblyFullName);
                    EventLogger.WriteEntry(ApplicationProperties.Name, errorMessage, EventLogEntryType.Error);
                    return GetUnavailableApplicationTypeInfo();
                }

                if (activityType == null)
                {
                    string errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadClassFormat, application.ActivityClass);
                    EventLogger.WriteEntry(ApplicationProperties.Name, errorMessage, EventLogEntryType.Error);
                    return GetUnavailableApplicationTypeInfo();
                }

                return new ApplicationTypeInfo
                (
                    _pathHelper,
                    application,
                    activityType,
                    allTypes,
                    allAssemblies.ToList(),
                    true
                );
            }
            catch (ReflectionTypeLoadException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
                return GetUnavailableApplicationTypeInfo();
            }
            catch (LogicBuilderException ex)
            {
                EventLogger.WriteEntry(ApplicationProperties.Name, ex);
                return GetUnavailableApplicationTypeInfo();
            }

            ApplicationTypeInfo GetUnavailableApplicationTypeInfo() 
                => new
                (
                    _pathHelper,
                    application,
                    false
                );

            void AppendTypes(Type[] types)
            {
                foreach (Type type in types)
                {
                    if (type == null || type.BaseType == typeof(MulticastDelegate))
                        continue;

                    string typeId = _typeHelper.ToId(type);
                    if (!allTypes.ContainsKey(typeId))
                        allTypes.Add(typeId, type);
                }
            }
        }
    }
}
