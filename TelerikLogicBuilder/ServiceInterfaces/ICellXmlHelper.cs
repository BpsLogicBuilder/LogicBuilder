using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Data;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ICellXmlHelper
    {
        string GetXmlString(DataGridViewCell cell);
        string GetXmlString(string encryptedData, int columnIndex);
        string GetXmlString(string encryptedData, SchemaName schemaName);
        void SetXmlString(DataSet dataSet, DataGridViewCell currentCell, string cellXml, string visibleText);
    }
}
