using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories
{
    internal class ConfigureFunctionsFactory : IConfigureFunctionsFactory
    {
        private readonly Func<IConfigureFunctionsForm, IConfigureFunctionsDragDropHandler> _getConfigureFunctionsDragDropHandler;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsTreeView> _getConfigureFunctionsTreeView;

        public ConfigureFunctionsFactory(
            Func<IConfigureFunctionsForm, IConfigureFunctionsDragDropHandler> getConfigureFunctionsDragDropHandler,
            Func<IConfigureFunctionsForm, ConfigureFunctionsTreeView> getConfigureFunctionsTreeView)
        {
            _getConfigureFunctionsDragDropHandler = getConfigureFunctionsDragDropHandler;
            _getConfigureFunctionsTreeView = getConfigureFunctionsTreeView;
        }

        public IConfigureFunctionsDragDropHandler GetConfigureFunctionsDragDropHandler(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsDragDropHandler(configureFunctionsForm);

        public ConfigureFunctionsTreeView GetConfigureFunctionsTreeView(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsTreeView(configureFunctionsForm);
    }
}
