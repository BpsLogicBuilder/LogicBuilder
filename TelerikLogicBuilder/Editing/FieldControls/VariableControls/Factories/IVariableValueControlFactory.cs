using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories
{
    internal interface IVariableValueControlFactory
    {
        ILiteralListVariableRichTextBoxControl GetiteralListVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable listOfLiteralsVariable);
        ILiteralVariableDomainAutoCompleteControl GetLiteralVariableDomainAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariableDomainMultilineControl GetLiteralVariableDomainMultilineControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariableDomainRichInputBoxControl GetLiteralVariableDomainRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariableDropDownListControl GetLiteralVariableDropDownListControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariableMultilineControl GetLiteralVariableMultilineControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariablePropertyInputRichInputBoxControl GetLiteralVariablePropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariableRichInputBoxControl GetLiteralVariableRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        ILiteralVariableTypeAutoCompleteControl GetLiteralVariableTypeAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable);
        IObjectListVariableRichTextBoxControl GetObjectListVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfObjectsVariable listOfObjectsVariable);
        IObjectVariableRichTextBoxControl GetObjectVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ObjectVariable objectVariable);
    }
}
