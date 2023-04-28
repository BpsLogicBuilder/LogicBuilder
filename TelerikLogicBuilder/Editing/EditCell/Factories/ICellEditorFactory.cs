namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell.Factories
{
    internal interface ICellEditorFactory
    {
        ICellEditor GetCellEditor(int column);
    }
}
