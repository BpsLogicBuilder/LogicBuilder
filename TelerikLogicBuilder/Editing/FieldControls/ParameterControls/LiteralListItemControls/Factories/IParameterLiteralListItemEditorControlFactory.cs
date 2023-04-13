using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls.Factories
{
    internal interface IParameterLiteralListItemEditorControlFactory
    {
        IListOfLiteralsParameterItemDomainAutoCompleteControl GetListOfLiteralsParameterItemDomainAutoCompleteControl(
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterItemDomainMultilineControl GetListOfLiteralsParameterItemDomainMultilineControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterItemDomainRichInputBoxControl GetListOfLiteralsParameterItemDomainRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterItemDropDownListControl GetListOfLiteralsParameterItemDropDownListControl(
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterItemMultilineControl GetListOfLiteralsParameterItemMultilineControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterItemPropertyInputRichInputBoxControl GetListOfLiteralsParameterItemPropertyInputRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterItemRichInputBoxControl GetListOfLiteralsParameterItemRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsParameterItemParameterSourcedPropertyRichInputBoxControl(
            IDataGraphEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsParameterItemTypeAutoCompleteControl GetListOfLiteralsParameterItemTypeAutoCompleteControl();
    }
}
