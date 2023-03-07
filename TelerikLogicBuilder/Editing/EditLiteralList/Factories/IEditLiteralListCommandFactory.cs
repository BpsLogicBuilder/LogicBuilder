using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface IEditLiteralListCommandFactory
    {
        AddLiteralListBoxItemCommand GetAddLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl);
        UpdateLiteralListBoxItemCommand GetUpdateLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl);
    }
}
