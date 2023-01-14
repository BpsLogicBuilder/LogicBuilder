using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Factories
{
    internal class ParametersControlFactory : IParametersControlFactory
    {
        private readonly Func<IConfigurationForm, IConfigureGenericListParameterControl> _getConfigureGenericListParameterControl;
        private readonly Func<IConfigurationForm, IConfigureGenericParameterControl> _getConfigureGenericParameterControl;
        private readonly Func<IConfigurationForm, IConfigureLiteralListParameterControl> _getConfigureLiteralListParameterControl;
        private readonly Func<IConfigurationForm, IConfigureLiteralParameterControl> _getConfigureLiteralParameterControl;
        private readonly Func<IConfigurationForm, IConfigureObjectListParameterControl> _getConfigureObjectListParameterControl;
        private readonly Func<IConfigurationForm, IConfigureObjectParameterControl> _getConfigureObjectParameterControl;

        public ParametersControlFactory(
            Func<IConfigurationForm, IConfigureGenericListParameterControl> getConfigureGenericListParameterControl,
            Func<IConfigurationForm, IConfigureGenericParameterControl> getConfigureGenericParameterControl,
            Func<IConfigurationForm, IConfigureLiteralListParameterControl> getConfigureLiteralListParameterControl,
            Func<IConfigurationForm, IConfigureLiteralParameterControl> getConfigureLiteralParameterControl,
            Func<IConfigurationForm, IConfigureObjectListParameterControl> getConfigureObjectListParameterControl,
            Func<IConfigurationForm, IConfigureObjectParameterControl> getConfigureObjectParameterControl)
        {
            _getConfigureGenericListParameterControl = getConfigureGenericListParameterControl;
            _getConfigureGenericParameterControl = getConfigureGenericParameterControl;
            _getConfigureLiteralListParameterControl = getConfigureLiteralListParameterControl;
            _getConfigureLiteralParameterControl = getConfigureLiteralParameterControl;
            _getConfigureObjectListParameterControl = getConfigureObjectListParameterControl;
            _getConfigureObjectParameterControl = getConfigureObjectParameterControl;
        }

        public IConfigureGenericListParameterControl GetConfigureGenericListParameterControl(IConfigurationForm configurationForm)
            => _getConfigureGenericListParameterControl(configurationForm);

        public IConfigureGenericParameterControl GetConfigureGenericParameterControl(IConfigurationForm configurationForm)
            => _getConfigureGenericParameterControl(configurationForm);

        public IConfigureLiteralListParameterControl GetConfigureLiteralListParameterControl(IConfigurationForm configurationForm)
            => _getConfigureLiteralListParameterControl(configurationForm);

        public IConfigureLiteralParameterControl GetConfigureLiteralParameterControl(IConfigurationForm configurationForm)
            => _getConfigureLiteralParameterControl(configurationForm);

        public IConfigureObjectListParameterControl GetConfigureObjectListParameterControl(IConfigurationForm configurationForm)
            => _getConfigureObjectListParameterControl(configurationForm);

        public IConfigureObjectParameterControl GetConfigureObjectParameterControl(IConfigurationForm configurationForm)
            => _getConfigureObjectParameterControl(configurationForm);
    }
}
