using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories
{
    internal interface IEditSetValueFunctionCommandFactory
    {
        SelectVariableCommand GetSelectVariableCommand(HelperButtonDropDownList helperButtonDropDownList);
    }
}
