namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDataGraphEditingControl : IEditingControl
    {
        bool DenySpecialCharacters { get; }
        bool DisplayNotCheckBox { get; }
    }
}
