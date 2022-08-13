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
    internal class ValidateSelectedDocumentsCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMainWindow _mainWindow;
        private readonly ITryGetSelectedDocuments _tryGetSelectedDocuments;
        private readonly IValidateSelectedDocuments _validateSelectedDocuments;

        public ValidateSelectedDocumentsCommand(
            IConfigurationService configurationService,
            IMainWindow mainWindow,
            ITryGetSelectedDocuments tryGetSelectedDocuments,
            IValidateSelectedDocuments validateSelectedDocuments)
        {
            _configurationService = configurationService;
            _mainWindow = mainWindow;
            _tryGetSelectedDocuments = tryGetSelectedDocuments;
            _validateSelectedDocuments = validateSelectedDocuments;
        }

        public async override void Execute()
        {
            if (!_tryGetSelectedDocuments.Try(out IList<string> sourceFiles))
                return;

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            await mdiParent.RunLoadContextAsync(ValidateSelectedDocuments);

            Task ValidateSelectedDocuments(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _validateSelectedDocuments.Validate
                (
                    sourceFiles,
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
