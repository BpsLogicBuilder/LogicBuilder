using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectFragmentViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectFragmentViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISelectFragmentViewControlFactory, SelectFragmentViewControlFactory>();
        }
    }
}
