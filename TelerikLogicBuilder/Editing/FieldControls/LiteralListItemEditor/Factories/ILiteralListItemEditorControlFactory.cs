using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Factories
{
    internal interface ILiteralListItemEditorControlFactory
    {
        IListOfLiteralsItemDomainAutoCompleteControl GetListOfLiteralsItemDomainAutoCompleteControl(
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemDomainMultilineControl GetListOfLiteralsItemDomainMultilineControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemDomainRichInputBoxControl GetListOfLiteralsItemDomainRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemDropDownListControl GetListOfLiteralsItemDropDownListControl(
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemMultilineControl GetListOfLiteralsItemMultilineControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemPropertyInputRichInputBoxControl GetListOfLiteralsItemPropertyInputRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemRichInputBoxControl GetListOfLiteralsItemRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsItemTypeAutoCompleteControl GetListOfLiteralsItemTypeAutoCompleteControl();
    }
}
