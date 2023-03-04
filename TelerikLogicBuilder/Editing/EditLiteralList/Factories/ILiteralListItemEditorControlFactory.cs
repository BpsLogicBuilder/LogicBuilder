using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface ILiteralListItemEditorControlFactory
    {
        IListOfLiteralsParameterDomainAutoCompleteControl GetListOfLiteralsParameterDomainAutoCompleteControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterDomainMultilineControl GetListOfLiteralsParameterDomainMultilineControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterDomainRichInputBoxControl GetListOfLiteralsParameterDomainRichInputBoxControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterDropDownListControl GetListOfLiteralsParameterDropDownListControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterMultilineControl GetListOfLiteralsParameterMultilineControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterPropertyInputRichInputBoxControl GetListOfLiteralsParameterPropertyInputRichInputBoxControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterRichInputBoxControl GetListOfLiteralsParameterRichInputBoxControl
            (IEditingControl editingControl,
            LiteralListParameterElementInfo listInfo);

        IListOfLiteralsParameterSourcedPropertyRichInputBoxControl GetListOfLiteralsParameterSourcedPropertyRichInputBoxControl
            (IEditingControl editingControl,
            ListOfLiteralsParameter literalListParameter);

        IListOfLiteralsParameterTypeAutoCompleteControl GetListOfLiteralsParameterTypeAutoCompleteControl();
    }
}
