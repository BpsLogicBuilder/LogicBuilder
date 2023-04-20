using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal interface IEditConditionFunctionsFormCommandFactory
    {
        AddConditionFunctionListBoxItemCommand GetAddConditionFunctionListBoxItemCommand(IEditConditionFunctionsForm editConditionFunctionsForm);
        EditConditionFunctionsFormCopyXmlCommand GetEditConditionFunctionsFormCopyXmlCommand(IEditConditionFunctionsForm editConditionFunctionsForm);
        EditConditionFunctionsFormEditXmlCommand GetEditConditionFunctionsFormEditXmlCommand(IEditConditionFunctionsForm editConditionFunctionsForm);
        UpdateConditionFunctionListBoxItemCommand GetUpdateConditionFunctionListBoxItemCommand(IEditConditionFunctionsForm editConditionFunctionsForm);
    }
}
