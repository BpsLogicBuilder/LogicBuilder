using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories
{
    internal interface IEditDialogConnectorControlFactory
    {
        IEditDialogConnectorControl GetEditDialogConnectorControl(
            IEditDialogConnectorForm editDialogConnectorForm,
            short connectorIndexToSelect,
            XmlDocument? connectorXmlDocument);
    }
}
