using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormFactoryServices
    {
        internal static IServiceCollection AddEditingFormFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditingFormFactory, EditingFormFactory>();
        }
    }
}
