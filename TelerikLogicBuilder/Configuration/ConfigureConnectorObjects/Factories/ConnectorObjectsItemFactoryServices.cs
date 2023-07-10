using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConnectorObjectsItemFactoryServices
    {
        internal static IServiceCollection AddConnectorObjectsItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConnectorObjectsItemFactory, ConnectorObjectsItemFactory>();
        }
    }
}
