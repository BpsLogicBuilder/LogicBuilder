using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Commands
{
    internal class AddConnectorObjectListBoxItemCommand : ClickCommandBase
    {
        private readonly IConnectorObjectsItemFactory _connectorObjectsItemFactory;

        private readonly AutoCompleteRadDropDownList txtType;
        private readonly IRadListBoxManager<ConnectorObjectListBoxItem> radListBoxManager;

        public AddConnectorObjectListBoxItemCommand(
            IConnectorObjectsItemFactory connectorObjectsItemFactory,
            IConfigureConnectorObjectsControl configureConnectorObjectsControl)
        {
            _connectorObjectsItemFactory = connectorObjectsItemFactory;
            txtType = configureConnectorObjectsControl.TxtType;
            radListBoxManager= configureConnectorObjectsControl.RadListBoxManager;
        }

        public override void Execute()
        {
            radListBoxManager.Add
            (
                _connectorObjectsItemFactory.GetConnectorObjectListBoxItem(txtType.Text.Trim())
            );
        }
    }
}
