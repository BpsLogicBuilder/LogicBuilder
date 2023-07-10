using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectFunctionViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectFunctionViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISelectFunctionViewControlFactory, SelectFunctionViewControlFactory>();
        }
    }
}
