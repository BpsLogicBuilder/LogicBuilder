using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class SearcherFactory : ISearcherFactory
    {
        private readonly Func<string, Document, string, bool, bool, Func<string, string, bool, bool, IList<string>>, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramSearcher> _getDiagramSearcher;
        private readonly Func<string, DataSet, string, bool, bool, Func<string, string, bool, bool, IList<string>>, IProgress<ProgressMessage>, CancellationTokenSource, ITableSearcher> _getTableSearcher;

        public SearcherFactory(
            Func<string, Document, string, bool, bool, Func<string, string, bool, bool, IList<string>>, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramSearcher> getDiagramSearcher,
            Func<string, DataSet, string, bool, bool, Func<string, string, bool, bool, IList<string>>, IProgress<ProgressMessage>, CancellationTokenSource, ITableSearcher> getTableSearcher)
        {
            _getDiagramSearcher = getDiagramSearcher;
            _getTableSearcher = getTableSearcher;
        }

        public IDiagramSearcher GetDiagramSearcher(string sourceFile, Document document, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getDiagramSearcher(sourceFile, document, searchString, matchCase, matchWholeWord, matchFunc, progress, cancellationTokenSource);

        public ITableSearcher GetTableSearcher(string sourceFile, DataSet dataSet, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getTableSearcher(sourceFile, dataSet, searchString, matchCase, matchWholeWord, matchFunc, progress, cancellationTokenSource);
    }
}
