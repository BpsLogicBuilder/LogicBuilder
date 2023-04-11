using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface IEditLiteralListCommandFactory
    {
        AddLiteralListBoxItemCommand GetAddLiteralListBoxItemCommand(IEditParameterLiteralListControl editLiteralListControl);
        EditLiteralListFormXmlCommand GetEditLiteralListFormXmlCommand(IEditParameterLiteralListForm editLiteralListForm);
        UpdateLiteralListBoxItemCommand GetUpdateLiteralListBoxItemCommand(IEditParameterLiteralListControl editLiteralListControl);
    }
}
