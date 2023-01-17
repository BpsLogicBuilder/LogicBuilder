using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories
{
    internal class EditGenericArgumentsCommandFactory : IEditGenericArgumentsCommandFactory
    {
        private readonly Func<IEditGenericArgumentsControl, AddGenericArgumentCommand> _getAddGenericArgumentCommand;
        private readonly Func<IEditGenericArgumentsControl, UpdateGenericArgumentCommand> _getUpdateGenericArgumentCommand;

        public EditGenericArgumentsCommandFactory(
            Func<IEditGenericArgumentsControl, AddGenericArgumentCommand> getAddGenericArgumentCommand,
            Func<IEditGenericArgumentsControl, UpdateGenericArgumentCommand> getUpdateGenericArgumentCommand)
        {
            _getAddGenericArgumentCommand = getAddGenericArgumentCommand;
            _getUpdateGenericArgumentCommand = getUpdateGenericArgumentCommand;
        }

        public AddGenericArgumentCommand GetAddGenericArgumentCommand(IEditGenericArgumentsControl editGenericArgumentsControl)
            => _getAddGenericArgumentCommand(editGenericArgumentsControl);

        public UpdateGenericArgumentCommand GetUpdateGenericArgumentCommand(IEditGenericArgumentsControl editGenericArgumentsControl)
            => _getUpdateGenericArgumentCommand(editGenericArgumentsControl);
    }
}
