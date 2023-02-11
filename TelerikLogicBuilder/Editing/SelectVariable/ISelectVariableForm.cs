namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal interface ISelectVariableForm : IEditingForm
    {
        string VariableName { get; }
        void SetVariable(string variableName);
    }
}
