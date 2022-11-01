using ABIS.LogicBuilder.FlowBuilder.Configuration.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationHelpersServices
    {
        internal static IServiceCollection AddConfigurationHelpers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IGetNextApplicationNumber, GetNextApplicationNumber>();
        }
    }
}
