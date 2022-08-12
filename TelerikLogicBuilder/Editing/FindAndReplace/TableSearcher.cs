using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class TableSearcher : ITableSearcher
    {
        private readonly IContextProvider _contextProvider;
        private readonly ICellXmlHelper _cellXmlHelper;

        public TableSearcher(IContextProvider contextProvider, ICellXmlHelper cellXmlHelper)
        {
            _contextProvider = contextProvider;
            _cellXmlHelper = cellXmlHelper;
        }

        public Task<SearchTableResults> Search(string sourceFile, DataSet dataSet, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => new TableSearcherUtility
            (
                sourceFile,
                dataSet,
                searchString,
                matchCase,
                matchWholeWord,
                matchFunc,
                progress,
                cancellationTokenSource,
                _contextProvider,
                _cellXmlHelper
            ).Search();
    }
}
