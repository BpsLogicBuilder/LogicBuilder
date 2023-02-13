using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlFactory
    {
        ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter);
    }
}
