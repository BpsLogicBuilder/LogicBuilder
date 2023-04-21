using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions
{
    internal interface IEditConditionFunctionsForm : IShapeEditForm, IApplicationForm
    {
        IEditingControl? CurrentEditingControl { get; }
        IEditConditionFunctionControl EditConditionFunctionControl { get; }
        IRadListBoxManager<IConditionFunctionListBoxItem> RadListBoxManager { get; }
        void UpdateConditionsList(string? xmlString);
    }
}
