using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Factories
{
    internal interface IConfigureGenericLiteralArgumentCommandFactory
    {
        UpdateGenericLiteralDomainCommand GetUpdateGenericLiteralDomainCommand(IConfigureGenericLiteralArgumentControl configureGenericLiteralArgumentControl);
    }
}
