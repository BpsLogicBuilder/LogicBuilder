using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal interface IConditionFunctionListBoxItemFactory
    {
        IConditionFunctionListBoxItem GetConditionFunctionListBoxItem(string visibleText,
            string hiddenText,
            IApplicationControl applicationControl);
    }
}
