using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories
{
    internal class EditDialogConnectorControlFactory : IEditDialogConnectorControlFactory
    {
        public IEditDialogConnectorControl GetEditDialogConnectorControl(IEditDialogConnectorForm editDialogConnectorForm, short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
            => new EditDialogConnectorControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConnectorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IConstructorTypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IEditDialogConnectorFieldControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IMetaObjectDataParser>(),
                Program.ServiceProvider.GetRequiredService<IMetaObjectElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDialogConnectorForm,
                connectorIndexToSelect,
                connectorXmlDocument
            );
    }
}
