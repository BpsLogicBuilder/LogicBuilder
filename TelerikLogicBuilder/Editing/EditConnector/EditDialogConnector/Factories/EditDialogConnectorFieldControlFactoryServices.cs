using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDialogConnectorFieldControlFactoryServices
    {
        internal static IServiceCollection AddEditDialogConnectorFieldControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditDialogConnectorFieldControlFactory, EditDialogConnectorFieldControlFactory>();
        }
    }
}
