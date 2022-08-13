using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ValidateActiveDocumentCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMainWindow _mainWindow;
        private readonly IValidateSelectedDocuments _validateSelectedDocuments;

        public ValidateActiveDocumentCommand(
            IConfigurationService configurationService,
            IMainWindow mainWindow,
            IValidateSelectedDocuments validateSelectedDocuments)
        {
            _configurationService = configurationService;
            _mainWindow = mainWindow;
            _validateSelectedDocuments = validateSelectedDocuments;
        }

        public override async void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            if (mdiParent.EditControl == null)
                return;

            mdiParent.EditControl.Save();

            await mdiParent.RunLoadContextAsync(ValidateSelectedDocuments);

            Task ValidateSelectedDocuments(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _validateSelectedDocuments.Validate
                (
                    new string[] { mdiParent.EditControl.SourceFile },
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
