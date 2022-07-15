﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class BuildActiveDocumentCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IBuildSaveAssembleRulesForSelectedDocuments _buildSaveConsolidateSelectedDocumentRules;
        private readonly IMDIParent mdiParent;

        public BuildActiveDocumentCommand(IConfigurationService configurationService, IBuildSaveAssembleRulesForSelectedDocuments buildSaveConsolidateSelectedDocumentRules, IMDIParent mdiParent)
        {
            _configurationService = configurationService;
            _buildSaveConsolidateSelectedDocumentRules = buildSaveConsolidateSelectedDocumentRules;
            this.mdiParent = mdiParent;
        }

        public override async void Execute()
        {
            if (this.mdiParent.EditControl == null)
                return;

            this.mdiParent.EditControl.Save();

            await this.mdiParent.RunLoadContextAsync(BuildSelectedDocumentRules);

            Task BuildSelectedDocumentRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _buildSaveConsolidateSelectedDocumentRules.BuildRules
                (
                    new string[] { this.mdiParent.EditControl.SourceFile },
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }
    }
}