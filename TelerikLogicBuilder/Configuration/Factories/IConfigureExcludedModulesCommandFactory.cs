using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigureExcludedModulesCommandFactory
    {
        UpdateExcludedModulesCommand GetUpdateExcludedModulesCommand(IConfigureExcludedModules configureExcludedModules);
    }
}
