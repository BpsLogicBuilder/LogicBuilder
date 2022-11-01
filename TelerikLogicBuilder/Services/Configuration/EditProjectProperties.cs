using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class EditProjectProperties : IEditProjectProperties
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IPathHelper _pathHelper;
        private readonly IUiNotificationService _uiNotificationService;

        public EditProjectProperties(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            ILoadProjectProperties loadProjectProperties,
            IPathHelper pathHelper,
            IUiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _loadProjectProperties = loadProjectProperties;
            _pathHelper = pathHelper;
            _uiNotificationService = uiNotificationService;
        }

        public void Edit()
        {
            try
            {
                string projectFileFullName = _configurationService.ProjectProperties.ProjectFileFullName;
                FileInfo fileInfo = _fileIOHelper.GetNewFileInfo(projectFileFullName);
                bool openedReadonly = false;
                if (fileInfo.IsReadOnly)
                {
                    DialogResult dialogResult = DisplayMessage.ShowYesNoCancel
                    (
                        _mainWindow.Instance,
                        string.Format(CultureInfo.CurrentCulture, Strings.fileNotWriteableFormat, _pathHelper.GetFileName(projectFileFullName)),
                        _mainWindow.RightToLeft
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        _fileIOHelper.SetWritable(projectFileFullName, true);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        openedReadonly = true;
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        throw _exceptionHelper.CriticalException("{583F4E4F-6881-4612-BB65-FF106B2DDDA1}");
                    }

                }

                using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
                ConfigureProjectProperties configureProjectProperties = disposableManager.GetConfigureProjectProperties(openedReadonly);
                configureProjectProperties.ShowDialog(_mainWindow.Instance);

                if (!openedReadonly && configureProjectProperties.DialogResult == DialogResult.OK)
                {
                    _configurationService.ProjectProperties = _loadProjectProperties.Load(projectFileFullName);
                    _mainWindow.MDIParent.RunLoadContextAsync(PostConfiguration);
                }

                Task PostConfiguration(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                {
                    IMDIParent mdiParent = _mainWindow.MDIParent;
                    mdiParent.UpdateApplicationMenuItems();
                    mdiParent.SetButtonStates(true);
                    mdiParent.ProjectExplorer.CreateProfile();
                    return Task.CompletedTask;
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                _mainWindow.DocumentsExplorer.RefreshTreeView();
            }
        }
    }
}
