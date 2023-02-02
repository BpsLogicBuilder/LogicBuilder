using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SearcherFactoryServices
    {
        internal static IServiceCollection AddSearcherFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Document, string, bool, bool, Func<string, string, bool, bool, IList<string>>, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramSearcher>>
                (
                    provider =>
                    (sourceFile, document, searchString, matchCase, matchWholeWord, matchFunc, progress, cancellationTokenSource) => new DiagramSearcher
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<IShapeXmlHelper>(),
                        provider.GetRequiredService<IStructuresFactory>(),
                        sourceFile,
                        document,
                        searchString,
                        matchCase,
                        matchWholeWord,
                        matchFunc,
                        progress,
                        cancellationTokenSource)
                )
                .AddTransient<ISearcherFactory, SearcherFactory>()
                .AddTransient<Func<string, DataSet, string, bool, bool, Func<string, string, bool, bool, IList<string>>, IProgress<ProgressMessage>, CancellationTokenSource, ITableSearcher>>
                (
                    provider =>
                    (sourceFile, dataSet, searchString, matchCase, matchWholeWord, matchFunc, progress, cancellationTokenSource) => new TableSearcher
                    (
                        provider.GetRequiredService<ICellXmlHelper>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<IStructuresFactory>(),
                        sourceFile,
                        dataSet,
                        searchString,
                        matchCase,
                        matchWholeWord,
                        matchFunc,
                        progress,
                        cancellationTokenSource
                    )
                );
        }
    }
}
