namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls
{
    internal interface ILiteralListParameterRichTextBoxControl : IParameterRichTextBoxValueControl
    {
        //use this to create LiteralListParameterElementInfo with ListOfLiteralsParameter for the LiteralList dialog form
        string? ParameterSourceClassName { get; }
    }
}
