using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Command;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Factories
{
    internal interface IConfigureLiteralParameterCommandFactory
    {
        UpdateLiteralParameterDomainCommand GetUpdateLiteralParameterDomainCommand(IConfigureLiteralParameterControl configureLiteralParameterControl);
    }
}
