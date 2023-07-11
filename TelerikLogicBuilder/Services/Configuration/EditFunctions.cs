using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
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
    internal class EditFunctions : IEditFunctions
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IUiNotificationService _uiNotificationService;

        public EditFunctions(
            IConfigurationService configurationService,
            IConstructorListInitializer constructorListInitializer,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IFunctionListInitializer functionListInitializer,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            IUiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _constructorListInitializer = constructorListInitializer;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _functionListInitializer = functionListInitializer;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _uiNotificationService = uiNotificationService;
        }

        public async void Edit()
        {
            try
            {
                string fullName = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ConfigurationFiles.Functions);
                FileInfo fileInfo = _fileIOHelper.GetNewFileInfo(fullName);
                bool openedReadonly = false;

                if (fileInfo.IsReadOnly)
                {
                    DialogResult dialogResult = DisplayMessage.ShowYesNoCancel
                    (
                        _mainWindow.Instance,
                        string.Format(CultureInfo.CurrentCulture, Strings.fileNotWriteableFormat, ConfigurationFiles.Functions),
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
                        throw _exceptionHelper.CriticalException("{709F9C4A-64A3-4CC4-9024-A22CF7D9AA06}");
                    }
                }

                await _mainWindow.MDIParent.RunLoadContextAsync(Configure);

                Task Configure(CancellationTokenSource cancellationTokenSource)
                {
                    _mainWindow.MDIParent.ChangeCursor(Cursors.WaitCursor);
                    IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
                    using IConfigureFunctionsForm configureFunctionsForm = disposableManager.GetConfigureFunctionsForm(openedReadonly);
                    configureFunctionsForm.ShowDialog(_mainWindow.Instance);
                    _mainWindow.MDIParent.ChangeCursor(Cursors.Default);

                    if (!openedReadonly && configureFunctionsForm.DialogResult == DialogResult.OK)
                    {
                        _configurationService.FunctionList = _functionListInitializer.InitializeList();
                        _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
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
