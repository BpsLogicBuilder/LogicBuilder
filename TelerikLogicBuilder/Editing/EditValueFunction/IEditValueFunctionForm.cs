using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction
{
    internal interface IEditValueFunctionForm : IDataGraphEditingForm
    {
        HelperButtonDropDownList CmbSelectFunction { get; }
        void SetFunctionName(string functionName);
    }
}
