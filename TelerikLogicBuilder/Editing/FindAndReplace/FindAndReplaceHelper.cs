﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindAndReplaceHelper : IFindAndReplaceHelper
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IConditionsDataParser _conditionsDataParser;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IDecisionDataParser _decisionDataParser;
        private readonly IDecisionsDataParser _decisionsDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IMainWindow _mainWindow;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IPriorityDataParser _priorityDataParser;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FindAndReplaceHelper(
            IAssertFunctionDataParser assertFunctionDataParser,
            ICellXmlHelper cellXmlHelper,
            IConditionsDataParser conditionsDataParser,
            IConnectorDataParser connectorDataParser,
            IDecisionDataParser decisionDataParser,
            IDecisionsDataParser decisionsDataParser,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionsDataParser functionsDataParser,
            IJumpDataParser jumpDataParser,
            IMainWindow mainWindow,
            IModuleDataParser moduleDataParser,
            IPriorityDataParser priorityDataParser,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IRetractFunctionDataParser retractFunctionDataParser,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _assertFunctionDataParser = assertFunctionDataParser;
            _cellXmlHelper = cellXmlHelper;
            _conditionsDataParser = conditionsDataParser;
            _connectorDataParser = connectorDataParser;
            _decisionDataParser = decisionDataParser;
            _decisionsDataParser = decisionsDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionsDataParser = functionsDataParser;
            _jumpDataParser = jumpDataParser;
            _mainWindow = mainWindow;
            _moduleDataParser = moduleDataParser;
            _priorityDataParser = priorityDataParser;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _retractFunctionDataParser = retractFunctionDataParser;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Shape? FindItemAllPages(Document visioDocument, RadListControl listOccurrences, RadGroupBox groupBoxOccurrences, ref int searchPageIndex, ref int searchShapeIndex, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc)
        {
            for (int i = searchPageIndex; i <= visioDocument.Pages.Count; i++)
            {
                for (int j = searchShapeIndex; j <= visioDocument.Pages[i].Shapes.Count; j++)
                {
                    IList<string> matches = FindMatchesInShape(visioDocument.Pages[i].Shapes[j], searchString, matchFunc, matchCase, matchWholeWord);
                    groupBoxOccurrences.Text = GetOccurrencesString(matches);
                    if (matches.Any())
                    {
                        listOccurrences.Items.AddRange(matches);
                        double xCoordinate = visioDocument.Pages[i].Shapes[j].get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowXFormOut, (short)VisCellIndices.visXFormPinX).ResultIU;
                        double yCoordinate = visioDocument.Pages[i].Shapes[j].get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowXFormOut, (short)VisCellIndices.visXFormPinY).ResultIU;
                        visioDocument.Application.ActiveWindow.Page = visioDocument.Pages[i];
                        visioDocument.Application.ActiveWindow.Select(visioDocument.Pages[i].Shapes[j], (short)VisSelectArgs.visSelect);
                        visioDocument.Application.ActiveWindow.ScrollViewTo(xCoordinate, yCoordinate);
                        if (j < visioDocument.Pages[i].Shapes.Count)
                        {
                            searchShapeIndex = j + 1;
                            searchPageIndex = i;
                        }
                        else if (j == visioDocument.Pages[i].Shapes.Count)
                        {
                            searchShapeIndex = 1;
                            searchPageIndex = i + 1;
                        }
                        return visioDocument.Pages[i].Shapes[j];
                    }
                }
                searchShapeIndex = 1;
                searchPageIndex = i + 1;
            }
            searchPageIndex = 1;
            searchShapeIndex = 1;

            DisplayMessage.Show(_mainWindow.Instance, Strings.searchAllPagesComplete, _mainWindow.RightToLeft);
            return null;
        }

        public GridViewCellInfo? FindItemAllRows(RadGridView dataGridView, RadListControl listOccurrences, RadGroupBox groupBoxOccurrences, ref int searchRowIndex, ref int searchCellIndex, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc)
        {
            GridViewRowCollection rows = dataGridView.Rows;
            GridViewColumnCollection columns = dataGridView.Columns;

            for (int i = searchRowIndex; i < rows.Count; i++)
            {
                for (int j = searchCellIndex; j < rows[i].Cells.Count; j++)
                {
                    IList<string> matches = FindMatchesInCell(rows[i].Cells[j].Value.ToString() ?? string.Empty, j, searchString, matchFunc, matchCase, matchWholeWord);
                    groupBoxOccurrences.Text = GetOccurrencesString(matches);
                    if (matches.Any())
                    {
                        listOccurrences.Items.AddRange(matches);
                        rows[i].IsCurrent = true;
                        columns[j].IsCurrent = true;
                        rows[i].Cells[j].EnsureVisible();

                        if (j < rows[i].Cells.Count)
                        {
                            searchCellIndex = j + 1;
                            searchRowIndex = i;
                        }
                        else if (j == rows[i].Cells.Count)
                        {
                            searchCellIndex = 0;
                            searchRowIndex = i + 1;
                        }
                        return rows[i].Cells[j];
                    }
                }
                searchCellIndex = 0;
                searchRowIndex = i + 1;
            }
            searchRowIndex = 0;
            searchCellIndex = 0;
            DisplayMessage.Show(_mainWindow.Instance, Strings.searchAllRowsComplete, _mainWindow.RightToLeft);
            return null;
        }

        public Shape? FindItemCurrentPage(Document visioDocument, RadListControl listOccurrences, RadGroupBox groupBoxOccurrences, ref int searchShapeIndex, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc)
        {
            Page page = (Page)visioDocument.Application.ActiveWindow.Page;
            for (int j = searchShapeIndex; j <= page.Shapes.Count; j++)
            {
                IList<string> matches = FindMatchesInShape(page.Shapes[j], searchString, matchFunc, matchCase, matchWholeWord);
                groupBoxOccurrences.Text = GetOccurrencesString(matches);
                if (matches.Any())
                {
                    listOccurrences.Items.AddRange(matches);
                    double xCoordinate = page.Shapes[j].get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowXFormOut, (short)VisCellIndices.visXFormPinX).ResultIU;
                    double yCoordinate = page.Shapes[j].get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowXFormOut, (short)VisCellIndices.visXFormPinY).ResultIU;
                    visioDocument.Application.ActiveWindow.Page = page;
                    visioDocument.Application.ActiveWindow.Select(page.Shapes[j], (short)VisSelectArgs.visSelect);
                    visioDocument.Application.ActiveWindow.ScrollViewTo(xCoordinate, yCoordinate);

                    if (j <= page.Shapes.Count)
                    {
                        searchShapeIndex = j + 1;
                    }

                    return page.Shapes[j];
                }
            }

            searchShapeIndex = 1;
            DisplayMessage.Show(_mainWindow.Instance, Strings.searchPageComplete, _mainWindow.RightToLeft);
            return null;
        }

        public GridViewCellInfo? FindItemCurrentRow(RadGridView dataGridView, RadListControl listOccurrences, RadGroupBox groupBoxOccurrences, ref int searchCellIndex, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc)
        {
            GridViewRowInfo? row = dataGridView.CurrentCell?.RowInfo;

            if (row == null)
            {
                DisplayMessage.Show(_mainWindow.Instance, Strings.noCurrentRow, _mainWindow.RightToLeft);
                return null;
            }

            for (int j = searchCellIndex; j < row.Cells.Count; j++)
            {
                IList<string> matches = FindMatchesInCell(row.Cells[j].Value.ToString() ?? string.Empty, j, searchString, matchFunc, matchCase, matchWholeWord);
                groupBoxOccurrences.Text = GetOccurrencesString(matches);
                if (matches.Any())
                {
                    listOccurrences.Items.AddRange(matches);
                    dataGridView.Columns[j].IsCurrent = true;
                    row.Cells[j].EnsureVisible();
                    if (j < row.Cells.Count)
                    {
                        searchCellIndex = j + 1;
                    }

                    return row.Cells[j];
                }
            }
            searchCellIndex = 0;
            DisplayMessage.Show(_mainWindow.Instance, Strings.searchRowComplete, _mainWindow.RightToLeft);
            return null;
        }

        public string GetVisibleText(int columnIndex, string cellXml)
        {
            if (string.IsNullOrEmpty(cellXml))
                throw _exceptionHelper.CriticalException("{10C27EB5-87A4-4FE2-B28C-7E9FBA1A6D63}");

            return columnIndex switch
            {
                TableColumns.CONDITIONCOLUMNINDEX => BuildConditionsVisibleText(cellXml),
                TableColumns.ACTIONCOLUMNINDEX => BuildFunctionsVisibleText(cellXml),
                TableColumns.PRIORITYCOLUMNINDEX => _priorityDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(cellXml)
                )?.ToString(CultureInfo.CurrentCulture) ?? throw _exceptionHelper.CriticalException("{BA99F97A-BD12-4460-A8F9-EC922024B01E}"),
                _ => throw _exceptionHelper.CriticalException("{DDADAAEE-D01B-40B5-BA3B-F5797BF4F337}"),
            };
        }

        public string GetVisibleText(string universalMasterName, string shapeXml)
        {
            if (string.IsNullOrEmpty(shapeXml))
                throw _exceptionHelper.CriticalException("{88F99CEE-D7DD-4F43-86C4-9DFE1E6ED345}");

            return universalMasterName switch
            {
                UniversalMasterName.ACTION => BuildFunctionsVisibleText(shapeXml),
                UniversalMasterName.DIALOG => BuildDialogFunctionsVisibleText(shapeXml),
                UniversalMasterName.CONDITIONOBJECT or UniversalMasterName.WAITCONDITIONOBJECT => BuildConditionsVisibleText(shapeXml),
                UniversalMasterName.DECISIONOBJECT or UniversalMasterName.WAITDECISIONOBJECT => BuildDecisionsVisibleText(shapeXml),
                UniversalMasterName.JUMPOBJECT => _jumpDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(shapeXml)),
                UniversalMasterName.MODULE => _moduleDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(shapeXml)),
                UniversalMasterName.CONNECTOBJECT => BuildConnectorVisibleText(shapeXml),
                _ => throw _exceptionHelper.CriticalException("{157F2D43-55B1-45E8-BA66-017B240FC61E}"),
            };
        }

        public void ReplaceCellItem(GridViewCellInfo currentCell,
            DataSet dataSet,
            string searchString,
            string replaceString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, string, bool, bool, string> replaceFunc,
            ApplicationTypeInfo applicationTypeInfo)
        {
            switch (currentCell.ColumnInfo.Index)
            {
                case TableColumns.CONDITIONCOLUMNINDEX:
                case TableColumns.ACTIONCOLUMNINDEX:
                case TableColumns.PRIORITYCOLUMNINDEX:
                    break;
                default:
                    return;
            }

            string cellXml = _cellXmlHelper.GetXmlString(currentCell);
            if (cellXml.Length == 0)
                return;

            cellXml = replaceFunc(cellXml, searchString, replaceString, matchCase, matchWholeWord);
            cellXml = RefreshCellDataVisibleTexts(cellXml, applicationTypeInfo);

            _cellXmlHelper.SetXmlString(dataSet, currentCell, cellXml, GetVisibleText(currentCell.ColumnInfo.Index, cellXml));
        }

        public void ReplaceShapeItem(Shape shape,
            string searchString,
            string replaceString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, string, bool, bool, string> replaceFunc,
            ApplicationTypeInfo applicationTypeInfo)
        {
            if (!ShapeCollections.TextSearchableShapes.Contains(shape.Master.NameU))
                return;

            string shapeXml = _shapeXmlHelper.GetXmlString(shape);
            if (shapeXml.Length == 0)
                return;

            shapeXml = replaceFunc(shapeXml, searchString, replaceString, matchCase, matchWholeWord);
            shapeXml = RefreshShapeDataVisibleTexts(shapeXml, applicationTypeInfo);

            _shapeXmlHelper.SetXmlString(shape, shapeXml, GetVisibleText(shape.Master.NameU, shapeXml));
        }

        private string BuildConditionsVisibleText(string shapeXml)
        {
            ConditionsData conditionsData = _conditionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(shapeXml)
            );

            return string.Join
            (
                string.Concat
                (
                    Environment.NewLine,
                    conditionsData.FirstChildElementName == XmlDataConstants.ORELEMENT 
                        ? Strings.builtInFunctionNameOr 
                        : Strings.builtInFunctionNameAnd,
                    Environment.NewLine
                ),
                conditionsData.FunctionElements.Select
                (
                    i => _functionDataParser.Parse(i).VisibleText
                )
            );
        }

        private string BuildConnectorVisibleText(string shapeXml)
        {
            ConnectorData connectorData = _connectorDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(shapeXml)
            );

            return connectorData.ConnectorCategory == ConnectorCategory.Dialog
                ? string.Format
                (
                    CultureInfo.CurrentCulture, 
                    Strings.dialogConnectorFormat, 
                    connectorData.Index.ToString(CultureInfo.CurrentCulture), 
                    _xmlDocumentHelpers.GetVisibleText(connectorData.TextXmlNode)
                )
                : _xmlDocumentHelpers.GetVisibleText(connectorData.TextXmlNode);
        }

        private string BuildDecisionsVisibleText(string shapeXml)
        {
            DecisionsData decisionsData = _decisionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(shapeXml)
            );

            return string.Join
            (
                string.Concat
                (
                    Environment.NewLine,
                    decisionsData.FirstChildElementName == XmlDataConstants.ORELEMENT 
                        ? Strings.builtInFunctionNameOr 
                        : Strings.builtInFunctionNameAnd,
                    Environment.NewLine
                ),
                decisionsData.DecisionElements.Select
                (
                    i => _decisionDataParser.Parse(i).VisibleText
                )
            );
        }

        private string BuildDialogFunctionsVisibleText(string shapeXml)
            => _functionDataParser.Parse
            (
                _functionsDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(shapeXml)
                )
                .FunctionElements[0]
            ).VisibleText;

        private string BuildFunctionsVisibleText(string shapeXml)
        {
            return string.Join
            (
                Environment.NewLine,
                _functionsDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(shapeXml)
                )
                .FunctionElements.Select
                (
                    e =>
                    {
                        return e.Name switch
                        {
                            XmlDataConstants.FUNCTIONELEMENT => _functionDataParser.Parse(e).VisibleText,
                            XmlDataConstants.ASSERTFUNCTIONELEMENT => _assertFunctionDataParser.Parse(e).VisibleText,
                            XmlDataConstants.RETRACTFUNCTIONELEMENT => _retractFunctionDataParser.Parse(e).VisibleText,
                            _ => throw _exceptionHelper.CriticalException("{30146EA8-536A-47CC-86BA-ADB807D9EF92}"),
                        };
                    }
                )
            );
        }

        private IList<string> FindMatchesInShape(Shape shape, string searchString, Func<string, string, bool, bool, IList<string>> matchFunc, bool matchCase, bool matchWholeWord)
        {
            if (!ShapeCollections.TextSearchableShapes.Contains(shape.Master.NameU))
                return new List<string>();

            string shapeXml = _shapeXmlHelper.GetXmlString(shape);
            if (shapeXml.Length == 0)
                return Array.Empty<string>();

            return matchFunc(shapeXml, searchString, matchCase, matchWholeWord);
        }

        private IList<string> FindMatchesInCell(string cellText, int columnIndex, string searchString, Func<string, string, bool, bool, IList<string>> matchFunc, bool matchCase, bool matchWholeWord)
        {
            switch (columnIndex)
            {
                case TableColumns.CONDITIONCOLUMNINDEX:
                case TableColumns.ACTIONCOLUMNINDEX:
                case TableColumns.PRIORITYCOLUMNINDEX:
                    break;
                default:
                    return Array.Empty<string>();
            }

            string cellXml = _cellXmlHelper.GetXmlString(cellText, columnIndex);
            if (cellXml.Length == 0)
                return new List<string>();

            return matchFunc(cellXml, searchString, matchCase, matchWholeWord);
        }

        private static string GetOccurrencesString(IList<string> matches)
        {
            if (!matches.Any())
                return string.Empty;

            return string.Format(CultureInfo.CurrentCulture, Strings.searchOccurrencesFormat, matches.Count.ToString(CultureInfo.CurrentCulture));
        }

        private string RefreshCellDataVisibleTexts(string xmlString, ApplicationTypeInfo applicationTypeInfo)
        {
            XmlDocument cellXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
            cellXmlDocument = _refreshVisibleTextHelper.RefreshVariableVisibleTexts(cellXmlDocument);
            cellXmlDocument = _refreshVisibleTextHelper.RefreshFunctionVisibleTexts(cellXmlDocument, applicationTypeInfo);
            cellXmlDocument = _refreshVisibleTextHelper.RefreshConstructorVisibleTexts(cellXmlDocument, applicationTypeInfo);
            cellXmlDocument = _refreshVisibleTextHelper.RefreshSetValueFunctionVisibleTexts(cellXmlDocument, applicationTypeInfo);
            cellXmlDocument = _refreshVisibleTextHelper.RefreshSetValueToNullFunctionVisibleTexts(cellXmlDocument);
            return cellXmlDocument.OuterXml;
        }

        private string RefreshShapeDataVisibleTexts(string xmlString, ApplicationTypeInfo applicationTypeInfo)
        {
            XmlDocument shapeXmlDocument = _xmlDocumentHelpers.ToXmlDocument(xmlString);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshVariableVisibleTexts(shapeXmlDocument);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshFunctionVisibleTexts(shapeXmlDocument, applicationTypeInfo);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshConstructorVisibleTexts(shapeXmlDocument, applicationTypeInfo);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshDecisionVisibleTexts(shapeXmlDocument, applicationTypeInfo);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshSetValueFunctionVisibleTexts(shapeXmlDocument, applicationTypeInfo);
            shapeXmlDocument = _refreshVisibleTextHelper.RefreshSetValueToNullFunctionVisibleTexts(shapeXmlDocument);
            return shapeXmlDocument.OuterXml;
        }
    }
}
