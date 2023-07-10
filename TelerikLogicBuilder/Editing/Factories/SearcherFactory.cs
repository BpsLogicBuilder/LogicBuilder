using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class SearcherFactory : ISearcherFactory
    {
        public IDiagramSearcher GetDiagramSearcher(string sourceFile, Document document, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => new DiagramSearcher
            (
                Program.ServiceProvider.GetRequiredService<IPathHelper>(),
                Program.ServiceProvider.GetRequiredService<IResultMessageBuilder>(),
                Program.ServiceProvider.GetRequiredService<IShapeXmlHelper>(),
                Program.ServiceProvider.GetRequiredService<IStructuresFactory>(),
                sourceFile,
                document,
                searchString,
                matchCase,
                matchWholeWord,
                matchFunc,
                progress,
                cancellationTokenSource
            );

        public ITableSearcher GetTableSearcher(string sourceFile, DataSet dataSet, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => new TableSearcher
            (
                Program.ServiceProvider.GetRequiredService<ICellXmlHelper>(),
                Program.ServiceProvider.GetRequiredService<IPathHelper>(),
                Program.ServiceProvider.GetRequiredService<IResultMessageBuilder>(),
                Program.ServiceProvider.GetRequiredService<IStructuresFactory>(),
                sourceFile,
                dataSet,
                searchString,
                matchCase,
                matchWholeWord,
                matchFunc,
                progress,
                cancellationTokenSource
            );
    }
}
