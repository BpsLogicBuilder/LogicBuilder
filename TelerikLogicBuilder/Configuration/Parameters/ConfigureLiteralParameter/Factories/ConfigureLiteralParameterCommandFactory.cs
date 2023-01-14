using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Command;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Factories
{
    internal class ConfigureLiteralParameterCommandFactory : IConfigureLiteralParameterCommandFactory
    {
        private readonly Func<IConfigureLiteralParameterControl, UpdateLiteralParameterDomainCommand> _getUpdateLiteralParameterDomainCommand;

        public ConfigureLiteralParameterCommandFactory(
            Func<IConfigureLiteralParameterControl, UpdateLiteralParameterDomainCommand> getUpdateLiteralParameterDomainCommand)
        {
            _getUpdateLiteralParameterDomainCommand = getUpdateLiteralParameterDomainCommand;
        }

        public UpdateLiteralParameterDomainCommand GetUpdateLiteralParameterDomainCommand(IConfigureLiteralParameterControl configureLiteralParameterControl)
            => _getUpdateLiteralParameterDomainCommand(configureLiteralParameterControl);
    }
}
