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
        private readonly IValidateSelectedDocuments _validateSelectedDocuments;
        private readonly IMDIParent mdiParent;

        public ValidateActiveDocumentCommand(IConfigurationService configurationService, IValidateSelectedDocuments validateSelectedDocuments, IMDIParent mdiParent)
        {
            _configurationService = configurationService;
            _validateSelectedDocuments = validateSelectedDocuments;
            this.mdiParent = mdiParent;
        }

        public override async void Execute()
        {
            if (this.mdiParent.EditControl == null)
                return;

            this.mdiParent.EditControl.Save();

            await this.mdiParent.RunLoadContextAsync(ValidateSelectedDocuments);

            Task ValidateSelectedDocuments(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _validateSelectedDocuments.Validate
                (
                    new string[] { this.mdiParent.EditControl.SourceFile },
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
