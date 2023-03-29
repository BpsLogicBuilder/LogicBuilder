using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories
{
    internal class EditConstructorCommandFactory : IEditConstructorCommandFactory
    {
        private readonly Func<IEditConstructorForm, EditConstructorFormXmlCommand> _getEditFormXmlCommand;
        private readonly Func<IEditConstructorForm, SelectConstructorCommand> _getSelectConstructorCommand;

        public EditConstructorCommandFactory(
            Func<IEditConstructorForm, EditConstructorFormXmlCommand> getEditFormXmlCommand,
            Func<IEditConstructorForm, SelectConstructorCommand> getSelectConstructorCommand)
        {
            _getEditFormXmlCommand = getEditFormXmlCommand;
            _getSelectConstructorCommand = getSelectConstructorCommand;
        }

        public EditConstructorFormXmlCommand GetEditFormXmlCommand(IEditConstructorForm editConstructorForm)
            => _getEditFormXmlCommand(editConstructorForm);

        public SelectConstructorCommand GetSelectConstructorCommand(IEditConstructorForm editConstructorForm)
            => _getSelectConstructorCommand(editConstructorForm);
    }
}
