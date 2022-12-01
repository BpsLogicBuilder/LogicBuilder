using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories
{
    internal interface IConfigureLiteralDomainCommandFactory
    {
        AddLiteralDomainListBoxItemCommand GetAddLiteralDomainListBoxItemCommand(IConfigureLiteralDomainControl configureLiteralDomainControl);
        UpdateLiteralDomainListBoxItemCommand GetUpdateLiteralDomainListBoxItemCommand(IConfigureLiteralDomainControl configureLiteralDomainControl);
    }
}
