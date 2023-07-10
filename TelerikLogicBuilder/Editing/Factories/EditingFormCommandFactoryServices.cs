using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditingFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditingFormCommandFactory, EditingFormCommandFactory>();
        }
    }
}
