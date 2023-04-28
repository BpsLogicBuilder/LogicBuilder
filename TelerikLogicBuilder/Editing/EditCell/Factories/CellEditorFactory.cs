using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell.Factories
{
    internal class CellEditorFactory : ICellEditorFactory
    {
        private readonly Func<int, ICellEditor> _getCellEditor;

        public CellEditorFactory(Func<int, ICellEditor> getCellEditor)
        {
            _getCellEditor = getCellEditor;
        }

        public ICellEditor GetCellEditor(int column)
            => _getCellEditor(column);
    }
}
