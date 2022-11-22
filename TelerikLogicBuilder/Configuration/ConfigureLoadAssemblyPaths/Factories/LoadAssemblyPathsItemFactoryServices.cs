using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class LoadAssemblyPathsItemFactoryServices
    {
        internal static IServiceCollection AddLoadAssemblyPathsItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, AssemblyPath>>
                (
                    provider =>
                    path => new AssemblyPath
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        path
                    )
                )
                .AddTransient<ILoadAssemblyPathsItemFactory, LoadAssemblyPathsItemFactory>();
        }
    }
}
