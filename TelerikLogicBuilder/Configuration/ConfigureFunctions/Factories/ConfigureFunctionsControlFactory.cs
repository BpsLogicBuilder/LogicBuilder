using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunctionsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunctionsRootNode;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories
{
    internal class ConfigureFunctionsControlFactory : IConfigureFunctionsControlFactory
    {
        private readonly Func<IConfigureFunctionsForm, IConfigureFunctionControl> _getConfigureFunctionControl;
        private readonly Func<IConfigureFunctionsForm, IConfigureFunctionsFolderControl> _getConfigureFunctionsFolderControl;

        public ConfigureFunctionsControlFactory(Func<IConfigureFunctionsForm, IConfigureFunctionControl> getConfigureFunctionControl, Func<IConfigureFunctionsForm, IConfigureFunctionsFolderControl> getConfigureFunctionsFolderControl)
        {
            _getConfigureFunctionControl = getConfigureFunctionControl;
            _getConfigureFunctionsFolderControl = getConfigureFunctionsFolderControl;
        }

        public IConfigureFunctionControl GetConfigureFunctionControl(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionControl(configureFunctionsForm);

        public IConfigureFunctionsFolderControl GetConfigureFunctionsFolderControl(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsFolderControl(configureFunctionsForm);

        public IConfigureFunctionsRootNodeControl GetConfigureFunctionsRootNodeControl()
            => new ConfigureFunctionsRootNodeControl();
    }
}
