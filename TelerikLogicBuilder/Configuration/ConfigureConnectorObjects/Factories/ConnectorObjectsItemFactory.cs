using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal class ConnectorObjectsItemFactory : IConnectorObjectsItemFactory
    {
        private readonly Func<string, ConnectorObjectListBoxItem> _getConnectorObjectListBoxItem;

        public ConnectorObjectsItemFactory(
            Func<string, ConnectorObjectListBoxItem> getConnectorObjectListBoxItem)
        {
            _getConnectorObjectListBoxItem = getConnectorObjectListBoxItem;
        }

        public ConnectorObjectListBoxItem GetConnectorObjectListBoxItem(string text)
            => _getConnectorObjectListBoxItem(text);
    }
}
