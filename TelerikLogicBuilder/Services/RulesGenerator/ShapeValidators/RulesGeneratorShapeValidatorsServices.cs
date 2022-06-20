using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesGeneratorShapeValidatorsServices
    {
        internal static IServiceCollection AddRulesGeneratorShapeValidators(this IServiceCollection services)
            => services
                .AddSingleton<IActionShapeValidator, ActionShapeValidator>()
                .AddSingleton<IApplicationConnectorValidator, ApplicationConnectorValidator>()
                .AddSingleton<IApplicationSpecificFlowShapeValidator, ApplicationSpecificFlowShapeValidator>()
                .AddSingleton<IBeginShapeValidator, BeginShapeValidator>()
                .AddSingleton<ICommentShapeValidator, CommentShapeValidator>()
                .AddSingleton<IConditionShapeValidator, ConditionShapeValidator>()
                .AddSingleton<IDecisionShapeValidator, DecisionShapeValidator>()
                .AddSingleton<IDialogShapeValidator, DialogShapeValidator>()
                .AddSingleton<IEndShapeValidator, EndShapeValidator>()
                .AddSingleton<IJumpShapeValidator, JumpShapeValidator>()
                .AddSingleton<IMergeShapeValidator, MergeShapeValidator>()
                .AddSingleton<IModuleShapeValidator, ModuleShapeValidator>()
                .AddSingleton<IRegularConnectorValidator, RegularConnectorValidator>()
                .AddSingleton<IWaitConditionShapeValidator, WaitConditionShapeValidator>()
                .AddSingleton<IWaitDecisionShapeValidator, WaitDecisionShapeValidator>()
                .AddSingleton<IShapeValidator, ShapeValidator>();
    }
}
