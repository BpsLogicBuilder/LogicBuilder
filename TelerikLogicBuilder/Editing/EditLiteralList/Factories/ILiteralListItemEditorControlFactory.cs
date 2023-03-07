using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface ILiteralListItemEditorControlFactory
    {
        IListOfLiteralsItemDomainAutoCompleteControl GetListOfLiteralsItemDomainAutoCompleteControl(
            IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemDomainMultilineControl GetListOfLiteralsItemDomainMultilineControl(
            IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemDomainRichInputBoxControl GetListOfLiteralsItemDomainRichInputBoxControl(
            IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemDropDownListControl GetListOfLiteralsItemDropDownListControl(
            IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemMultilineControl GetListOfLiteralsItemMultilineControl(
            IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemPropertyInputRichInputBoxControl GetListOfLiteralsItemPropertyInputRichInputBoxControl(
            IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsItemRichInputBoxControl GetListOfLiteralsItemRichInputBoxControl(
            IEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsItemParameterSourcedPropertyRichInputBoxControl(
            IEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsItemTypeAutoCompleteControl GetListOfLiteralsItemTypeAutoCompleteControl();
    }
}
