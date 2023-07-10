using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseConstructorFactoryServices
    {
        internal static IServiceCollection AddIntellisenseConstructorFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IChildConstructorFinderFactory, ChildConstructorFinderFactory>()
                .AddSingleton<IConstructorFactory, ConstructorFactory>();
        }
    }
}
