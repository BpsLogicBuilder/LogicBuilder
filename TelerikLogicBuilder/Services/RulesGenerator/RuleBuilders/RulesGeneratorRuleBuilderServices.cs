using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesGeneratorRuleBuilderServices
    {
        internal static IServiceCollection AddRulesGeneratorRuleBuilders(this IServiceCollection services)
            => services
                .AddSingleton<ILongStringManager, LongStringManager>();
    }
}
