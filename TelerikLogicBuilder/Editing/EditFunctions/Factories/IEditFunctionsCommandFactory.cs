using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal interface IEditFunctionsCommandFactory
    {
        AddFunctionListBoxItemCommand GetAddFunctionListBoxItemCommand(IEditFunctionsForm editFunctionsForm);
        EditFunctionsFormCopyXmlCommand GetEditFFunctionsFormCopyXmlCommand(IEditFunctionsForm editFunctionsForm);
        EditFunctionsFormEditXmlCommand GetEditFunctionsFormXmlCommand(IEditFunctionsForm editFunctionsForm);
        UpdateFunctionListBoxItemCommand GetUpdateFunctionListBoxItemCommand(IEditFunctionsForm editFunctionsForm);
    }
}
