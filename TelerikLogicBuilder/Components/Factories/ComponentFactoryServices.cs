using ABIS.LogicBuilder.FlowBuilder.Components.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ComponentFactoryServices
    {
        internal static IServiceCollection AddComponentFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IComponentFactory, ComponentFactory>();
        }
    }
}
