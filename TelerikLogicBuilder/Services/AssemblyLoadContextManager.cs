using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class AssemblyLoadContextManager : IAssemblyLoadContextManager
    {
        private readonly IConfigurationService _configurationService;
        private readonly IPathHelper _pathHelper;
        private Dictionary<string, LogicBuilderAssemblyLoadContext>? _loadContexts;

        public AssemblyLoadContextManager(
            IConfigurationService configurationService,
            IPathHelper pathHelper)
        {
            _configurationService = configurationService;
            _pathHelper = pathHelper;
        }

        public void CreateLoadContexts()
        {
            _loadContexts = _configurationService.ProjectProperties.ApplicationList.ToDictionary
            (
                a => a.Key,
                a => new LogicBuilderAssemblyLoadContext
                (
                    GetAssemblyFullName(a.Value)
                )
            );
        }

        public LogicBuilderAssemblyLoadContext GetAssemblyLoadContext()
        {
            if (_loadContexts?.Any() !=  true)
            {
                _loadContexts = _configurationService.ProjectProperties.ApplicationList.ToDictionary
                (
                    a => a.Key,
                    a => new LogicBuilderAssemblyLoadContext
                    (
                        GetAssemblyFullName(a.Value)
                    )
                );
            }

            return _loadContexts[_configurationService.GetSelectedApplicationKey().ToLowerInvariant()];
        }

        public Dictionary<string, LogicBuilderAssemblyLoadContext>? GetAssemblyLoadContextDictionary() => _loadContexts;

        public void UnloadLoadContexts()
        {
            UnloadLoadContexts(out WeakReference alcWeakRef);
            while (alcWeakRef.IsAlive)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void UnloadLoadContexts(out WeakReference alcWeakRef)
        {
            alcWeakRef = new WeakReference(_loadContexts, trackResurrection: true);
            if (_loadContexts?.Any() == true)
            {
                foreach (var kvp in _loadContexts)
                    kvp.Value.Unload();

                _loadContexts.Clear();
            }

            _loadContexts = null;
        }

        private string GetAssemblyFullName(Application application)
            => _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ProjectPropertiesConstants.BINFOLDER,
                application.Name,
                application.ActivityAssembly
            );
    }
}
