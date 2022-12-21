using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Factories
{
    internal interface IConfigureLiteralListVariableCommandFactory
    {
        UpdateLiteralListVariableDefaultValueCommand GetUpdateLiteralListVariableDefaultValueCommand(IConfigureLiteralListVariableControl configureLiteralListVariableControl);
        UpdateLiteralListVariableDomainCommand GetUpdateLiteralListVariableDomainCommand(IConfigureLiteralListVariableControl configureLiteralListVariableControl);
    }
}
