using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationControlFactory : IConfigurationControlFactory
    {
        private readonly Func<IConfigureProjectProperties, IApplicationControl> _getApplicationControl;
        private readonly Func<IConfigureConnectorObjectsForm, IConfigureConnectorObjectsControl> _getConfigureConnectorObjectsControl;

        public ConfigurationControlFactory(
            Func<IConfigureProjectProperties, IApplicationControl> getApplicationControl,
            Func<IConfigureConnectorObjectsForm,IConfigureConnectorObjectsControl> getConfigureConnectorObjectsControl)
        {
            _getApplicationControl = getApplicationControl;
            _getConfigureConnectorObjectsControl = getConfigureConnectorObjectsControl;
        }

        public IApplicationControl GetApplicationControl(IConfigureProjectProperties configureProjectProperties)
            => _getApplicationControl(configureProjectProperties);

        public IConfigureConnectorObjectsControl GetConfigureConnectorObjectsControl(IConfigureConnectorObjectsForm configureConnectorObjectsForm)
            => _getConfigureConnectorObjectsControl(configureConnectorObjectsForm);
    }
}
