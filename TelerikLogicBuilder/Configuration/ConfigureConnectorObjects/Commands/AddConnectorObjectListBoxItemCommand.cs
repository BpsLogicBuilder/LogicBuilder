﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Commands
{
    internal class AddConnectorObjectListBoxItemCommand : ClickCommandBase
    {
        private readonly IConfigurationItemFactory _configurationItemFactory;

        private readonly AutoCompleteRadDropDownList txtType;
        private readonly IRadListBoxManager<ConnectorObjectListBoxItem> radListBoxManager;

        public AddConnectorObjectListBoxItemCommand(
            IConfigurationItemFactory configurationItemFactory,
            IConfigureConnectorObjectsControl configureConnectorObjectsControl)
        {
            _configurationItemFactory = configurationItemFactory;
            txtType = configureConnectorObjectsControl.TxtType;
            radListBoxManager= configureConnectorObjectsControl.RadListBoxManager;
        }

        public override void Execute()
        {
            radListBoxManager.Add
            (
                _configurationItemFactory.GetConnectorObjectListBoxItem(txtType.Text.Trim())
            );
        }
    }
}
