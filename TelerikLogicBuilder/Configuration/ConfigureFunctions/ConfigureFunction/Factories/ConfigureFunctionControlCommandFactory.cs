using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Factories
{
    internal class ConfigureFunctionControlCommandFactory : IConfigureFunctionControlCommandFactory
    {
        private readonly Func<IConfigureFunctionControl, ConfigureFunctionReturnTypeCommand> _getConfigureFunctionReturnTypeCommand;
        private readonly Func<IConfigureFunctionControl, EditFunctionGenericArgumentsCommand> _getEditFunctionGenericArgumentsCommand;

        public ConfigureFunctionControlCommandFactory(
            Func<IConfigureFunctionControl, ConfigureFunctionReturnTypeCommand> getConfigureFunctionReturnTypeCommand,
            Func<IConfigureFunctionControl, EditFunctionGenericArgumentsCommand> getEditFunctionGenericArgumentsCommand)
        {
            _getConfigureFunctionReturnTypeCommand = getConfigureFunctionReturnTypeCommand;
            _getEditFunctionGenericArgumentsCommand = getEditFunctionGenericArgumentsCommand;
        }

        public ConfigureFunctionReturnTypeCommand GetConfigureFunctionReturnTypeCommand(IConfigureFunctionControl configureFunctionControl)
            => _getConfigureFunctionReturnTypeCommand(configureFunctionControl);

        public EditFunctionGenericArgumentsCommand GetEditFunctionGenericArgumentsCommand(IConfigureFunctionControl configureFunctionControl)
            => _getEditFunctionGenericArgumentsCommand(configureFunctionControl);
    }
}
