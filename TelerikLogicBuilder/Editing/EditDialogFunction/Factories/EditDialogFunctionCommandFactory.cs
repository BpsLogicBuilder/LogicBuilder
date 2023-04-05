using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories
{
    internal class EditDialogFunctionCommandFactory : IEditDialogFunctionCommandFactory
    {
        private readonly Func<IEditDialogFunctionForm, EditDialogFunctionFormXmlCommand> _getEditDialogFunctionFormXmlCommand;
        private readonly Func<IEditDialogFunctionForm, SelectDialogFunctionCommand> _getSelectDialogFunctionCommand;

        public EditDialogFunctionCommandFactory(
            Func<IEditDialogFunctionForm, EditDialogFunctionFormXmlCommand> getEditDialogFunctionFormXmlCommand,
            Func<IEditDialogFunctionForm, SelectDialogFunctionCommand> getSelectDialogFunctionCommand)
        {
            _getEditDialogFunctionFormXmlCommand = getEditDialogFunctionFormXmlCommand;
            _getSelectDialogFunctionCommand = getSelectDialogFunctionCommand;
        }

        public EditDialogFunctionFormXmlCommand GetEditDialogFunctionFormXmlCommand(IEditDialogFunctionForm editDialogFunctionForm)
            => _getEditDialogFunctionFormXmlCommand(editDialogFunctionForm);

        public SelectDialogFunctionCommand GetSelectDialogFunctionCommand(IEditDialogFunctionForm editDialogFunctionForm)
            => _getSelectDialogFunctionCommand(editDialogFunctionForm);
    }
}
