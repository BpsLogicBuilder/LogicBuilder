using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface ISearchSelectedDocuments
    {
        Task<IList<ResultMessage>> Search(
            IList<string> sourceFiles,
            string searchString, 
            bool matchCase, 
            bool matchWholeWord, 
            Func<string, string, bool, bool, IList<string>> matchFunc,
            IProgress<ProgressMessage> progress, 
            CancellationTokenSource cancellationTokenSource);
    }
}
