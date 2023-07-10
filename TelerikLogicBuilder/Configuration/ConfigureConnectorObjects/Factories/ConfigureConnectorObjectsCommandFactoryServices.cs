using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConnectorObjectsCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureConnectorObjectsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfigureConnectorObjectsCommandFactory, ConfigureConnectorObjectsCommandFactory>();
        }
    }
}
