using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface IEditLiteralListCommandFactory
    {
        AddLiteralListBoxItemCommand GetAddLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl);
        EditLiteralListFormXmlCommand GetEditLiteralListFormXmlCommand(IEditLiteralListForm editLiteralListForm);
        UpdateLiteralListBoxItemCommand GetUpdateLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl);
    }
}
