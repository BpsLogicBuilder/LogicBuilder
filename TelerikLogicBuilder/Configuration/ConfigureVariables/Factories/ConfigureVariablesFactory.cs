using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories
{
    internal class ConfigureVariablesFactory : IConfigureVariablesFactory
    {
        private readonly Func<IConfigureVariablesForm, IConfigureVariablesDragDropHandler> _getConfigureVariablesDragDropHandler;
        private readonly Func<IConfigureVariablesForm, ConfigureVariablesTreeView> _getConfigureVariablesTreeView;

        public ConfigureVariablesFactory(
            Func<IConfigureVariablesForm, IConfigureVariablesDragDropHandler> getConfigureVariablesDragDropHandler,
            Func<IConfigureVariablesForm, ConfigureVariablesTreeView> getConfigureVariablesTreeView)
        {
            _getConfigureVariablesDragDropHandler = getConfigureVariablesDragDropHandler;
            _getConfigureVariablesTreeView = getConfigureVariablesTreeView;
        }

        public IConfigureVariablesDragDropHandler GetConfigureVariablesDragDropHandler(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesDragDropHandler(configureVariablesForm);

        public ConfigureVariablesTreeView GetConfigureVariablesTreeView(IConfigureVariablesForm configureVariablesForm)
            => _getConfigureVariablesTreeView(configureVariablesForm);
    }
}
