using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IFunctionsFormFieldSetHelper
    {
        FunctionsFormFieldSet GetFunctionsFormFieldSet(Function function);
        FunctionsFormFieldSet GetFunctionsFormFieldSet(RadTreeNode treeNode);
    }
}
