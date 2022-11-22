using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Factories
{
    internal class ConfigureGenericLiteralListArgumentCommandFactory : IConfigureGenericLiteralListArgumentCommandFactory
    {
        private readonly Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDefaultValueCommand> _getUpdateGenericLiteralListDefaultValueCommand;
        private readonly Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDomainCommand> _getUpdateGenericLiteralListDomainCommand;

        public ConfigureGenericLiteralListArgumentCommandFactory(
            Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDefaultValueCommand> getUpdateGenericLiteralListDefaultValueCommand,
            Func<IConfigureGenericLiteralListArgumentControl, UpdateGenericLiteralListDomainCommand> getUpdateGenericLiteralListDomainCommand)
        {
            _getUpdateGenericLiteralListDefaultValueCommand = getUpdateGenericLiteralListDefaultValueCommand;
            _getUpdateGenericLiteralListDomainCommand = getUpdateGenericLiteralListDomainCommand;
        }

        public UpdateGenericLiteralListDefaultValueCommand GetUpdateGenericLiteralListDefaultValueCommand(IConfigureGenericLiteralListArgumentControl configureGenericLiteralListArgumentControl)
            => _getUpdateGenericLiteralListDefaultValueCommand(configureGenericLiteralListArgumentControl);

        public UpdateGenericLiteralListDomainCommand GetUpdateGenericLiteralListDomainCommand(IConfigureGenericLiteralListArgumentControl configureGenericLiteralListArgumentControl)
            => _getUpdateGenericLiteralListDomainCommand(configureGenericLiteralListArgumentControl);
    }
}
