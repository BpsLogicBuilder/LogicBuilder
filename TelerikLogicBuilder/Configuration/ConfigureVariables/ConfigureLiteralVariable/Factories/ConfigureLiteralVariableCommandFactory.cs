using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Factories
{
    internal class ConfigureLiteralVariableCommandFactory : IConfigureLiteralVariableCommandFactory
    {
        private readonly Func<IConfigureLiteralVariableControl, UpdateLiteralVariableDomainCommand> _getUpdateLiteralVaraibleDomainCommand;

        public ConfigureLiteralVariableCommandFactory(
            Func<IConfigureLiteralVariableControl, UpdateLiteralVariableDomainCommand> getUpdateLiteralVaraibleDomainCommand)
        {
            _getUpdateLiteralVaraibleDomainCommand = getUpdateLiteralVaraibleDomainCommand;
        }

        public UpdateLiteralVariableDomainCommand GetUpdateLiteralVariableDomainCommand(IConfigureLiteralVariableControl configureLiteralVariableControl)
            => _getUpdateLiteralVaraibleDomainCommand(configureLiteralVariableControl);
    }
}
