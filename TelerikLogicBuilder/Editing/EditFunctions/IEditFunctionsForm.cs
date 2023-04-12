using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal interface IEditFunctionsForm : IShapeEditForm, IApplicationForm
    {
        IEditingControl? CurrentEditingControl { get; }
        IEditVoidFunctionControl EditVoidFunctionControl { get; }
        IRadListBoxManager<IFunctionListBoxItem> RadListBoxManager { get; }
        void UpdateFunctionsList(string xmlString);
    }
}
