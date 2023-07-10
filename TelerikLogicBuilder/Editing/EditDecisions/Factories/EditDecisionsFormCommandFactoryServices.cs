using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDecisionsFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditDecisionsFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditDecisionsFormCommandFactory, EditDecisionsFormCommandFactory>();
        }
    }
}
