using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories
{
    internal class EditConstructorCommandFactory : IEditConstructorCommandFactory
    {
        private readonly Func<IEditConstructorForm, SelectConstructorCommand> _getSelectConstructorCommand;

        public EditConstructorCommandFactory(
            Func<IEditConstructorForm, SelectConstructorCommand> getSelectConstructorCommand)
        {
            _getSelectConstructorCommand = getSelectConstructorCommand;
        }

        public SelectConstructorCommand GetSelectConstructorCommand(IEditConstructorForm editConstructorForm)
            => _getSelectConstructorCommand(editConstructorForm);
    }
}
