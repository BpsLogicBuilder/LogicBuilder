using ABIS.LogicBuilder.FlowBuilder.Components;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ComponentsServices
    {
        internal static IServiceCollection AddComponents(this IServiceCollection services)
        {
            return services
                .AddTransient<RichInputBox>();
        }
    }
}
