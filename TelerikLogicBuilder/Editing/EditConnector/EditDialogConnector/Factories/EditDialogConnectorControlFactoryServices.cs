using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Xml;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDialogConnectorControlFactoryServices
    {
        internal static IServiceCollection AddEditDialogConnectorControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditDialogConnectorForm, short, XmlDocument?, IEditDialogConnectorControl>>
                (
                    provider =>
                    (editDialogConnectorForm, connectorIndexToSelect, connectorXmlDocument) => new EditDialogConnectorControl
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConnectorDataParser>(),
                        provider.GetRequiredService<IConstructorTypeHelper>(),
                        provider.GetRequiredService<IEditDialogConnectorFieldControlFactory>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IMetaObjectDataParser>(),
                        provider.GetRequiredService<IMetaObjectElementValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        editDialogConnectorForm, 
                        connectorIndexToSelect, 
                        connectorXmlDocument
                    )
                )
                .AddTransient<IEditDialogConnectorControlFactory, EditDialogConnectorControlFactory>();
        }
    }
}
