using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Factories
{
    internal interface IConfigureLiteralListParameterCommandFactory
    {
        UpdateLiteralListParameterDefaultValueCommand GetUpdateLiteralListParameterDefaultValueCommand(IConfigureLiteralListParameterControl configureLiteralListParameterControl);
        UpdateLiteralListParameterDomainCommand GetUpdateLiteralListParameterDomainCommand(IConfigureLiteralListParameterControl configureLiteralListParameterControl);
    }
}
