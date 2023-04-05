using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction
{
    internal interface IEditDialogFunctionForm : IDataGraphEditingForm, IShapeEditForm
    {
        HelperButtonDropDownList CmbSelectFunction { get; }
        void SetFunctionName(string functionName);
    }
}
