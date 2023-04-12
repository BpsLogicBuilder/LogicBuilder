using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface IEditLiteralListCommandFactory
    {
        AddParameterLiteralListBoxItemCommand GetAddParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl);
        EditParameterLiteralListFormXmlCommand GetEditParameterLiteralListFormXmlCommand(IEditParameterLiteralListForm editParameterLiteralListForm);
        UpdateParameterLiteralListBoxItemCommand GetUpdateParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl);
    }
}
