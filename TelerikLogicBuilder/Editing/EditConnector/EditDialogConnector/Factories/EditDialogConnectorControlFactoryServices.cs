using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDialogConnectorControlFactoryServices
    {
        internal static IServiceCollection AddEditDialogConnectorControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditDialogConnectorControlFactory, EditDialogConnectorControlFactory>();
        }
    }
}
