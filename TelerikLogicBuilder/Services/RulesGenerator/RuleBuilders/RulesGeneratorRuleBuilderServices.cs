using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesGeneratorRuleBuilderServices
    {
        internal static IServiceCollection AddRulesGeneratorRuleBuilders(this IServiceCollection services) 
            => services
                .AddSingleton<IBeginFlowRuleBuilder, BeginFlowRuleBuilder>()
                .AddSingleton<IConditionsRuleBuilder, ConditionsRuleBuilder>()
                .AddSingleton<IDecisionsRuleBuilder, DecisionsRuleBuilder>()
                .AddSingleton<IDialogWithExitsRuleBuilder, DialogWithExitsRuleBuilder>()
                .AddSingleton<IDialogWithoutExitsRuleBuilder, DialogWithoutExitsRuleBuilder>()
                .AddSingleton<IMergeRuleBuilder, MergeRuleBuilder>()
                .AddSingleton<IModuleBeginRuleBuilder, ModuleBeginRuleBuilder>()
                .AddSingleton<IModuleRuleBuilder, ModuleRuleBuilder>()
                .AddSingleton<IShapeSetRuleBuilder, ShapeSetRuleBuilder>()
                .AddSingleton<ITableRowRuleBuilder, TableRowRuleBuilder>()
                .AddSingleton<IWaitConditionsRuleBuilder, WaitConditionsRuleBuilder>()
                .AddSingleton<IWaitDecisionsRuleBuilder, WaitDecisionsRuleBuilder>()
                .AddSingleton<IDiagramResourcesManager, DiagramResourcesManager>()
                .AddSingleton<ILongStringManager, LongStringManager>()
                .AddSingleton<ITableResourcesManager, TableResourcesManager>();
    }
}
