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
    internal class DeploySelectedFilesToApiCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedRulesResourcesPairs _tryGetSelectedRulesResourcesPairs;
        private readonly IDeploySelectedFilesToApi _deploySelectedFilesToApi;
        private readonly IMainWindow _mainWindow;
        private readonly string applicationName;

        public DeploySelectedFilesToApiCommand(
            IConfigurationService configurationService,
            ITryGetSelectedRulesResourcesPairs tryGetSelectedRulesResourcesPairs,
            IDeploySelectedFilesToApi deploySelectedFilesToApi,
            IMainWindow mainWindow,
            string applicationName)
        {
            _configurationService = configurationService;
            _tryGetSelectedRulesResourcesPairs = tryGetSelectedRulesResourcesPairs;
            _deploySelectedFilesToApi = deploySelectedFilesToApi;
            _mainWindow = mainWindow;
            this.applicationName = applicationName;
        }

        public override async void Execute()
        {
            if (
                    !_tryGetSelectedRulesResourcesPairs.Try
                    (
                        applicationName,
                        Strings.selectRulesToDeploy,
                        out IList<RulesResourcesPair> sourceFiles
                    )
               )
            {
                return;
            }

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            await mdiParent.RunAsync(DeploySelectedRules);

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
