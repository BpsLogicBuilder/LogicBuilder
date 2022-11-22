using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories
{
    internal interface IConfigureExcludedModulesCommandFactory
    {
        UpdateExcludedModulesCommand GetUpdateExcludedModulesCommand(IConfigureExcludedModules configureExcludedModules);
    }
}
