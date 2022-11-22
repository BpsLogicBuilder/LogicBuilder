using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralArgument.Factories
{
    internal class ConfigureGenericLiteralArgumentCommandFactory : IConfigureGenericLiteralArgumentCommandFactory
    {
        private readonly Func<IConfigureGenericLiteralArgumentControl, UpdateGenericLiteralDomainCommand> _getUpdateDomainCommand;

        public ConfigureGenericLiteralArgumentCommandFactory(
            Func<IConfigureGenericLiteralArgumentControl, UpdateGenericLiteralDomainCommand> getUpdateDomainCommand)
        {
            _getUpdateDomainCommand = getUpdateDomainCommand;
        }

        public UpdateGenericLiteralDomainCommand GetUpdateGenericLiteralDomainCommand(IConfigureGenericLiteralArgumentControl configureGenericLiteralArgumentControl)
            => _getUpdateDomainCommand(configureGenericLiteralArgumentControl);
    }
}
