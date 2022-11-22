using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths;
using System;

namespace Microsoft.Extensions.DependencyInjection
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
