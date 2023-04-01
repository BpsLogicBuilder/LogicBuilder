using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories
{
    internal class EditValueFunctionCommandFactory : IEditValueFunctionCommandFactory
    {
        private readonly Func<IEditValueFunctionForm, EditValueFunctionFormXmlCommand> _getEditValueFunctionFormXmlCommand;
        private readonly Func<IEditValueFunctionForm, SelectValueFunctionCommand> _getSelectValueFunctionCommand;

        public EditValueFunctionCommandFactory(
            Func<IEditValueFunctionForm, EditValueFunctionFormXmlCommand> getEditValueFunctionFormXmlCommand,
            Func<IEditValueFunctionForm, SelectValueFunctionCommand> getSelectValueFunctionCommand)
        {
            _getEditValueFunctionFormXmlCommand = getEditValueFunctionFormXmlCommand;
            _getSelectValueFunctionCommand = getSelectValueFunctionCommand;
        }

        public EditValueFunctionFormXmlCommand GetEditValueFunctionFormXmlCommand(IEditValueFunctionForm editFunctionForm)
            => _getEditValueFunctionFormXmlCommand(editFunctionForm);

        public SelectValueFunctionCommand GetSelectValueFunctionCommand(IEditValueFunctionForm editFunctionForm)
            => _getSelectValueFunctionCommand(editFunctionForm);
    }
}
