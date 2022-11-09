using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal static class LoadAssemblyPathsCommandFactoryServices
    {
        internal static IServiceCollection AddLoadAssemblyPathsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<ILoadAssemblyPathsControl, AddAssemblyPathListBoxItemCommand>>
                (
                    provider =>
                    loadAssemblyPathsControl => new AddAssemblyPathListBoxItemCommand
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        loadAssemblyPathsControl
                    )
                )
                .AddTransient<ILoadAssemblyPathsCommandFactory, LoadAssemblyPathsCommandFactory>()
                .AddTransient<Func<ILoadAssemblyPathsControl, UpdateAssemblyPathListBoxItemCommand>>
                (
                    provider =>
                    loadAssemblyPathsControl => new UpdateAssemblyPathListBoxItemCommand
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        loadAssemblyPathsControl
                    )
                );
        }
    }
}
