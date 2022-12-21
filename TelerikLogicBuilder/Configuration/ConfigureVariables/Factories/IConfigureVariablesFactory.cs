using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories
{
    internal interface IConfigureVariablesFactory
    {
        IConfigureVariablesDragDropHandler GetConfigureVariablesDragDropHandler(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesTreeView GetConfigureVariablesTreeView(IConfigureVariablesForm configureVariablesForm);
    }
}
