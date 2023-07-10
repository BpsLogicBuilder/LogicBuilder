using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDecisionFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditDecisionFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditDecisionFormCommandFactory, EditDecisionFormCommandFactory>();
        }
    }
}
