using ABIS.LogicBuilder.FlowBuilder.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FactoryServices
    {
        internal static IServiceCollection AddFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IServiceFactory, ServiceFactory>();
        }
    }
}
