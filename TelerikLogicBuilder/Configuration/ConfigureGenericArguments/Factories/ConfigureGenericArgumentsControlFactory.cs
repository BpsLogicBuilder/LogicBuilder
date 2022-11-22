using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericObjectArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericObjectListArgument;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories
{
    internal class ConfigureGenericArgumentsControlFactory : IConfigureGenericArgumentsControlFactory
    {
        private readonly Func<IConfigureGenericArgumentsForm, ConfigureGenericLiteralArgumentControl> _getConfigureGenericLiteralArgumentControl;
        private readonly Func<IConfigureGenericArgumentsForm, ConfigureGenericLiteralListArgumentControl> _getConfigureGenericLiteralListArgumentControl;
        private readonly Func<IConfigureGenericArgumentsForm, ConfigureGenericObjectArgumentControl> _getConfigureGenericObjectArgumentControl;
        private readonly Func<IConfigureGenericArgumentsForm, ConfigureGenericObjectListArgumentControl> _getConfigureGenericObjectListArgumentControl;

        public ConfigureGenericArgumentsControlFactory(
            Func<IConfigureGenericArgumentsForm, ConfigureGenericLiteralArgumentControl> getConfigureGenericLiteralArgumentControl,
            Func<IConfigureGenericArgumentsForm, ConfigureGenericLiteralListArgumentControl> getConfigureGenericLiteralListArgumentControl,
            Func<IConfigureGenericArgumentsForm, ConfigureGenericObjectArgumentControl> getConfigureGenericObjectArgumentControl,
            Func<IConfigureGenericArgumentsForm, ConfigureGenericObjectListArgumentControl> getConfigureGenericObjectListArgumentControl)
        {
            _getConfigureGenericLiteralArgumentControl = getConfigureGenericLiteralArgumentControl;
            _getConfigureGenericLiteralListArgumentControl = getConfigureGenericLiteralListArgumentControl;
            _getConfigureGenericObjectArgumentControl = getConfigureGenericObjectArgumentControl;
            _getConfigureGenericObjectListArgumentControl = getConfigureGenericObjectListArgumentControl;
        }

        public ConfigureGenericLiteralArgumentControl GetConfigureGenericLiteralArgumentControl(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getConfigureGenericLiteralArgumentControl(configureGenericArgumentsForm);

        public ConfigureGenericLiteralListArgumentControl GetConfigureGenericLiteralListArgumentControl(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getConfigureGenericLiteralListArgumentControl(configureGenericArgumentsForm);

        public ConfigureGenericObjectArgumentControl GetConfigureGenericObjectArgumentControl(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getConfigureGenericObjectArgumentControl(configureGenericArgumentsForm);

        public ConfigureGenericObjectListArgumentControl GetConfigureGenericObjectListArgumentControl(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getConfigureGenericObjectListArgumentControl(configureGenericArgumentsForm);
    }
}
