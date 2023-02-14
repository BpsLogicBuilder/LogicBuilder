using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlFactory
    {
        ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDomainMultilineControl GetLiteralParameterDomainMultilineControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterMultilineControl GetLiteralParameterMultilineControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterTypeAutoCompleteControl GetLiteralParameterTypeAutoCompleteControl(IEditingControl editingControl);
    }
}
