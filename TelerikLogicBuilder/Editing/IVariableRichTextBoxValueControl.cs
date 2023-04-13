using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IVariableRichTextBoxValueControl : IObjectRichTextBoxValueControl
    {
        LiteralListVariableElementInfo LiteralListElementInfo { get; }
        ObjectListVariableElementInfo ObjectListElementInfo { get; }
    }
}
