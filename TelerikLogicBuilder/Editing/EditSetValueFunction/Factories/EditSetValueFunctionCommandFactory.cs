using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories
{
    internal class EditSetValueFunctionCommandFactory : IEditSetValueFunctionCommandFactory
    {
        private readonly Func<HelperButtonDropDownList, SelectVariableCommand> _getSelectVariableCommand;

        public EditSetValueFunctionCommandFactory(Func<HelperButtonDropDownList, SelectVariableCommand> getSelectVariableCommand)
        {
            _getSelectVariableCommand = getSelectVariableCommand;
        }

        public SelectVariableCommand GetSelectVariableCommand(HelperButtonDropDownList helperButtonDropDownList)
            => _getSelectVariableCommand(helperButtonDropDownList);
    }
}
