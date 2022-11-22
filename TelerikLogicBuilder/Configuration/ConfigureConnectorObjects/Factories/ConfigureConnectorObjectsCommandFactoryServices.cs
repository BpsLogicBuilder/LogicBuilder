using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConnectorObjectsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureConnectorObjectsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConnectorObjectsControl, AddConnectorObjectListBoxItemCommand>>
                (
                    provider =>
                    configureConnectorObjectsControl => new AddConnectorObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        configureConnectorObjectsControl
                    )
                )
                .AddTransient<IConfigureConnectorObjectsCommandFactory, ConfigureConnectorObjectsCommandFactory>()
                .AddTransient<Func<IConfigureConnectorObjectsControl, UpdateConnectorObjectListBoxItemCommand>>
                (
                    provider =>
                    configureConnectorObjectsControl => new UpdateConnectorObjectListBoxItemCommand
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        configureConnectorObjectsControl
                    )
                );
        }
    }
}
