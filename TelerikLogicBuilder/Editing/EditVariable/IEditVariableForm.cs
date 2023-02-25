namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable
{
    internal interface IEditVariableForm : IEditingForm
    {
        string VariableName { get; }
        void SetVariable(string variableName);
    }
}
