using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConnectorObjectsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureConnectorObjectsControlFactories(this IServiceCollection services)
        {
            return services
                 .AddSingleton<IConfigureConnectorObjectsControlFactory, ConfigureConnectorObjectsControlFactory>();
        }
    }
}
