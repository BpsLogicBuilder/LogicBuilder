using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface IEditLiteralListCommandFactory
    {
        AddParameterLiteralListBoxItemCommand GetAddParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl);
        AddVariableLiteralListBoxItemCommand GetAddVariableLiteralListBoxItemCommand(IEditVariableLiteralListControl editVariableLiteralListControl);
        EditParameterLiteralListFormXmlCommand GetEditParameterLiteralListFormXmlCommand(IEditParameterLiteralListForm editParameterLiteralListForm);
        EditVariableLiteralListFormXmlCommand GetEditVariableLiteralListFormXmlCommand(IEditVariableLiteralListForm editVariableLiteralListForm);
        UpdateParameterLiteralListBoxItemCommand GetUpdateParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl);
        UpdateVariableLiteralListBoxItemCommand GetUpdateVariableLiteralListBoxItemCommand(IEditVariableLiteralListControl editVariableLiteralListControl);
    }
}
