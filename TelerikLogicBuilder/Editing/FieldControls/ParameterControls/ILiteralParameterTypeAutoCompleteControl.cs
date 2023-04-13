using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls
{
    internal interface ILiteralParameterTypeAutoCompleteControl : IValueControl, ITypeAutoCompleteTextControl
    {
        void RequestDocumentUpdate();
    }
}
