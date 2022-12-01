using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories
{
    internal class ConfigureLiteralListDefaultValueControlFactory : IConfigureLiteralListDefaultValueControlFactory
    {
        private readonly Func<IConfigureLiteralListDefaultValueForm, IConfigureLiteralListDefaultValueControl> _getConfigureLiteralListDefaultValueControl;

        public ConfigureLiteralListDefaultValueControlFactory(Func<IConfigureLiteralListDefaultValueForm, IConfigureLiteralListDefaultValueControl> getConfigureLiteralListDefaultValueControl)
        {
            _getConfigureLiteralListDefaultValueControl = getConfigureLiteralListDefaultValueControl;
        }

        public IConfigureLiteralListDefaultValueControl GetConfigureLiteralListDefaultValueControl(IConfigureLiteralListDefaultValueForm configureLiteralListDefaultValueForm)
            => _getConfigureLiteralListDefaultValueControl(configureLiteralListDefaultValueForm);
    }
}
