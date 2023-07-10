using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories
{
    internal class EditSetValueFunctionCommandFactory : IEditSetValueFunctionCommandFactory
    {
        public SelectVariableCommand GetSelectVariableCommand(HelperButtonDropDownList helperButtonDropDownList)
           => new
           (
               helperButtonDropDownList
           );
    }
}
