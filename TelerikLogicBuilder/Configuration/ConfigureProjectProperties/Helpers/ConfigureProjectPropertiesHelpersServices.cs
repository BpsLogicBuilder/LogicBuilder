using Microsoft.Extensions.DependencyInjection;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Helpers
{
    internal static class ConfigureProjectPropertiesHelpersServices
    {
        internal static IServiceCollection AddConfigureProjectPropertiesHelpers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IGetNextApplicationNumber, GetNextApplicationNumber>();
        }
    }
}
