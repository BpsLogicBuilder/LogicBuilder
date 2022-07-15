﻿using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class DeleteSelectedFilesFromApiCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedRulesResourcesPairs _tryGetSelectedRulesResourcesPairs;
        private readonly IDeleteSelectedFilesFromApi _deleteSelectedFilesFromApi;
        private readonly MDIParent mdiParent;
        private readonly string applicationName;

        public DeleteSelectedFilesFromApiCommand(IConfigurationService configurationService, ITryGetSelectedRulesResourcesPairs tryGetSelectedRulesResourcesPairs, IDeleteSelectedFilesFromApi deleteSelectedFilesFromApi, MDIParent mdiParent, string applicationName)
        {
            _configurationService = configurationService;
            _tryGetSelectedRulesResourcesPairs = tryGetSelectedRulesResourcesPairs;
            _deleteSelectedFilesFromApi = deleteSelectedFilesFromApi;
            this.mdiParent = mdiParent;
            this.applicationName = applicationName;
        }

        public override async void Execute()
        {
            if (
                    !_tryGetSelectedRulesResourcesPairs.Try
                    (
                        applicationName,
                        Strings.selectRulesToDelete,
                        out IList<RulesResourcesPair> sourceFiles,
                        this.mdiParent
                    )
               )
            {
                return;
            }

            await this.mdiParent.RunAsync(DeleteSelectedRules);

            Task DeleteSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deleteSelectedFilesFromApi.Delete
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }
    }
}