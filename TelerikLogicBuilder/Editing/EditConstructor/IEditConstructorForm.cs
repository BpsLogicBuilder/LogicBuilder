using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor
{
    internal interface IEditConstructorForm : IDataGraphEditingForm
    {
        HelperButtonDropDownList CmbSelectConstructor { get; }
        void SetConstructorName(string constructorName);
    }
}
