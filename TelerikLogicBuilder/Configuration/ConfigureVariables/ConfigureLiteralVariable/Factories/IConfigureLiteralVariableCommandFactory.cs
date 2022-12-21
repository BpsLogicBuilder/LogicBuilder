using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Factories
{
    internal interface IConfigureLiteralVariableCommandFactory
    {
        UpdateLiteralVariableDomainCommand GetUpdateLiteralVariableDomainCommand(IConfigureLiteralVariableControl configureLiteralVariableControl);
    }
}
