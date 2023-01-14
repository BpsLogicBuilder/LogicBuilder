using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class EditConstructors : IEditConstructors
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IUiNotificationService _uiNotificationService;
        private readonly IConstructorListInitializer _constructorListInitializer;

        public EditConstructors(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            IUiNotificationService uiNotificationService,
            IConstructorListInitializer constructorListInitializer)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _uiNotificationService = uiNotificationService;
            _constructorListInitializer = constructorListInitializer;
        }

        public async void Edit()
        {
            try
            {
                string fullName = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ConfigurationFiles.Constructors);
                FileInfo fileInfo = _fileIOHelper.GetNewFileInfo(fullName);
                bool openedReadonly = false;

                if (fileInfo.IsReadOnly)
                {
                    DialogResult dialogResult = DisplayMessage.ShowYesNoCancel
                    (
                        _mainWindow.Instance,
                        string.Format(CultureInfo.CurrentCulture, Strings.fileNotWriteableFormat, ConfigurationFiles.Constructors),
                        _mainWindow.RightToLeft
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        _fileIOHelper.SetWritable(fullName, true);
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
                        throw _exceptionHelper.CriticalException("{C3BBE23B-7E11-4914-A723-347DE88A53EE}");
                    }
                }

                _mainWindow.MDIParent.ChangeCursor(Cursors.WaitCursor);
                await _mainWindow.MDIParent.RunLoadContextAsync(Configure);
                _mainWindow.MDIParent.ChangeCursor(Cursors.Default);

                Task Configure(CancellationTokenSource cancellationTokenSource)
                {
                    using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
                    IConfigureConstructorsForm configureConstructorsForm = disposableManager.GetConfigureConstructorsForm(openedReadonly);
                    configureConstructorsForm.ShowDialog(_mainWindow.Instance);

                    if (!openedReadonly && configureConstructorsForm.DialogResult == DialogResult.OK)
                        _configurationService.ConstructorList = _constructorListInitializer.InitializeList();

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
