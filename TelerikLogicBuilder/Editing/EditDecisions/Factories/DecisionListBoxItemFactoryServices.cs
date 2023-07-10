using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DecisionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddDecisionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDecisionListBoxItemFactory, DecisionListBoxItemFactory>();
        }
    }
}
