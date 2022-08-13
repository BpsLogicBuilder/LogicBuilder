using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class BuildSaveConsolidateSelectedDocumentsCommand : ClickCommandBase
    {
        private readonly IBuildSaveAssembleRulesForSelectedDocuments _buildSaveConsolidateSelectedDocumentRules;
        private readonly IConfigurationService _configurationService;
        private readonly IMainWindow _mainWindow;
        private readonly ITryGetSelectedDocuments _tryGetSelectedDocuments;
        private readonly UiNotificationService _uiNotificationService;

        public BuildSaveConsolidateSelectedDocumentsCommand(
            IBuildSaveAssembleRulesForSelectedDocuments buildSaveConsolidateSelectedDocumentRules,
            IConfigurationService configurationService,
            IMainWindow mainWindow,
            ITryGetSelectedDocuments tryGetSelectedDocuments,
            UiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _mainWindow = mainWindow;
            _tryGetSelectedDocuments = tryGetSelectedDocuments;
            _buildSaveConsolidateSelectedDocumentRules = buildSaveConsolidateSelectedDocumentRules;
            _uiNotificationService = uiNotificationService;
        }

        public override async void Execute()
        {
            if (!_tryGetSelectedDocuments.Try(out IList<string> sourceFiles))
                return;

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            await mdiParent.RunLoadContextAsync(BuildSelectedDocumentRules);

            _uiNotificationService.RequestRulesExplorerRefresh(true);

            Task BuildSelectedDocumentRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _buildSaveConsolidateSelectedDocumentRules.BuildRules
                (
                    sourceFiles,
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
