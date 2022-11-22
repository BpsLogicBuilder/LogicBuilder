using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal interface IConfigureConnectorObjectsCommandFactory
    {
        AddConnectorObjectListBoxItemCommand GetAddConnectorObjectListBoxItemCommand(IConfigureConnectorObjectsControl configureConnectorObjectsControl);
        UpdateConnectorObjectListBoxItemCommand GetUpdateConnectorObjectListBoxItemCommand(IConfigureConnectorObjectsControl configureConnectorObjectsControl);
    }
}
