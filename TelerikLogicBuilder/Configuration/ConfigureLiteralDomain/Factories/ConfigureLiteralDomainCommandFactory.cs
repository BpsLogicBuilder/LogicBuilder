using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories
{
    internal class ConfigureLiteralDomainCommandFactory : IConfigureLiteralDomainCommandFactory
    {
        private readonly Func<IConfigureLiteralDomainControl, AddLiteralDomainListBoxItemCommand> _getAddLiteralDomainListBoxItemCommand;
        private readonly Func<IConfigureLiteralDomainControl, UpdateLiteralDomainListBoxItemCommand> _getUpdateLiteralDomainListBoxItemCommand;

        public ConfigureLiteralDomainCommandFactory(
            Func<IConfigureLiteralDomainControl, AddLiteralDomainListBoxItemCommand> getAddLiteralDomainListBoxItemCommand,
            Func<IConfigureLiteralDomainControl, UpdateLiteralDomainListBoxItemCommand> getUpdateLiteralDomainListBoxItemCommand)
        {
            _getAddLiteralDomainListBoxItemCommand = getAddLiteralDomainListBoxItemCommand;
            _getUpdateLiteralDomainListBoxItemCommand = getUpdateLiteralDomainListBoxItemCommand;
        }

        public AddLiteralDomainListBoxItemCommand GetAddLiteralDomainListBoxItemCommand(IConfigureLiteralDomainControl configureLiteralDomainControl)
            => _getAddLiteralDomainListBoxItemCommand(configureLiteralDomainControl);

        public UpdateLiteralDomainListBoxItemCommand GetUpdateLiteralDomainListBoxItemCommand(IConfigureLiteralDomainControl configureLiteralDomainControl)
            => _getUpdateLiteralDomainListBoxItemCommand(configureLiteralDomainControl);
    }
}
