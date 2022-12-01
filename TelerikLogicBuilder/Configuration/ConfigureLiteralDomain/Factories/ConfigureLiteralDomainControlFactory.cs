using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories
{
    internal class ConfigureLiteralDomainControlFactory : IConfigureLiteralDomainControlFactory
    {
        private readonly Func<IConfigureLiteralDomainForm, IConfigureLiteralDomainControl> _getConfigureLiteralDomainControl;

        public ConfigureLiteralDomainControlFactory(
            Func<IConfigureLiteralDomainForm, IConfigureLiteralDomainControl> getConfigureLiteralDomainControl)
        {
            _getConfigureLiteralDomainControl = getConfigureLiteralDomainControl;
        }

        public IConfigureLiteralDomainControl GetConfigureLiteralDomainControl(IConfigureLiteralDomainForm configureLiteralDomainForm)
            => _getConfigureLiteralDomainControl(configureLiteralDomainForm);
    }
}
