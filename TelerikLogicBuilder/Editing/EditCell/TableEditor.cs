using ABIS.LogicBuilder.FlowBuilder.Editing.EditCell.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell
{
    internal class TableEditor : ITableEditor
    {
        private readonly IMainWindow _mainWindow;
        private readonly ICellEditorFactory _cellEditorFactory;

        public TableEditor(IMainWindow mainWindow, ICellEditorFactory cellEditorFactory)
        {
            _mainWindow = mainWindow;
            _cellEditorFactory = cellEditorFactory;
        }

        public async void EditCell(DataSet dataSet, GridViewCellInfo currentCell)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            await mdiParent.RunLoadContextAsync(Edit);

            Task Edit(CancellationTokenSource cancellationTokenSource)
            {
                _cellEditorFactory
                    .GetCellEditor(currentCell.ColumnInfo.Index)
                    .Edit(dataSet, currentCell);

                return Task.CompletedTask;
            }
        }
    }
}
