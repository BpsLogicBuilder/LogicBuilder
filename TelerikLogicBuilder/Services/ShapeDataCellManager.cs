using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Office.Interop.Visio;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ShapeDataCellManager : IShapeDataCellManager
    {
        private readonly IExceptionHelper _exceptionHelper;
        private const string PROPERTYLOCK = "1";
        private const string PROPERTYUNLOCK = "0";

        public ShapeDataCellManager(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public void AddPropertyCell(Shape shape, string cellName, string labelName)
        {
            AddPropertyCell(shape, cellName, labelName, VisCellVals.visPropTypeString, "@", string.Empty, false, true, string.Empty);
        }

        public bool CellExists(Shape shape, string cellName)
        {
            if (string.IsNullOrEmpty(cellName))
                throw _exceptionHelper.CriticalException("{FCA76B2D-2C4C-4F5E-8E26-C5E62013B9E2}");

            return shape.get_CellExistsU($"{CustomPropertyConstants.PREFIX}{cellName}", (short)VisExistsFlags.visExistsLocally) != 0;
        }

        public string GetPropertyString(Shape shape, string cellName)
        {
            if (!CellExists(shape, cellName))
                return string.Empty;

            string propName = $"{CustomPropertyConstants.PREFIX}{cellName}";
            Cell customPropertyCell = shape.get_CellsU(propName);

            if (customPropertyCell == null)
                throw new CriticalLogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.customPropertyNotFoundFormat, propName));

            return customPropertyCell.get_ResultStr(VisUnitCodes.visNoCast);
        }

        public string GetRulesDataString(Shape shape) 
            => GetPropertyString(shape, CustomPropertyConstants.RULESDATAU);

        public void LockUpdate(Shape shape)
        {
            Cell lockTextEditCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLock, (short)VisCellIndices.visLockTextEdit);
            Cell lockCustPropCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLock, (short)VisCellIndices.visLockCustProp);
            lockTextEditCell.FormulaU = PROPERTYLOCK;
            lockCustPropCell.FormulaU = PROPERTYLOCK;
        }

        public void SetPropertyString(Shape shape, string cellName, string stringValue)
        {
            if (!CellExists(shape, cellName))
            {
                throw _exceptionHelper.CriticalException("{C53BB0D6-1C09-4272-9F66-AA80E1CE13B4}");
            }
            else
            {
                Cell customPropertyCell = shape.get_CellsU($"{CustomPropertyConstants.PREFIX}{cellName}");
                SetCellValueToString(customPropertyCell, stringValue);
            }
        }

        public void SetRulesDataString(Shape shape, string stringValue)
        {
            AddPropertyCell(shape, CustomPropertyConstants.RULESDATAU, CustomPropertyConstants.RULESDATALABEL);
            SetPropertyString(shape, CustomPropertyConstants.RULESDATAU, stringValue);
        }

        public void UnlockUpdate(Shape shape)
        {
            Cell lockTextEditCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLock, (short)VisCellIndices.visLockTextEdit);
            Cell lockCustPropCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLock, (short)VisCellIndices.visLockCustProp);
            lockTextEditCell.FormulaU = PROPERTYUNLOCK;
            lockCustPropCell.FormulaU = PROPERTYUNLOCK;
        }

        private void AddPropertyCell(Shape shape, string cellName, string labelName, VisCellVals propType, string format, string prompt, bool askOnDrop, bool hidden, string sortKey)
        {
            Cell shapeCell;
            short rowIndex;
            string propName = $"{CustomPropertyConstants.PREFIX}{cellName}";

            if (CellExists(shape, cellName))
                rowIndex = shape.get_CellsRowIndexU(propName);
            else
                rowIndex = shape.AddNamedRow((short)(VisSectionIndices.visSectionProp), cellName, (short)(VisRowIndices.visRowProp));

            // Column 1 : Prompt
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsPrompt);
            SetCellValueToString(shapeCell, prompt);

            // Column 2 : Label
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsLabel);
            SetCellValueToString(shapeCell, labelName);

            // Column 3 : Format
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsFormat);
            SetCellValueToString(shapeCell, format);

            // Column 4 : Sort Key
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsSortKey);
            SetCellValueToString(shapeCell, sortKey);

            // Column 5 : Type
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsType);
            SetCellValueToString(shapeCell, ((short)propType).ToString(CultureInfo.InvariantCulture));

            // Column 6 : Hidden (This corresponds to the invisible cell in the Shapesheet)
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsInvis);
            SetCellValueToString(shapeCell, hidden.ToString(CultureInfo.InvariantCulture));

            // Column 7 : Ask on drop
            shapeCell = shape.get_CellsSRC((short)VisSectionIndices.visSectionProp, rowIndex, (short)VisCellIndices.visCustPropsAsk);
            SetCellValueToString(shapeCell, askOnDrop.ToString(CultureInfo.InvariantCulture));

            //Cell value
            shapeCell = shape.get_CellsU(propName);
            SetCellValueToString(shapeCell, string.Empty);
        }

        private static void SetCellValueToString(Cell formulaCell, string newValue)
        {
            formulaCell.FormulaU = StringToFormulaForString(newValue);
        }

        private static string StringToFormulaForString(string inputValue)
        {
            string quote = "\"";
            string quoteQuote = "\"\"";

            string result = inputValue ?? string.Empty;
            result = result.Replace(quote, quoteQuote);
            result = quote + result + quote;

            return result;
        }
    }
}
