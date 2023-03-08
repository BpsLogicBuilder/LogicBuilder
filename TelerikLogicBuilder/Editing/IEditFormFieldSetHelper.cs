using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IEditFormFieldSetHelper
    {
        EditFormFieldSet GetFieldSetForFunction(Function function);
    }
}
