using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls
{
    internal interface ILiteralVariableTypeAutoCompleteControl : IValueControl, ITypeAutoCompleteTextControl
    {
        void RequestDocumentUpdate();
    }
}
