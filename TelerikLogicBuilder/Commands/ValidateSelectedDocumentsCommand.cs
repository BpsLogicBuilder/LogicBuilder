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
    internal class ValidateSelectedDocumentsCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedDocuments _tryGetSelectedDocuments;
        private readonly IValidateSelectedDocuments _validateSelectedDocuments;
        private readonly MDIParent mdiParent;

        public ValidateSelectedDocumentsCommand(IConfigurationService configurationService, ITryGetSelectedDocuments tryGetSelectedDocuments, IValidateSelectedDocuments validateSelectedDocuments, MDIParent mdiParent)
        {
            _configurationService = configurationService;
            _tryGetSelectedDocuments = tryGetSelectedDocuments;
            _validateSelectedDocuments = validateSelectedDocuments;
            this.mdiParent = mdiParent;
        }

        public async override void Execute()
        {
            if (!_tryGetSelectedDocuments.Try(out IList<string> sourceFiles, this.mdiParent))
                return;

            await this.mdiParent.RunLoadContextAsync(ValidateSelectedDocuments);

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
