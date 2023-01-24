using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectReturnType;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.Factories
{
    internal class ConfigureReturnTypeControlFactory : IConfigureReturnTypeControlFactory
    {
        private readonly Func<IConfigureReturnTypeForm, IConfigureGenericListReturnTypeControl> _getConfigureGenericListReturnTypeControl;
        private readonly Func<IConfigureReturnTypeForm, IConfigureGenericReturnTypeControl> _getConfigureGenericReturnTypeControl;
        private readonly Func<IConfigureReturnTypeForm, IConfigureLiteralListReturnTypeControl> _getConfigureLiteralListReturnTypeControl;
        private readonly Func<IConfigureReturnTypeForm, IConfigureLiteralReturnTypeControl> _getConfigureLiteralReturnTypeControl;
        private readonly Func<IConfigureReturnTypeForm, IConfigureObjectListReturnTypeControl> _getConfigureObjectListReturnTypeControl;
        private readonly Func<IConfigureReturnTypeForm, IConfigureObjectReturnTypeControl> _getConfigureObjectReturnTypeControl;

        public ConfigureReturnTypeControlFactory(
            Func<IConfigureReturnTypeForm, IConfigureGenericListReturnTypeControl> getConfigureGenericListReturnTypeControl,
            Func<IConfigureReturnTypeForm, IConfigureGenericReturnTypeControl> getConfigureGenericReturnTypeControl,
            Func<IConfigureReturnTypeForm, IConfigureLiteralListReturnTypeControl> getConfigureLiteralListReturnTypeControl,
            Func<IConfigureReturnTypeForm, IConfigureLiteralReturnTypeControl> getConfigureLiteralReturnTypeControl,
            Func<IConfigureReturnTypeForm, IConfigureObjectListReturnTypeControl> getConfigureObjectListReturnTypeControl,
            Func<IConfigureReturnTypeForm, IConfigureObjectReturnTypeControl> getConfigureObjectReturnTypeControl)
        {
            _getConfigureGenericListReturnTypeControl = getConfigureGenericListReturnTypeControl;
            _getConfigureGenericReturnTypeControl = getConfigureGenericReturnTypeControl;
            _getConfigureLiteralListReturnTypeControl = getConfigureLiteralListReturnTypeControl;
            _getConfigureLiteralReturnTypeControl = getConfigureLiteralReturnTypeControl;
            _getConfigureObjectListReturnTypeControl = getConfigureObjectListReturnTypeControl;
            _getConfigureObjectReturnTypeControl = getConfigureObjectReturnTypeControl;
        }

        public IConfigureGenericListReturnTypeControl GetConfigureGenericListReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm)
            => _getConfigureGenericListReturnTypeControl(configureReturnTypeForm);

        public IConfigureGenericReturnTypeControl GetConfigureGenericReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm)
            => _getConfigureGenericReturnTypeControl(configureReturnTypeForm);

        public IConfigureLiteralListReturnTypeControl GetConfigureLiteralListReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm)
            => _getConfigureLiteralListReturnTypeControl(configureReturnTypeForm);

        public IConfigureLiteralReturnTypeControl GetConfigureLiteralReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm)
            => _getConfigureLiteralReturnTypeControl(configureReturnTypeForm);

        public IConfigureObjectListReturnTypeControl GetConfigureObjectListReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm)
            => _getConfigureObjectListReturnTypeControl(configureReturnTypeForm);

        public IConfigureObjectReturnTypeControl GetConfigureObjectReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm)
            => _getConfigureObjectReturnTypeControl(configureReturnTypeForm);
    }
}
