using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Factories
{
    internal class ConfigureLiteralListVariableCommandFactory : IConfigureLiteralListVariableCommandFactory
    {
        private readonly Func<IConfigureLiteralListVariableControl, UpdateLiteralListVariableDefaultValueCommand> _getUpdateLiteralListVariableDefaultValueCommand;
        private readonly Func<IConfigureLiteralListVariableControl, UpdateLiteralListVariableDomainCommand> _getUpdateLiteralListVariableDomainCommand;

        public ConfigureLiteralListVariableCommandFactory(
            Func<IConfigureLiteralListVariableControl, UpdateLiteralListVariableDefaultValueCommand> getUpdateLiteralListVariableDefaultValueCommand,
            Func<IConfigureLiteralListVariableControl, UpdateLiteralListVariableDomainCommand> getUpdateLiteralListVariableDomainCommand)
        {
            _getUpdateLiteralListVariableDefaultValueCommand = getUpdateLiteralListVariableDefaultValueCommand;
            _getUpdateLiteralListVariableDomainCommand = getUpdateLiteralListVariableDomainCommand;
        }

        public UpdateLiteralListVariableDefaultValueCommand GetUpdateLiteralListVariableDefaultValueCommand(IConfigureLiteralListVariableControl configureLiteralListVariableControl)
            => _getUpdateLiteralListVariableDefaultValueCommand(configureLiteralListVariableControl);

        public UpdateLiteralListVariableDomainCommand GetUpdateLiteralListVariableDomainCommand(IConfigureLiteralListVariableControl configureLiteralListVariableControl)
            => _getUpdateLiteralListVariableDomainCommand(configureLiteralListVariableControl);
    }
}
