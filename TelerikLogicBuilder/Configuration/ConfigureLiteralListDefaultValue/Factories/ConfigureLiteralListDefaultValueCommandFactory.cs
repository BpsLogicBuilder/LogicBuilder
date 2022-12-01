using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories
{
    internal class ConfigureLiteralListDefaultValueCommandFactory : IConfigureLiteralListDefaultValueCommandFactory
    {
        private readonly Func<IConfigureLiteralListDefaultValueControl, AddLiteralListDefaultValueItemCommand> _getAddLiteralListDefaultValueItemCommand;
        private readonly Func<IConfigureLiteralListDefaultValueControl, UpdateLiteralListDefaultValueItemCommand> _getUpdateLiteralListDefaultValueItemCommand;

        public ConfigureLiteralListDefaultValueCommandFactory(
            Func<IConfigureLiteralListDefaultValueControl, AddLiteralListDefaultValueItemCommand> getAddLiteralListDefaultValueItemCommand,
            Func<IConfigureLiteralListDefaultValueControl, UpdateLiteralListDefaultValueItemCommand> getUpdateLiteralListDefaultValueItemCommand)
        {
            _getAddLiteralListDefaultValueItemCommand = getAddLiteralListDefaultValueItemCommand;
            _getUpdateLiteralListDefaultValueItemCommand = getUpdateLiteralListDefaultValueItemCommand;
        }

        public AddLiteralListDefaultValueItemCommand GetAddLiteralListDefaultValueItemCommand(IConfigureLiteralListDefaultValueControl configureLiteralListDefaultValueControl)
            => _getAddLiteralListDefaultValueItemCommand(configureLiteralListDefaultValueControl);

        public UpdateLiteralListDefaultValueItemCommand GetUpdateLiteralListDefaultValueItemCommand(IConfigureLiteralListDefaultValueControl configureLiteralListDefaultValueControl)
            => _getUpdateLiteralListDefaultValueItemCommand(configureLiteralListDefaultValueControl);
    }
}
