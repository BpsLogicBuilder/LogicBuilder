using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingControlFactoryServices
    {
        internal static IServiceCollection AddEditingControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditingControlFactory, EditingControlFactory>();
        }
    }
}
