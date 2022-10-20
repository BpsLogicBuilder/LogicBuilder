using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface ISearcherFactory
    {
        IDiagramSearcher GetDiagramSearcher(string sourceFile,
            Document document,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource);

        ITableSearcher GetTableSearcher(string sourceFile,
            DataSet dataSet,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource);
    }
}
