using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface ITableSearcher
    {
        Task<SearchTableResults> Search(string sourceFile, DataSet dataSet, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
    }
}
