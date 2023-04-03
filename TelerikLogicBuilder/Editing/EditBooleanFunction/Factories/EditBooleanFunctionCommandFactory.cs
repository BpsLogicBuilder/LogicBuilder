using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories
{
    internal class EditBooleanFunctionCommandFactory : IEditBooleanFunctionCommandFactory
    {
        private readonly Func<IEditBooleanFunctionForm, EditBooleanFunctionFormXmlCommand> _getEditBooleanFunctionFormXmlCommand;
        private readonly Func<IEditBooleanFunctionForm, SelectBooleanFunctionCommand> _getSelectBooleanFunctionCommand;

        public EditBooleanFunctionCommandFactory(
            Func<IEditBooleanFunctionForm, EditBooleanFunctionFormXmlCommand> getEditBooleanFunctionFormXmlCommand,
            Func<IEditBooleanFunctionForm, SelectBooleanFunctionCommand> getSelectBooleanFunctionCommand)
        {
            _getEditBooleanFunctionFormXmlCommand = getEditBooleanFunctionFormXmlCommand;
            _getSelectBooleanFunctionCommand = getSelectBooleanFunctionCommand;
        }

        public EditBooleanFunctionFormXmlCommand GetEditBooleanFunctionFormXmlCommand(IEditBooleanFunctionForm editFunctionForm)
            => _getEditBooleanFunctionFormXmlCommand(editFunctionForm);

        public SelectBooleanFunctionCommand GetSelectBooleanFunctionCommand(IEditBooleanFunctionForm editFunctionForm)
            => _getSelectBooleanFunctionCommand(editFunctionForm);
    }
}
