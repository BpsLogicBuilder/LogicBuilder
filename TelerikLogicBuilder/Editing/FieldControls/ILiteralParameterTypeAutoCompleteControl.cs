using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface ILiteralParameterTypeAutoCompleteControl : IValueControl, ITypeAutoCompleteTextControl
    {
        void RequestDocumentUpdate();
    }
}
