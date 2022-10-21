using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class DiagramSearcher : IDiagramSearcher
    {
        private readonly IResultMessageBuilder _resultMessageBuilder;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IStructuresFactory _structuresFactory;

        public DiagramSearcher(
            IPathHelper pathHelper,
            IResultMessageBuilder resultMessageBuilder,
            IShapeXmlHelper shapeXmlHelper,
            IStructuresFactory structuresFactory,
            string sourceFile,
            Document document,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource)
        {
            _resultMessageBuilder = resultMessageBuilder;
            _shapeXmlHelper = shapeXmlHelper;
            _structuresFactory = structuresFactory;

            SourceFile = sourceFile;
            Document = document;
            SearchString = searchString;
            MatchCase = matchCase;
            MatchWholeWord = matchWholeWord;
            MatchFunc = matchFunc;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
            FileName = pathHelper.GetFileName(SourceFile);
        }

        private Document Document { get; }
        private string SourceFile { get; }
        private string SearchString { get; }
        private bool MatchCase { get; }
        private bool MatchWholeWord { get; }
        private Func<string, string, bool, bool, IList<string>> MatchFunc { get; }
        private IProgress<ProgressMessage> Progress { get; }
        private CancellationTokenSource CancellationTokenSource { get; }

        private string FileName { get; }
        private List<ResultMessage> SearchResults { get; } = new List<ResultMessage>();
        private int ShapeCount { get; set; }

        public async Task<SearchDiagramResults> Search()
        {
            await Task.Run
            (
                SearchDiagram,
                CancellationTokenSource.Token
            );

            return new SearchDiagramResults
            (
                ShapeCount,
                SearchResults
            );
        }

        private void AddSearchResult(string message, Page page, Shape shape)
            => SearchResults.Add(GetResultMessage(message, GetVisioFileSource(page, shape)));

        private ResultMessage GetResultMessage(string message, VisioFileSource visioFileSource)
            => _resultMessageBuilder.BuilderMessage(visioFileSource, message);

        private VisioFileSource GetVisioFileSource(Page page, Shape shape)
            => _structuresFactory.GetVisioFileSource
            (
                SourceFile,
                page.ID,
                page.Index,
                shape.Master.Name,
                shape.ID,
                shape.Index
            );

        private void SearchDiagram()
        {
            for (int i = 1; i <= this.Document.Pages.Count; i++)
            {
                Page page = this.Document.Pages[i];
                ReportProgress
                (
                    this.Document.Pages.Count == 1
                        ? 50
                        : (int)((float)i / (float)this.Document.Pages.Count * 100)
                );

                for (int j = 1; j <= page.Shapes.Count; j++)
                {
                    Shape shape = page.Shapes[j];
                    if (!ShapeCollections.TextSearchableShapes.Contains(shape.Master.NameU))
                        continue;

                    string shapeXml = _shapeXmlHelper.GetXmlString(shape);
                    if (shapeXml.Length == 0)
                        continue;

                    IList<string> matches = this.MatchFunc(shapeXml, this.SearchString, this.MatchCase, this.MatchWholeWord);
                    if (matches.Count > 0)
                    {
                        string shapeText = string.Join(Strings.spaceString, matches.Select(m => m.Replace(Environment.NewLine, Strings.spaceString)));
                        ShapeCount++;
                        AddSearchResult
                        (
                            shapeText.Length > MiscellaneousConstants.MAXTEXTDISPLAYED
                                ? shapeText[..MiscellaneousConstants.MAXTEXTDISPLAYED]
                                : shapeText,
                            page,
                            shape
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
