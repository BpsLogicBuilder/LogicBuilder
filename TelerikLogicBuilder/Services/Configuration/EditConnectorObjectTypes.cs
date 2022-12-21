using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class EditConnectorObjectTypes : IEditConnectorObjectTypes
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IUiNotificationService _uiNotificationService;

        public EditConnectorObjectTypes(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            ILoadProjectProperties loadProjectProperties,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            IUiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _loadProjectProperties = loadProjectProperties;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _uiNotificationService = uiNotificationService;
        }

        public async void Edit()
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

                _mainWindow.MDIParent.ChangeCursor(Cursors.WaitCursor);
                await _mainWindow.MDIParent.RunLoadContextAsync(ConfigureConnectorObjects);
                _mainWindow.MDIParent.ChangeCursor(Cursors.Default);

                Task ConfigureConnectorObjects(CancellationTokenSource cancellationTokenSource)
                {//?Update Project Properties in memory
                    using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
                    IConfigureConnectorObjectsForm configureConnectorObjectsForm = disposableManager.GetConfigureConnectorObjectsForm(openedReadonly);
                    configureConnectorObjectsForm.ShowDialog(_mainWindow.Instance);
                    if (!openedReadonly && configureConnectorObjectsForm.DialogResult == DialogResult.OK)
                    {
                        _configurationService.ProjectProperties = _loadProjectProperties.Load(projectFileFullName);
                    }

                    return Task.CompletedTask;
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}
