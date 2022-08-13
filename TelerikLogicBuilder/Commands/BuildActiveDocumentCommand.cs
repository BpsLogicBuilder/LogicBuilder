using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class BuildActiveDocumentCommand : ClickCommandBase
    {
        private readonly IBuildSaveAssembleRulesForSelectedDocuments _buildSaveConsolidateSelectedDocumentRules;
        private readonly IConfigurationService _configurationService;
        private readonly IMainWindow _mainWindow;
        private readonly UiNotificationService _uiNotificationService;

        public BuildActiveDocumentCommand(
            IBuildSaveAssembleRulesForSelectedDocuments buildSaveConsolidateSelectedDocumentRules,
            IConfigurationService configurationService,
            IMainWindow mainWindow,
            UiNotificationService uiNotificationService)
        {
            _buildSaveConsolidateSelectedDocumentRules = buildSaveConsolidateSelectedDocumentRules;
            _configurationService = configurationService;
            _mainWindow = mainWindow;
            _uiNotificationService = uiNotificationService;
        }

        public override async void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            if (mdiParent.EditControl == null)
                return;

            mdiParent.EditControl.Save();

            await mdiParent.RunLoadContextAsync(BuildSelectedDocumentRules);

            _uiNotificationService.RequestRulesExplorerRefresh(true);

            Task BuildSelectedDocumentRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _buildSaveConsolidateSelectedDocumentRules.BuildRules
                (
                    new string[] { mdiParent.EditControl.SourceFile },
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
