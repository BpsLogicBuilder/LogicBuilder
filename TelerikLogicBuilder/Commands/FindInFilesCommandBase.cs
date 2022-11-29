using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal abstract class FindInFilesCommandBase<TFindInFilesForm> : ClickCommandBase where TFindInFilesForm : IFindInFilesForm
    {
        private readonly ICheckVisioConfiguration _checkVisioConfiguration;
        private readonly IGetSourceFilesForDocumentSearch _getSourceFilesForDocumentSearch;
        private readonly IMainWindow _mainWindow;
        protected readonly ISearchFunctions _searchFunctions;
        private readonly ISearchSelectedDocuments _searchSelectedDocuments;

        public FindInFilesCommandBase(
            ICheckVisioConfiguration checkVisioConfiguration,
            IGetSourceFilesForDocumentSearch getSourceFilesForDocumentSearch,
            IMainWindow mainWindow,
            ISearchFunctions searchFunctions,
            ISearchSelectedDocuments searchSelectedDocuments)
        {
            _checkVisioConfiguration = checkVisioConfiguration;
            _getSourceFilesForDocumentSearch = getSourceFilesForDocumentSearch;
            _mainWindow = mainWindow;
            _searchFunctions = searchFunctions;
            _searchSelectedDocuments = searchSelectedDocuments;
        }

        protected abstract Func<string, string, bool, bool, IList<string>> MatchFunc { get; } 

        public async override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            using IScopedDisposableManager<TFindInFilesForm> findTextDisposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TFindInFilesForm>>();
            TFindInFilesForm findTextInFiles = findTextDisposableManager.ScopedService;
            DialogResult result = findTextInFiles.ShowDialog(_mainWindow.Instance);
            if (result != DialogResult.OK)
                return;

            await mdiParent.RunAsync(SearchSelectedDocuments);

            Task SearchSelectedDocuments(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            {
                IList<string> configErrors = _checkVisioConfiguration.Check();
                if (configErrors.Count > 0)
                {
                    using IScopedDisposableManager<TextViewer> textViewerDisposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
                    TextViewer textViewer = textViewerDisposableManager.ScopedService;
                    textViewer.SetText(configErrors.ToArray());
                    textViewer.ShowDialog(_mainWindow.Instance);
                    return Task.FromResult(Array.Empty<ResultMessage>());
                }

                return _searchSelectedDocuments.Search
                (
                    _getSourceFilesForDocumentSearch.GetSourceFiles
                    (
                        findTextInFiles.SearchPattern,
                        findTextInFiles.SearchType
                    ),
                    findTextInFiles.SearchString,
                    findTextInFiles.MatchCase,
                    findTextInFiles.MatchWholeWord,
                    MatchFunc,
                    progress,
                    cancellationTokenSource
                );
            }
        }
    }
}
