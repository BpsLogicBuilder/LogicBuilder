using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Factories
{
    internal class ConfigureConstructorControlCommandFactory : IConfigureConstructorControlCommandFactory
    {
        private readonly Func<IConfigureConstructorControl, EditConstructorTypeNameCommand> _getEditConstructorTypeNameCommand;
        private readonly Func<IConfigureConstructorControl, EditGenericArgumentsCommand> _getEditGenericArgumentsCommand;

        public ConfigureConstructorControlCommandFactory(
            Func<IConfigureConstructorControl, EditConstructorTypeNameCommand> getEditConstructorTypeNameCommand,
            Func<IConfigureConstructorControl, EditGenericArgumentsCommand> getEditGenericArgumentsCommand)
        {
            _getEditConstructorTypeNameCommand = getEditConstructorTypeNameCommand;
            _getEditGenericArgumentsCommand = getEditGenericArgumentsCommand;
        }

        public EditConstructorTypeNameCommand GetEditConstructorTypeNameCommand(IConfigureConstructorControl configureConstructorControl)
            => _getEditConstructorTypeNameCommand(configureConstructorControl);

        public EditGenericArgumentsCommand GetEditGenericArgumentsCommand(IConfigureConstructorControl configureConstructorControl)
            => _getEditGenericArgumentsCommand(configureConstructorControl);
    }
}
