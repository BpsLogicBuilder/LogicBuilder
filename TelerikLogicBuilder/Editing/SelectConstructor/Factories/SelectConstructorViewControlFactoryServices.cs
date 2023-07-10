using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SelectConstructorViewControlFactoryServices
    {
        internal static IServiceCollection AddSelectConstructorViewControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISelectConstructorViewControlFactory, SelectConstructorViewControlFactory>();
        }
    }
}
