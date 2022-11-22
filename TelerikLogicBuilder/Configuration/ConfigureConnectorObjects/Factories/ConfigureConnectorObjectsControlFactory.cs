using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories
{
    internal class ConfigureConnectorObjectsControlFactory : IConfigureConnectorObjectsControlFactory
    {
        private readonly Func<IConfigureConnectorObjectsForm, IConfigureConnectorObjectsControl> _getConfigureConnectorObjectsControl;

        public ConfigureConnectorObjectsControlFactory(
            Func<IConfigureConnectorObjectsForm, IConfigureConnectorObjectsControl> getConfigureConnectorObjectsControl)
        {
            _getConfigureConnectorObjectsControl = getConfigureConnectorObjectsControl;
        }

        public IConfigureConnectorObjectsControl GetConfigureConnectorObjectsControl(IConfigureConnectorObjectsForm configureConnectorObjectsForm)
            => _getConfigureConnectorObjectsControl(configureConnectorObjectsForm);
    }
}
