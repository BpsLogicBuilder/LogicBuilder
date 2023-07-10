using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfiguredItemControlFactoryServices
    {
        internal static IServiceCollection AddSelectEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConfiguredItemControlFactory, ConfiguredItemControlFactory>();
        }
    }
}
