using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConditionFunctionsFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditConditionFunctionsFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditConditionFunctionsFormCommandFactory, EditConditionFunctionsFormCommandFactory>();
        }
    }
}
