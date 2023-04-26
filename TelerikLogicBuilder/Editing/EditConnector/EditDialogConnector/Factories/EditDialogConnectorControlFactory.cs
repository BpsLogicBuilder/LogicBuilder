using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories
{
    internal class EditDialogConnectorControlFactory : IEditDialogConnectorControlFactory
    {
        private readonly Func<IEditDialogConnectorForm, short, XmlDocument?, IEditDialogConnectorControl> _getEditDialogConnectorControl;

        public EditDialogConnectorControlFactory(
            Func<IEditDialogConnectorForm, short, XmlDocument?, IEditDialogConnectorControl> getEditDialogConnectorControl)
        {
            _getEditDialogConnectorControl = getEditDialogConnectorControl;
        }

        public IEditDialogConnectorControl GetEditDialogConnectorControl(IEditDialogConnectorForm editDialogConnectorForm, short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
            => _getEditDialogConnectorControl(editDialogConnectorForm, connectorIndexToSelect, connectorXmlDocument);
    }
}
