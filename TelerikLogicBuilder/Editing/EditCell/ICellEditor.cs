using System.Data;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell
{
    internal interface ICellEditor
    {
        void Edit(DataSet dataSet, GridViewCellInfo currentCell);
    }
}
