using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlFactory
    {
        ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
    }
}
