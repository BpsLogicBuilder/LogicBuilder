using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class TableValidator : ITableValidator
    {
        private readonly ICellHelper _cellHelper;
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IConditionsElementValidator _conditionsElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionsElementValidator _functionsElementValidator;
        private readonly IPriorityDataParser _priorityDataParser;
        private readonly IResultMessageBuilder _resultMessageBuilder;
        private readonly ITableFileSourceFactory _tableFileSourceFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public TableValidator(
            ICellHelper cellHelper,
            ICellXmlHelper cellXmlHelper,
            IConditionsElementValidator conditionsElementValidator,
            IExceptionHelper exceptionHelper,
            IFunctionsElementValidator functionsElementValidator,
            IPathHelper pathHelper,
            IPriorityDataParser priorityDataParser,
            IResultMessageBuilder resultMessageBuilder,
            ITableFileSourceFactory tableFileSourceFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            DataSet dataSet,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource)
        {
            _cellHelper = cellHelper;
            _cellXmlHelper = cellXmlHelper;
            _conditionsElementValidator = conditionsElementValidator;
            _exceptionHelper = exceptionHelper;
            _functionsElementValidator = functionsElementValidator;
            _priorityDataParser = priorityDataParser;
            _resultMessageBuilder = resultMessageBuilder;
            _tableFileSourceFactory = tableFileSourceFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            SourceFile = sourceFile;
            FileName = pathHelper.GetFileName(SourceFile);
            DataSet = dataSet;
            Application = application;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
        }

        #region Properties
        private string SourceFile { get; }
        private string FileName { get; }
        private DataSet DataSet { get; }
        private ApplicationTypeInfo Application { get; }
        private List<ResultMessage> ValidationErrors { get; } = new List<ResultMessage>();
        private IProgress<ProgressMessage> Progress { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
        #endregion Properties

        public async Task<IList<ResultMessage>> Validate()
        {
            await Task.Run(ValidateTable, CancellationTokenSource.Token);
            return ValidationErrors;
        }

        private void ValidateTable()
        {
            Progress.Report
            (
                new ProgressMessage
                (
                    0,
                    string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskValidatingFormat, FileName)
                )
            );

            for (int i = 0; i < this.DataSet.Tables[TableName.RULESTABLE]!.Rows.Count; i++)
            {
                for (int j = 0; j < this.DataSet.Tables[TableName.RULESTABLE]!.Rows[i].ItemArray.Length; j++)
                {
                    if (j == TableColumns.ACTIONCOLUMNINDEXVISIBLE || j == TableColumns.CONDITIONCOLUMNINDEXVISIBLE || j == TableColumns.PRIORITYCOLUMNINDEXVISIBLE)
                        continue;

                    switch (j)
                    {
                        case TableColumns.CONDITIONCOLUMNINDEX:
                            ValidateConditionsXmlData(GetCellValue(), i + 1, TableColumns.CONDITIONCOLUMNINDEXUSER);
                            break;
                        case TableColumns.ACTIONCOLUMNINDEX:
                            ValidateActionXmlData(GetCellValue(), i + 1, TableColumns.ACTIONCOLUMNINDEXUSER);
                            break;
                        case TableColumns.PRIORITYCOLUMNINDEX:
                            ValidatePriorityXmlData(GetCellValue(), i + 1, TableColumns.PRIORITYCOLUMNINDEXUSER);
                            break;
                        case TableColumns.REEVALUATECOLUMNINDEX:
                            ValidateReEvaluateData(GetCellValue(), i + 1, TableColumns.REEVALUATECOLUMNINDEXUSER);
                            break;
                        case TableColumns.ACTIVECOLUMNINDEX:
                            ValidateActiveData(GetCellValue(), i + 1, TableColumns.ACTIVECOLUMNINDEXUSER);
                            break;
                        default:
                            throw _exceptionHelper.CriticalException("{7B84D841-C67D-423C-A63D-7F8B68D1B916}");
                    }

                    object GetCellValue()
                        => this.DataSet.Tables[TableName.RULESTABLE]!.Rows[i].ItemArray.GetValue(j)!;
                }

                Progress.Report
                (
                    new ProgressMessage
                    (
                        0,
                        string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskValidatingFormat, FileName)
                    )
                );
            }
        }

        private void ValidateActionXmlData(object fieldValue, int row, int column)
        {
            int dialogFunctions = _cellHelper.CountDialogFunctions(fieldValue);
            if (dialogFunctions > 0)
                AddValidationMessage(Strings.actionCellDialogFunctionsInvalid, row, column);

            string functionsXml = _cellXmlHelper.GetXmlString((string)fieldValue, SchemaName.FunctionsDataSchema);
            if (functionsXml.Length == 0)
            {
                AddValidationMessage(Strings.cellDataRequired, row, column);
                return;
            }

            List<string> errors = new();
            _functionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(functionsXml),
                this.Application,
                errors
            );
            errors.ForEach(error => AddValidationMessage(error, row, column));
        }

        private void ValidateConditionsXmlData(object fieldValue, int row, int column)
        {
            string conditionsXml = _cellXmlHelper.GetXmlString((string)fieldValue, SchemaName.ConditionsDataSchema);
            if (conditionsXml.Length == 0)
                return;

            List<string> errors = new();
            _conditionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(conditionsXml),
                this.Application,
                errors
            );
            errors.ForEach(error => AddValidationMessage(error, row, column));
        }

        private void ValidatePriorityXmlData(object fieldValue, int row, int column)
        {
            string priorityXml = _cellXmlHelper.GetXmlString((string)fieldValue, SchemaName.ShapeDataSchema);
            if (priorityXml.Length == 0)
            {
                AddValidationMessage(Strings.invalidPriorityCellData, row, column);
                return;
            }

            int? priority = _priorityDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(priorityXml));
            if (!priority.HasValue)
            {
                AddValidationMessage(Strings.invalidPriorityCellData, row, column);
                return;
            }

            if (priority < 1)
                AddValidationMessage(Strings.invalidPriorityCellData, row, column);
        }

        private void ValidateReEvaluateData(object fieldValue, int row, int column)
        {
            if (!bool.TryParse(fieldValue.ToString(), out bool _))
                AddValidationMessage(Strings.invalidCellData, row, column);
        }

        private void ValidateActiveData(object fieldValue, int row, int column)
        {
            if (!bool.TryParse(fieldValue.ToString(), out bool _))
                AddValidationMessage(Strings.invalidCellData, row, column);
        }

        private TableFileSource GetTableFileSource(int row, int column)
            => _tableFileSourceFactory.GetTableFileSource
            (
                SourceFile,
                row,
                column
            );

        private void AddValidationMessage(string message, int row, int column)
            => ValidationErrors.Add(GetResultMessage(message, GetTableFileSource(row, column)));

        private ResultMessage GetResultMessage(string message, TableFileSource tableFileSource)
            => _resultMessageBuilder.BuilderMessage(tableFileSource, message);
    }
}
