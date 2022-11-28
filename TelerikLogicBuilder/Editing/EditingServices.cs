using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;
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
    internal static class EditingServices
    {
        internal static IServiceCollection AddEditingControls(this IServiceCollection services)
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
                .AddTransient<IDocumentEditorFactory, DocumentEditorFactory>()
                .AddSingleton<IFindAndReplaceHelper, FindAndReplaceHelper>()
                .AddTransient<FindCell>()
                .AddTransient<IFindConstructorInCell, FindConstructorInCell>()
                .AddTransient<IFindConstructorInFiles, FindConstructorInFiles>()
                .AddTransient<FindConstructorInShape>()
                .AddTransient<IFindFunctionInCell, FindFunctionInCell>()
                .AddTransient<IFindFunctionInFiles, FindFunctionInFiles>()
                .AddTransient<FindFunctionInShape>()
                .AddTransient<FindShape>()
                .AddTransient<IFindTextInCell, FindTextInCell>()
                .AddTransient<IFindTextInFiles, FindTextInFiles>()
                .AddTransient<FindTextInShape>()
                .AddTransient<FindReplaceConstructorInCell>()
                .AddTransient<FindReplaceConstructorInShape>()
                .AddTransient<FindReplaceFunctionInCell>()
                .AddTransient<FindReplaceFunctionInShape>()
                .AddTransient<FindReplaceTextInCell>()
                .AddTransient<FindReplaceTextInShape>()
                .AddTransient<FindReplaceVariableInCell>()
                .AddTransient<FindReplaceVariableInShape>()
                .AddTransient<IFindVariableInCell, FindVariableInCell>()
                .AddTransient<IFindVariableInFiles, FindVariableInFiles>()
                .AddTransient<FindVariableInShape>()
                .AddSingleton<IFunctionsFormFieldSetHelper, FunctionsFormFieldSetHelper>()
                .AddSingleton<IGetSourceFilesForDocumentSearch, GetSourceFilesForDocumentSearch>()
                .AddTransient<ISearcherFactory, SearcherFactory>()
                .AddSingleton<ISearchFunctions, SearchFunctions>()
                .AddSingleton<ISearchSelectedDocuments, SearchSelectedDocuments>()
                .AddTransient<Func<string, bool, TableControl>>
                (
                    provider =>
                    (tableSourceFile, openedAsReadOnly) => ActivatorUtilities.CreateInstance<TableControl>
                    (
                        provider,
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        tableSourceFile,
                        openedAsReadOnly
                    )
                )
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
                        cancellationTokenSource)
                )
                .AddTransient<Func<string, bool, VisioControl>>
                (
                    provider =>
                    (visioSourceFile, openedAsReadOnly) => ActivatorUtilities.CreateInstance<VisioControl>
                    (
                        provider,
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        visioSourceFile,
                        openedAsReadOnly
                    )
                );
        }
    }
}
