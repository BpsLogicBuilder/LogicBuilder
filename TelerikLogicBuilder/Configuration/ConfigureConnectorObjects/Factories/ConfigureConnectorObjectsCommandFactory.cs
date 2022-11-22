using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal class ConfigureConnectorObjectsCommandFactory : IConfigureConnectorObjectsCommandFactory
    {
        private readonly Func<IConfigureConnectorObjectsControl, AddConnectorObjectListBoxItemCommand> _getAddConnectorObjectListBoxItemCommand;
        private readonly Func<IConfigureConnectorObjectsControl, UpdateConnectorObjectListBoxItemCommand> _getUpdateConnectorObjectListBoxItemCommand;

        public ConfigureConnectorObjectsCommandFactory(
            Func<IConfigureConnectorObjectsControl, AddConnectorObjectListBoxItemCommand> getAddConnectorObjectListBoxItemCommand,
            Func<IConfigureConnectorObjectsControl, UpdateConnectorObjectListBoxItemCommand> getUpdateConnectorObjectListBoxItemCommand)
        {
            _getAddConnectorObjectListBoxItemCommand = getAddConnectorObjectListBoxItemCommand;
            _getUpdateConnectorObjectListBoxItemCommand = getUpdateConnectorObjectListBoxItemCommand;
        }

        public AddConnectorObjectListBoxItemCommand GetAddConnectorObjectListBoxItemCommand(IConfigureConnectorObjectsControl configureConnectorObjectsControl)
            => _getAddConnectorObjectListBoxItemCommand(configureConnectorObjectsControl);

        public UpdateConnectorObjectListBoxItemCommand GetUpdateConnectorObjectListBoxItemCommand(IConfigureConnectorObjectsControl configureConnectorObjectsControl)
            => _getUpdateConnectorObjectListBoxItemCommand(configureConnectorObjectsControl);
    }
}
