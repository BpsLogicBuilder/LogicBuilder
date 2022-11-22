using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConnectorObjectsItemFactoryServices
    {
        internal static IServiceCollection AddConnectorObjectsItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConnectorObjectsItemFactory, ConnectorObjectsItemFactory>()
                .AddTransient<Func<string, ConnectorObjectListBoxItem>>
                (
                    provider =>
                    text => new ConnectorObjectListBoxItem
                    (
                        text
                    )
                );
        }
    }
}
