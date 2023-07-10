using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConditionFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditConditionFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditConditionFunctionCommandFactory, EditConditionFunctionCommandFactory>();
        }
    }
}
