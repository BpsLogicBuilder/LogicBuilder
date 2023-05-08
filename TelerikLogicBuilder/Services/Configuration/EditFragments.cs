using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
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
    internal class EditFragments : IEditFragments
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IUiNotificationService _uiNotificationService;
        private readonly IFragmentListInitializer _fragmentListInitializer;

        public EditFragments(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            IUiNotificationService uiNotificationService,
            IFragmentListInitializer fragmentListInitializer)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _uiNotificationService = uiNotificationService;
            _fragmentListInitializer = fragmentListInitializer;
        }

        public async void Edit()
        {
            try
            {
                string fullName = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ConfigurationFiles.Fragments);
                FileInfo fileInfo = _fileIOHelper.GetNewFileInfo(fullName);
                bool openedReadonly = false;
                if (fileInfo.IsReadOnly)
                {
                    DialogResult dialogResult = DisplayMessage.ShowYesNoCancel
                    (
                        _mainWindow.Instance,
                        string.Format(CultureInfo.CurrentCulture, Strings.fileNotWriteableFormat, ConfigurationFiles.Fragments),
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
                        throw _exceptionHelper.CriticalException("{6998DE36-A436-462E-A498-37C08C98FE91}");
                    }
                }

                await _mainWindow.MDIParent.RunLoadContextAsync(Configure);
                
                Task Configure(CancellationTokenSource cancellationTokenSource)
                {
                    _mainWindow.MDIParent.ChangeCursor(Cursors.WaitCursor);
                    using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
                    IConfigureFragmentsForm configureFragmentsForm = disposableManager.GetConfigureFragmentsForm(openedReadonly);
                    configureFragmentsForm.ShowDialog(_mainWindow.Instance);
                    _mainWindow.MDIParent.ChangeCursor(Cursors.Default);

                    if (!openedReadonly && configureFragmentsForm.DialogResult == DialogResult.OK)
                        _configurationService.FragmentList = _fragmentListInitializer.InitializeList();

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
