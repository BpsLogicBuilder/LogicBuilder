using System.Data;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell
{
    internal interface ITableEditor
    {
        void EditCell(DataSet dataSet, GridViewCellInfo currentCell);
    }
}
