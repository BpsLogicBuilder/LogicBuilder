using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ICellXmlHelper
    {
        string GetXmlString(GridViewCellInfo cell);
        string GetXmlString(string encryptedData, int columnIndex);
        string GetXmlString(string encryptedData, SchemaName schemaName);
        void SetXmlString(DataSet dataSet, GridViewCellInfo currentCell, string cellXml, string visibleText);
    }
}
