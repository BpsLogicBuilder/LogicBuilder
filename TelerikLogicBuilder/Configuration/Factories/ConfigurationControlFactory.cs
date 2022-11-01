using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationControlFactory : IConfigurationControlFactory
    {
        private readonly Func<IConfigureProjectProperties, IApplicationControl> _getApplicationControl;

        public ConfigurationControlFactory(Func<IConfigureProjectProperties, IApplicationControl> getApplicationControl)
        {
            _getApplicationControl = getApplicationControl;
        }

        public IApplicationControl GetApplicationControl(IConfigureProjectProperties configureProjectProperties)
            => _getApplicationControl(configureProjectProperties);
    }
}
