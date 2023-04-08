namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable
{
    internal interface IEditVariableForm : IEditingForm, IEditVariableHost
    {
        string VariableName { get; }
        void SetVariable(string variableName);
    }
}
