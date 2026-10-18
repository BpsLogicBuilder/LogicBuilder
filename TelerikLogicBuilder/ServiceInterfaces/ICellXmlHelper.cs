using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Data;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ICellXmlHelper
    {
        string GetXmlString(GridViewCellInfo cell);
        string GetXmlString(string cellData, int columnIndex);
        string GetXmlString(string cellData, SchemaName schemaName);
        void SetXmlString(DataSet dataSet, GridViewCellInfo currentCell, string cellXml, string visibleText);
        void SetXmlString(DataSet dataSet, int rowIndex, int columnIndex, string cellXml, string visibleText);
    }
}
