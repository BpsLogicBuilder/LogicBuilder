using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction
{
    internal interface IEditDialogFunctionForm : IDataGraphEditingForm
    {
        HelperButtonDropDownList CmbSelectFunction { get; }
        string XmlResult { get; }
        void SetFunctionName(string functionName);
    }
}
