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
    internal class DeploySelectedFilesToApiCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedRulesResourcesPairs _tryGetSelectedRulesResourcesPairs;
        private readonly IDeploySelectedFilesToApi _deploySelectedFilesToApi;
        private readonly MDIParent mdiParent;
        private readonly string applicationName;

        public DeploySelectedFilesToApiCommand(IConfigurationService configurationService, ITryGetSelectedRulesResourcesPairs tryGetSelectedRulesResourcesPairs, IDeploySelectedFilesToApi deploySelectedFilesToApi, MDIParent mdiParent, string applicationName)
        {
            _configurationService = configurationService;
            _tryGetSelectedRulesResourcesPairs = tryGetSelectedRulesResourcesPairs;
            _deploySelectedFilesToApi = deploySelectedFilesToApi;
            this.mdiParent = mdiParent;
            this.applicationName = applicationName;
        }

        public override async void Execute()
        {
            if (
                    !_tryGetSelectedRulesResourcesPairs.Try
                    (
                        applicationName,
                        Strings.selectRulesToDeploy,
                        out IList<RulesResourcesPair> sourceFiles,
                        this.mdiParent
                    )
               )
            {
                return;
            }

            await this.mdiParent.RunAsync(DeploySelectedRules);

            Task DeploySelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deploySelectedFilesToApi.Deploy
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
