using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction
{
    internal interface IEditBooleanFunctionForm : IDataGraphEditingForm
    {
        HelperButtonDropDownList CmbSelectFunction { get; }
        void SetFunctionName(string functionName);
    }
}
