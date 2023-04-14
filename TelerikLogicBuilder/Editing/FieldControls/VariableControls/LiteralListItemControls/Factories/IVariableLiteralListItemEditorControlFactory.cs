using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories
{
    internal interface IVariableLiteralListItemEditorControlFactory
    {
        IListOfLiteralsVariableItemDomainAutoCompleteControl GetListOfLiteralsVariableItemDomainAutoCompleteControl(
            ListOfLiteralsVariable literalListVariable);

        IListOfLiteralsVariableItemDomainMultilineControl GetListOfLiteralsVariableItemDomainMultilineControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsVariable literalListVariable);

        IListOfLiteralsVariableItemDomainRichInputBoxControl GetListOfLiteralsVariableItemDomainRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsVariable literalListVariable);

        IListOfLiteralsVariableItemDropDownListControl GetListOfLiteralsVariableItemDropDownListControl(
            ListOfLiteralsVariable literalListVariable);

        IListOfLiteralsVariableItemMultilineControl GetListOfLiteralsVariableItemMultilineControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsVariable literalListVariable);

        IListOfLiteralsVariableItemPropertyInputRichInputBoxControl GetListOfLiteralsVariableItemPropertyInputRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsVariable literalListVariable);

        IListOfLiteralsVariableItemRichInputBoxControl GetListOfLiteralsVariableItemRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            LiteralListVariableElementInfo listInfo);

        IListOfLiteralsVariableItemTypeAutoCompleteControl GetListOfLiteralsVariableItemTypeAutoCompleteControl();
    }
}
