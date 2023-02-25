namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal interface IEditVariableForm : IEditingForm
    {
        string VariableName { get; }
        void SetVariable(string variableName);
    }
}
