using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
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
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedDocuments _tryGetSelectedDocuments;
        private readonly IBuildSaveAssembleRulesForSelectedDocuments _buildSaveConsolidateSelectedDocumentRules;
        private readonly UiNotificationService _uiNotificationService;
        private readonly MDIParent mdiParent;

        public BuildSaveConsolidateSelectedDocumentsCommand(
            IConfigurationService configurationService,
            ITryGetSelectedDocuments tryGetSelectedDocuments,
            IBuildSaveAssembleRulesForSelectedDocuments buildSaveConsolidateSelectedDocumentRules,
            UiNotificationService uiNotificationService,
            MDIParent mdiParent)
        {
            _configurationService = configurationService;
            _tryGetSelectedDocuments = tryGetSelectedDocuments;
            _buildSaveConsolidateSelectedDocumentRules = buildSaveConsolidateSelectedDocumentRules;
            _uiNotificationService = uiNotificationService;
            this.mdiParent = mdiParent;
        }

        public override async void Execute()
        {
            if (!_tryGetSelectedDocuments.Try(out IList<string> sourceFiles, this.mdiParent))
                return;

            await this.mdiParent.RunLoadContextAsync(BuildSelectedDocumentRules);

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
