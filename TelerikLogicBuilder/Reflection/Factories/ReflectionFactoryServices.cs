using ABIS.LogicBuilder.FlowBuilder.Reflection.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.Reflection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ReflectionFactoryServices
    {
        internal static IServiceCollection AddReflectionFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string[], ILoadAssemblyFromName>>
                (
                    provider =>
                    (activityAssemblyFullName, paths) => new LoadAssemblyFromName
                    (
                        provider.GetRequiredService<IAssemblyLoadContextManager>(),
                        provider.GetRequiredService<IPathHelper>(),
                        activityAssemblyFullName,
                        paths
                    )
                )
                .AddTransient<IReflectionFactory, ReflectionFactory>();
        }
    }
}
