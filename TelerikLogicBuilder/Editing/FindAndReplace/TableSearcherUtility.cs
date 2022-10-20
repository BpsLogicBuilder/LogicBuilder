using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class TableSearcherUtility
    {
        private readonly IContextProvider _contextProvider;
        private readonly IPathHelper _pathHelper;
        private readonly IResultMessageBuilder _resultMessageBuilder;
        private readonly ICellXmlHelper _cellXmlHelper;

        public TableSearcherUtility(
            string sourceFile,
            DataSet dataSet,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource,
            IContextProvider contextProvider,
            ICellXmlHelper cellXmlHelper)
        {
            SourceFile = sourceFile;
            DataSet = dataSet;
            SearchString = searchString;
            MatchCase = matchCase;
            MatchWholeWord = matchWholeWord;
            MatchFunc = matchFunc;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
            _pathHelper = contextProvider.PathHelper;
            _resultMessageBuilder = contextProvider.ResultMessageBuilder;
            _contextProvider = contextProvider;
            _cellXmlHelper = cellXmlHelper;

            FileName = _pathHelper.GetFileName(SourceFile);
        }

        private DataSet DataSet { get; }
        private string SourceFile { get; }
        private string SearchString { get; }
        private bool MatchCase { get; }
        private bool MatchWholeWord { get; }
        private Func<string, string, bool, bool, IList<string>> MatchFunc { get; }
        private IProgress<ProgressMessage> Progress { get; }
        private CancellationTokenSource CancellationTokenSource { get; }

        private string FileName { get; }
        private List<ResultMessage> SearchResults { get; } = new List<ResultMessage>();
        private int CellCount { get; set; }

        public async Task<SearchTableResults> Search()
        {
            await Task.Run
            (
                SearchTable,
                CancellationTokenSource.Token
            );

            return new SearchTableResults
            (
                CellCount,
                SearchResults
            );
        }

        private void AddSearchResult(string message, int row, int column)
            => SearchResults.Add(GetResultMessage(message, GetTableFileSource(row, column)));

        private ResultMessage GetResultMessage(string message, TableFileSource tableFileSource)
            => _resultMessageBuilder.BuilderMessage(tableFileSource, message);

        private TableFileSource GetTableFileSource(int row, int column)
            => new
            (
                _contextProvider.PathHelper,
                SourceFile,
                row,
                column
            );

        private void SearchTable()
        {
            DataTable rulesTable = this.DataSet.Tables[TableName.RULESTABLE]!;
            if (rulesTable.Rows.Count == 0)
                return;

            for (int i = 0; i < rulesTable.Rows.Count; i++)
            {
                int row = i + 1;
                ReportProgress
                (
                    rulesTable.Rows.Count == 1
                        ? 50
                        : (int)((float)(row) / (float)rulesTable.Rows.Count * 100)
                );

                for (int j = 0; j < rulesTable.Rows[i].ItemArray.Length; j++)
                {
                    int column = 0;
                    switch (j)
                    {
                        case TableColumns.CONDITIONCOLUMNINDEX:
                            column = TableColumns.CONDITIONCOLUMNINDEXUSER;
                            break;
                        case TableColumns.ACTIONCOLUMNINDEX:
                            column = TableColumns.ACTIONCOLUMNINDEXUSER;
                            break;
                        case TableColumns.PRIORITYCOLUMNINDEX:
                            column = TableColumns.PRIORITYCOLUMNINDEXUSER;
                            break;
                        default:
                            continue;
                    }

                    string cellXml = _cellXmlHelper.GetXmlString((string)rulesTable.Rows[i].ItemArray.GetValue(j)!, j);
                    if (cellXml.Length == 0)
                        continue;

                    IList<string> matches = this.MatchFunc(cellXml, this.SearchString, this.MatchCase, this.MatchWholeWord);
                    if (matches.Count > 0)
                    {
                        string cellText = string.Join(Strings.spaceString, matches.Select(m => m.Replace(Environment.NewLine, Strings.spaceString)));
                        CellCount++;
                        AddSearchResult
                        (
                            cellText.Length > MiscellaneousConstants.MAXTEXTDISPLAYED
                                ? cellText[..MiscellaneousConstants.MAXTEXTDISPLAYED]
                                : cellText,
                            row,
                            column
                        );
                    }
                }
            }

            void ReportProgress(int percent)
            {
                Progress.Report
                (
                    new ProgressMessage
                    (
                        percent,
                        string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskSearchingFileFormat, this.FileName)
                    )
                );
            }
        }
    }
}
