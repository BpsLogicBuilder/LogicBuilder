namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal class ConnectorObjectsItemFactory : IConnectorObjectsItemFactory
    {
        public ConnectorObjectListBoxItem GetConnectorObjectListBoxItem(string text)
            => new
            (
                text
            );
    }
}
