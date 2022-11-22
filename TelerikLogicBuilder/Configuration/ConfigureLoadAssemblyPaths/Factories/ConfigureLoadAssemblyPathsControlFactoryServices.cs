using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLoadAssemblyPathsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureLoadAssemblyPathsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureLoadAssemblyPathsControlFactory, ConfigureLoadAssemblyPathsControlFactory>()
                .AddTransient<Func<IConfigureLoadAssemblyPathsForm, IConfigureLoadAssemblyPathsControl>>
                (
                    provider =>
                    configureLoadAssemblyPathsForm => new ConfigureLoadAssemblyPathsControl
                    (
                        provider.GetRequiredService<IConfigureLoadAssemblyPathsCommandFactory>(),
                        provider.GetRequiredService<ILoadAssemblyPathsItemFactory>(),
                        configureLoadAssemblyPathsForm
                    )
                );
        }
    }
}
