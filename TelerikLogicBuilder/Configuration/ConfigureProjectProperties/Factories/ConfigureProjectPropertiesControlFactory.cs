using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal class ConfigureProjectPropertiesControlFactory : IConfigureProjectPropertiesControlFactory
    {
        private readonly Func<IConfigureProjectPropertiesForm, IApplicationControl> _getApplicationControl;

        public ConfigureProjectPropertiesControlFactory(
            Func<IConfigureProjectPropertiesForm, IApplicationControl> getApplicationControl)
        {
            _getApplicationControl = getApplicationControl;
        }

        public IApplicationControl GetApplicationControl(IConfigureProjectPropertiesForm configureProjectProperties)
            => _getApplicationControl(configureProjectProperties);
    }
}
