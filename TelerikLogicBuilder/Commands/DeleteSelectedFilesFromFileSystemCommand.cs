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
    internal class DeleteSelectedFilesFromFileSystemCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedRulesResourcesPairs _tryGetSelectedRulesResourcesPairs;
        private readonly IDeleteSelectedFilesFromFileSystem _deleteSelectedFilesFromFileSystem;
        private readonly IMainWindow _mainWindow;
        private readonly string applicationName;

        public DeleteSelectedFilesFromFileSystemCommand(
            IConfigurationService configurationService,
            ITryGetSelectedRulesResourcesPairs tryGetSelectedRulesResourcesPairs,
            IDeleteSelectedFilesFromFileSystem deleteSelectedFilesFromFileSystem,
            IMainWindow mainWindow,
            string applicationName)
        {
            _configurationService = configurationService;
            _tryGetSelectedRulesResourcesPairs = tryGetSelectedRulesResourcesPairs;
            _deleteSelectedFilesFromFileSystem = deleteSelectedFilesFromFileSystem;
            _mainWindow = mainWindow;
            this.applicationName = applicationName;
        }

        public override async void Execute()
        {
            if (
                    !_tryGetSelectedRulesResourcesPairs.Try
                    (
                        applicationName,
                        Strings.selectRulesToDelete,
                        out IList<RulesResourcesPair> sourceFiles
                    )
               )
            {
                return;
            }

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            await mdiParent.RunAsync(DeleteSelectedRules);

            Task DeleteSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deleteSelectedFilesFromFileSystem.Delete
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
