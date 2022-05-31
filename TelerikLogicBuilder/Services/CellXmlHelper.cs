using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class CellXmlHelper : ICellXmlHelper
    {
        private readonly IEncryption _encryption;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlValidator _xmlValidator;

        public CellXmlHelper(IEncryption encryption, IExceptionHelper exceptionHelper, IXmlValidator xmlValidator)
        {
            _encryption = encryption;
            _exceptionHelper = exceptionHelper;
            _xmlValidator = xmlValidator;
        }

        public string GetXmlString(DataGridViewCell cell)
        {
            string encryptedData = GetCellString();
            if (encryptedData.Length < 1)
                return string.Empty;

            return GetXmlString(encryptedData, cell.ColumnIndex);

            string GetCellString() 
                => cell.Value == null
                    ? string.Empty
                    : cell.Value.ToString()!;
        }

        public string GetXmlString(string encryptedData, int columnIndex) 
            => GetXmlString(encryptedData, GetSchemaName(columnIndex));

        public string GetXmlString(string encryptedData, SchemaName schemaName)
        {
            try
            {
                string xml = _encryption.Decrypt(encryptedData);

                var validationResponse = _xmlValidator.Validate
                (
                    schemaName,
                    xml
                );

                return validationResponse.Success ? xml : string.Empty;
            }
            catch (XmlException)
            {
                return string.Empty;
            }
        }

        public void SetXmlString(DataSet dataSet, DataGridViewCell currentCell, string cellXml, string visibleText)
        {
            int rowIndex = currentCell.RowIndex;
            int columnIndex = currentCell.ColumnIndex;

            try
            {
                var validationResponse = _xmlValidator.Validate
                (
                    GetSchemaName(columnIndex),
                    cellXml
                );

                if (!validationResponse.Success)
                    return;

                if (dataSet.Tables[TableName.RULESTABLE] == null)
                    throw _exceptionHelper.CriticalException("{4A75DE97-2EEA-4F9E-9CD9-9068F72FCAA0}");

                if (currentCell.DataGridView!.Rows[rowIndex].IsNewRow)
                {
                    DataRow newRow = dataSet.Tables[TableName.RULESTABLE]!.NewRow();
                    //Always set visible text column first.  The visible text column is hidden
                    newRow[columnIndex + 1] = visibleText;
                    newRow[columnIndex] = _encryption.Encrypt(cellXml);
                    dataSet.Tables[TableName.RULESTABLE]!.Rows.Add(newRow);

                    //DataGridView adds an additional row
                    DataGridViewRow? dataGridViewRow = currentCell.DataGridView.Rows.Count > rowIndex ? currentCell.DataGridView.Rows[rowIndex + 1] : null;
                    if (dataGridViewRow != null && !dataGridViewRow.IsNewRow)
                        currentCell.DataGridView.Rows.Remove(dataGridViewRow);
                }
                else
                {
                    //Always set visible text column first.  The visible text column is hidden
                    dataSet.Tables[TableName.RULESTABLE]!.Rows[rowIndex][columnIndex + 1] = visibleText;
                    dataSet.Tables[TableName.RULESTABLE]!.Rows[rowIndex][columnIndex] = _encryption.Encrypt(cellXml);
                }
            }
            catch (XmlException)
            {
            }
        }

        private SchemaName GetSchemaName(int columnIndex)
        {
            return columnIndex switch
            {
                TableColumns.CONDITIONCOLUMNINDEX => SchemaName.ConditionsDataSchema,
                TableColumns.ACTIONCOLUMNINDEX => SchemaName.FunctionsDataSchema,
                TableColumns.PRIORITYCOLUMNINDEX => SchemaName.ShapeDataSchema,
                _ => throw _exceptionHelper.CriticalException("{7599F0A3-01A7-434B-ADA0-6CA33020E5C0}"),
            };
        }
    }
}
