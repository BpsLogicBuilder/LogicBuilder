using ABIS.LogicBuilder.FlowBuilder.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IPropertyInputRichInputBoxControl : IRichInputBoxValueControl
    {
        ApplicationTypeInfo Application { get; }
        string Comments { get; }
        string? SourceClassName { get; }
    }
}
