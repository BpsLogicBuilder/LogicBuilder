﻿using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
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
    internal class ValidateSelectedRulesCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITryGetSelectedRules _tryGetSelectedRules;
        private readonly IValidateSelectedRules _validateSelectedRules;
        private readonly IMainWindow _mainWindow;
        private readonly string applicationName;

        public ValidateSelectedRulesCommand(
            IConfigurationService configurationService,
            ITryGetSelectedRules tryGetSelectedRules,
            IValidateSelectedRules validateSelectedRules,
            IMainWindow mainWindow,
            string applicationName)
        {
            _configurationService = configurationService;
            _tryGetSelectedRules = tryGetSelectedRules;
            _validateSelectedRules = validateSelectedRules;
            _mainWindow = mainWindow;
            this.applicationName = applicationName;
        }

        public async override void Execute()
        {
            if (!_tryGetSelectedRules.Try(applicationName, Strings.selectRulesToValidate, out IList<string> sourceFiles))
                return;

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            await mdiParent.RunLoadContextAsync(ValidateSelectedRules);

            Task ValidateSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _validateSelectedRules.Validate
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }
    }
}
