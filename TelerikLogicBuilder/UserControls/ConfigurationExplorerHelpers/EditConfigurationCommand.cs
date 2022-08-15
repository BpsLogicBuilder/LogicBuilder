using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers
{
    internal class EditConfigurationCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditConstructors _editConstructors;
        private readonly IEditFragments _editFragments;
        private readonly IEditFunctions _editFunctions;
        private readonly IEditProjectProperties _editProjectProperties;
        private readonly IEditVariables _editVariables;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public EditConfigurationCommand(
            IConfigurationService configurationService,
            IEditConstructors editConstructors,
            IEditFragments editFragments,
            IEditFunctions editFunctions,
            IEditProjectProperties editProjectProperties,
            IEditVariables editVariables,
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            
            _configurationService = configurationService;
            _editConstructors = editConstructors;
            _editFragments = editFragments;
            _editFunctions = editFunctions;
            _editProjectProperties = editProjectProperties;
            _editVariables = editVariables;
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public async override void Execute()
        {
            RadTreeNode? selectedNode = _mainWindow.ConfigurationExplorer.TreeView.SelectedNode;
            if (selectedNode == null)
                return;

            if (!_treeViewService.IsFileNode(selectedNode))
                throw _exceptionHelper.CriticalException("{AE031350-BABD-426C-B9CF-CC99FD9A7611}");

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;

            string fileName = _pathHelper.GetFileName(selectedNode.Name).ToLowerInvariant();
            if(IsProjectPropertiesFile())
            {
                EditProjectProperties();
                return;
            }

            await mdiParent.RunLoadContextAsync(Edit);

            Task Edit(CancellationTokenSource cancellationTokenSource)
            {
                switch (fileName)
                {
                    case ConfigurationFiles.Constructors:
                        _editConstructors.Edit();
                        break;
                    case ConfigurationFiles.Fragments:
                        _editFragments.Edit();
                        break;
                    case ConfigurationFiles.Functions:
                        _editFunctions.Edit();
                        break;
                    case ConfigurationFiles.Variables:
                        _editVariables.Edit();
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{4BD1B450-3745-4543-B3FE-063A06E294D8}");
                }
                return Task.CompletedTask;
            }

            bool IsProjectPropertiesFile()
                => string.Compare
                (
                    fileName,
                    $"{_configurationService.ProjectProperties.ProjectName}{FileExtensions.PROJECTFILEEXTENSION}",
                    true,
                    CultureInfo.InvariantCulture
                ) == 0;
        }

        private void EditProjectProperties()
        {
            try
            {
                _editProjectProperties.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}
