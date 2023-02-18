using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlFactory
    {
        ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDomainMultilineControl GetLiteralParameterDomainMultilineControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterMultilineControl GetLiteralParameterMultilineControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterPropertyInputRichInputBoxControl GetLiteralParameterPropertyInputRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterSourcedPropertyRichInputBoxControl GetLiteralParameterSourcedPropertyRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter, IDictionary<string, ParameterControlSet> editControlsSet);
        ILiteralParameterTypeAutoCompleteControl GetLiteralParameterTypeAutoCompleteControl(IEditingControl editingControl, LiteralParameter literalParameter);
        IObjectParameterRichTextBoxControl GetObjectParameterRichTextBoxControl(IEditingControl editingControl, ObjectParameter objectParameter);
    }
}
