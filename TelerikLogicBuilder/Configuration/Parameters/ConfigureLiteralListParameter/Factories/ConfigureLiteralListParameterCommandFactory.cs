using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Factories
{
    internal class ConfigureLiteralListParameterCommandFactory : IConfigureLiteralListParameterCommandFactory
    {
        private readonly Func<IConfigureLiteralListParameterControl, UpdateLiteralListParameterDefaultValueCommand> _getUpdateLiteralListParameterDefaultValueCommand;
        private readonly Func<IConfigureLiteralListParameterControl, UpdateLiteralListParameterDomainCommand> _getUpdateLiteralListParameterDomainCommand;

        public ConfigureLiteralListParameterCommandFactory(
            Func<IConfigureLiteralListParameterControl, UpdateLiteralListParameterDefaultValueCommand> getUpdateLiteralListParameterDefaultValueCommand,
            Func<IConfigureLiteralListParameterControl, UpdateLiteralListParameterDomainCommand> getUpdateLiteralListParameterDomainCommand)
        {
            _getUpdateLiteralListParameterDefaultValueCommand = getUpdateLiteralListParameterDefaultValueCommand;
            _getUpdateLiteralListParameterDomainCommand = getUpdateLiteralListParameterDomainCommand;
        }

        public UpdateLiteralListParameterDefaultValueCommand GetUpdateLiteralListParameterDefaultValueCommand(IConfigureLiteralListParameterControl configureLiteralListParameterControl)
            => _getUpdateLiteralListParameterDefaultValueCommand(configureLiteralListParameterControl);

        public UpdateLiteralListParameterDomainCommand GetUpdateLiteralListParameterDomainCommand(IConfigureLiteralListParameterControl configureLiteralListParameterControl)
            => _getUpdateLiteralListParameterDomainCommand(configureLiteralListParameterControl);
    }
}
