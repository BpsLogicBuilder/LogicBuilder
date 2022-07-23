using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers
{
    internal class ValidateCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;
        private readonly IValidateSelectedRules _validateSelectedRules;
        private readonly UiNotificationService _uiNotificationService;
        private readonly IRulesExplorer _rulesExplorer;

        public ValidateCommand(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            ITreeViewService treeViewService,
            IValidateSelectedRules validateSelectedRules,
            UiNotificationService uiNotificationService,
            IRulesExplorer rulesExplorer)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _treeViewService = treeViewService;
            _validateSelectedRules = validateSelectedRules;
            _uiNotificationService = uiNotificationService;
            _rulesExplorer = rulesExplorer;
        }

        public async override void Execute()
        {
            try
            {
                RadTreeNode? selectedNode = _rulesExplorer.TreeView.SelectedNode;
                if (selectedNode == null)
                    return;

                if (!_treeViewService.IsFileNode(selectedNode))
                    throw _exceptionHelper.CriticalException("{75F9A36B-A8AD-4A1F-B801-323869177A66}");

                if (!selectedNode.Name.EndsWith(FileExtensions.RULESFILEEXTENSION, true, CultureInfo.InvariantCulture))
                    throw _exceptionHelper.CriticalException("{ECCE6846-055F-4528-81E1-5FE67EA087D5}");

                IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

                await mdiParent.RunLoadContextAsync(ValidateSelectedRules);

                Task ValidateSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                    => _validateSelectedRules.Validate
                    (
                        new string[] { selectedNode.Name },
                        _configurationService.GetApplicationFromPath(selectedNode.Name),
                        progress,
                        cancellationTokenSource
                    );
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}
