using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConditionFunctionListBoxItemFactoryServices
    {
        internal static IServiceCollection AddConditionFunctionListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IConditionFunctionListBoxItemFactory, ConditionFunctionListBoxItemFactory>();
        }
    }
}
