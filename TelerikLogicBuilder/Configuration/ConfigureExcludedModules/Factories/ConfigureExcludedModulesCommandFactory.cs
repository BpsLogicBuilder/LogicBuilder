using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories
{
    internal class ConfigureExcludedModulesCommandFactory : IConfigureExcludedModulesCommandFactory
    {
        private readonly Func<IConfigureExcludedModulesForm, UpdateExcludedModulesCommand> _getUpdateExcludedModulesCommand;

        public ConfigureExcludedModulesCommandFactory(
            Func<IConfigureExcludedModulesForm, UpdateExcludedModulesCommand> getUpdateExcludedModulesCommand)
        {
            _getUpdateExcludedModulesCommand = getUpdateExcludedModulesCommand;
        }

        public UpdateExcludedModulesCommand GetUpdateExcludedModulesCommand(IConfigureExcludedModulesForm configureExcludedModules)
            => _getUpdateExcludedModulesCommand(configureExcludedModules);
    }
}
