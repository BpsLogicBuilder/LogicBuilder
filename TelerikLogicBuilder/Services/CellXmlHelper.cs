using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class CellXmlHelper : ICellXmlHelper
    {
        private readonly IEncryption _encryption;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlValidatorFactory _xmlValidatorFactory;

        public CellXmlHelper(IEncryption encryption, IExceptionHelper exceptionHelper, IXmlValidatorFactory xmlValidatorFactory)
        {
            _encryption = encryption;
            _exceptionHelper = exceptionHelper;
            _xmlValidatorFactory = xmlValidatorFactory;
        }

        public string GetXmlString(GridViewCellInfo cell)
        {
            string encryptedData = GetCellString();
            if (encryptedData.Length < 1)
                return string.Empty;

            return GetXmlString(encryptedData, cell.ColumnInfo.Index);

            string GetCellString() 
                => cell.Value?.ToString() ?? string.Empty;
        }

        public string GetXmlString(string encryptedData, int columnIndex) 
            => GetXmlString(encryptedData, GetSchemaName(columnIndex));

        public string GetXmlString(string encryptedData, SchemaName schemaName)
        {
            try
            {
                string xml = _encryption.Decrypt(encryptedData);

                var validationResponse = _xmlValidatorFactory.GetXmlValidator
                (
                    schemaName
                ).Validate(xml);

                return validationResponse.Success ? xml : string.Empty;
            }
            catch (XmlException)
            {
                return string.Empty;
            }
        }

        public void SetXmlString(DataSet dataSet, GridViewCellInfo currentCell, string cellXml, string visibleText)
        {
            int rowIndex = currentCell.RowInfo.Index;
            int columnIndex = currentCell.ColumnInfo.Index;

            try
            {
                var validationResponse = _xmlValidatorFactory.GetXmlValidator
                (
                    GetSchemaName(columnIndex)
                ).Validate(cellXml);

                if (!validationResponse.Success)
                    throw _exceptionHelper.CriticalException("{EE8A6D4C-E57B-4BD3-9DD5-3C8F741C63C5}");

                if (dataSet.Tables[TableName.RULESTABLE] == null)
                    throw _exceptionHelper.CriticalException("{4A75DE97-2EEA-4F9E-9CD9-9068F72FCAA0}");

                //Always set visible text column first.  The visible text column is hidden
                dataSet.Tables[TableName.RULESTABLE]!.Rows[rowIndex][columnIndex + 1] = visibleText;
                dataSet.Tables[TableName.RULESTABLE]!.Rows[rowIndex][columnIndex] = _encryption.Encrypt(cellXml);
            }
            catch (XmlException)
            {
                throw _exceptionHelper.CriticalException("{21C80621-8E80-434A-B2C1-3E75E6528122}");
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
