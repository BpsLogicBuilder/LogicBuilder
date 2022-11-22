using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories
{
    internal class ConfigureExcludedModulesCommandFactory : IConfigureExcludedModulesCommandFactory
    {
        private readonly Func<IConfigureExcludedModules, UpdateExcludedModulesCommand> _getUpdateExcludedModulesCommand;

        public ConfigureExcludedModulesCommandFactory(
            Func<IConfigureExcludedModules, UpdateExcludedModulesCommand> getUpdateExcludedModulesCommand)
        {
            _getUpdateExcludedModulesCommand = getUpdateExcludedModulesCommand;
        }

        public UpdateExcludedModulesCommand GetUpdateExcludedModulesCommand(IConfigureExcludedModules configureExcludedModules)
            => _getUpdateExcludedModulesCommand(configureExcludedModules);
    }
}
