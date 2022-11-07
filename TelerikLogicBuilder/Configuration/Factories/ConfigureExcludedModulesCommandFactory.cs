using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
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
