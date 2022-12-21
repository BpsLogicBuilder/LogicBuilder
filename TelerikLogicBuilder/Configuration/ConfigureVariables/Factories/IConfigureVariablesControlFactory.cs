using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureVariablesFolder;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories
{
    internal interface IConfigureVariablesControlFactory
    {
        IConfigureLiteralListVariableControl GetConfigureLiteralListVariableControl(IConfigureVariablesForm configureVariablesForm);
        IConfigureLiteralVariableControl GetConfigureLiteralVariableControl(IConfigureVariablesForm configureVariablesForm);
        IConfigureObjectListVariableControl GetConfigureObjectListVariableControl(IConfigureVariablesForm configureVariablesForm);
        IConfigureObjectVariableControl GetConfigureObjectVariableControl(IConfigureVariablesForm configureVariablesForm);
        IConfigureVariablesFolderControl GetConfigureVariablesFolderControl(IConfigureVariablesForm configureVariablesForm);
    }
}
