namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IPropertyInputRichInputBoxControl : IRichInputBoxValueControl
    {
        string Comments { get; }
        string? SourceClassName { get; }
    }
}
