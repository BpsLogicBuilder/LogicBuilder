﻿using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLoadAssemblyPathsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLoadAssemblyPathsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureLoadAssemblyPathsControl, AddAssemblyPathListBoxItemCommand>>
                (
                    provider =>
                    loadAssemblyPathsControl => new AddAssemblyPathListBoxItemCommand
                    (
                        provider.GetRequiredService<ILoadAssemblyPathsItemFactory>(),
                        loadAssemblyPathsControl
                    )
                )
                .AddTransient<IConfigureLoadAssemblyPathsCommandFactory, ConfigureLoadAssemblyPathsCommandFactory>()
                .AddTransient<Func<IConfigureLoadAssemblyPathsControl, UpdateAssemblyPathListBoxItemCommand>>
                (
                    provider =>
                    loadAssemblyPathsControl => new UpdateAssemblyPathListBoxItemCommand
                    (
                        provider.GetRequiredService<ILoadAssemblyPathsItemFactory>(),
                        loadAssemblyPathsControl
                    )
                );
        }
    }
}
