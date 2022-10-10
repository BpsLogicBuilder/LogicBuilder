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
    internal class DeploySelectedFilesToFileSystemCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedRulesResourcesPairs _tryGetSelectedRulesResourcesPairs;
        private readonly IDeploySelectedFilesToFileSystem _deploySelectedFilesToFileSystem;
        private readonly IMainWindow _mainWindow;
        private readonly string applicationName;

        public DeploySelectedFilesToFileSystemCommand(
            IConfigurationService configurationService,
            ITryGetSelectedRulesResourcesPairs tryGetSelectedRulesResourcesPairs,
            IDeploySelectedFilesToFileSystem deploySelectedFilesToFileSystem,
            IMainWindow mainWindow,
            string applicationName)
        {
            _configurationService = configurationService;
            _tryGetSelectedRulesResourcesPairs = tryGetSelectedRulesResourcesPairs;
            _deploySelectedFilesToFileSystem = deploySelectedFilesToFileSystem;
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
                => _deploySelectedFilesToFileSystem.Deploy
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
