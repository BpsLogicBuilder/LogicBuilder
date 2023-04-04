using ABIS.LogicBuilder.FlowBuilder.Data;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IParameterRichTextBoxValueControl : IObjectRichTextBoxValueControl
    {
        LiteralListParameterElementInfo LiteralListElementInfo { get; }
        ObjectListParameterElementInfo ObjectListElementInfo { get; }
    }
}
