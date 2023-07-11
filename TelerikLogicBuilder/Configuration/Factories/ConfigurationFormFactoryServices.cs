using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationFormFactoryServices
    {
        internal static IServiceCollection AddConfigurationFormFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfigurationFormFactory, ConfigurationFormFactory>();
        }
    }
}
