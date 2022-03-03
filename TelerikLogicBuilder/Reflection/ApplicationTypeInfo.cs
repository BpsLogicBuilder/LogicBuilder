using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Reflection
{
    internal class ApplicationTypeInfo
    {
        private readonly IPathHelper _pathHelper;

        public ApplicationTypeInfo(IPathHelper pathHelper, Application application, bool assemblyAvailable)
        {
            _pathHelper = pathHelper;
            Application = application;
            AllTypes = new();
            AllAssemblies = new();
            AssemblyAvailable = assemblyAvailable;
        }

        public ApplicationTypeInfo(IPathHelper pathHelper, Application application, Type activityType, SortedDictionary<string, Type> allTypes, List<Assembly> allAssemblies, bool assemblyAvailable)
        {
            _pathHelper = pathHelper;
            Application = application;
            ActivityType = activityType;
            AllTypes = allTypes;
            AllAssemblies = allAssemblies;
            AssemblyAvailable = assemblyAvailable;
        }

        internal Application Application { get; }
        
        internal Type? ActivityType { get; }
        internal SortedDictionary<string, Type> AllTypes { get; }
        internal List<string> AllTypesList => AllTypes.Keys.ToList();
        internal List<Assembly> AllAssemblies { get; }
        [MemberNotNullWhen(true, nameof(ActivityType))]
        internal bool AssemblyAvailable { get; }

        internal Dictionary<string, Assembly> AllAssembliesDictionary => AllAssemblies.ToDictionary(assembly => assembly.FullName!);
        internal string UnavailableMessage
            => string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.assemblyUnavailableMessageFormat,
                _pathHelper.CombinePaths
                (
                    this.Application.ActivityAssemblyPath,
                    this.Application.ActivityAssembly
                ),
                this.Application.ActivityClass
            );
    }
}
