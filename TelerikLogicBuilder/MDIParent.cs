using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Commands.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : RadForm, IMDIParent
    {
        private readonly IApplicationCommandsFactory _applicationCommandsFactory;
        private readonly ICheckSelectedApplication _checkSelectedApplication;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly ICreateProjectProperties _createProjectProperties;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly ILoadContextSponsor _loadContextSponsor;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IMainWindow _mainWindow;
        private readonly IServiceFactory _serviceFactory;
        private readonly IThemeManager _themeManager;
        private readonly IVariableListInitializer _variableListInitializer;
        private readonly IUiNotificationService _uiNotificationService;

        private readonly BuildActiveDocumentCommand _buildActiveDocumentCommand;
        private readonly BuildSaveConsolidateSelectedDocumentsCommand _buildSaveConsolidateSelectedDocumentsCommand;
        private readonly CloseProjectCommand _closeProjectCommand;
        private readonly DeleteSelectionCommand _deleteSelectionCommand;
        private readonly DisplayIndexInformationCommand _displayIndexInformationCommand;
        private readonly EditActiveDocumentCommand _editActiveDocumentCommand;
        private readonly EditConnectorObjectTypesCommand _editConnectorObjectTypesCommand;
        private readonly EditConstructorsCommand _editConstructorsCommand;
        private readonly EditFragmentsCommand _editFragmentsCommand;
        private readonly EditFunctionsCommand _editFunctionsCommand;
        private readonly EditProjectPropertiesCommand _editProjectPropertiesCommand;
        private readonly EditVariablesCommand _editVariablesCommand;
        private readonly ExitCommand _exitCommand;
        private readonly FindConstructorCommand _findConstructorCommand;
        private readonly FindConstructorInFilesCommand _findConstructorInFilesCommand;
        private readonly FindCellCommand _findCellCommand;
        private readonly FindFunctionCommand _findFunctionCommand;
        private readonly FindFunctionInFilesCommand _findFunctionInFilesCommand;
        private readonly FindReplaceConstructorCommand _findReplaceConstructorCommand;
        private readonly FindReplaceFunctionCommand _findReplaceFunctionCommand;
        private readonly FindReplaceTextCommand _findReplaceTextCommand;
        private readonly FindReplaceVariableCommand _findReplaceVariableCommand;
        private readonly FindShapeCommand _findShapeCommand;
        private readonly FindTextCommand _findTextCommand;
        private readonly FindTextInFilesCommand _findTextInFilesCommand;
        private readonly FindVariableCommand _findVariableCommand;
        private readonly FindVariableInFilesCommand _findVariableInFilesCommand;
        private readonly NewProjectCommand _newProjectCommand;
        private readonly OpenProjectCommand _openProjectCommand;
        private readonly PageSetupCommand _pageSetupCommand;
        private readonly RedoCommand _redoCommand;
        private readonly SaveActiveDocumentCommand _saveActiveDocumentCommand;
        private readonly ShowAboutCommand _showAboutCommand;
        private readonly ShowHelpContentsCommand _showHelpContentsCommand;
        private readonly UndoCommand _undoCommand;
        private readonly ValidateActiveDocumentCommand _validateActiveDocumentCommand;
        private readonly ValidateSelectedDocumentsCommand _validateSelectedDocumentsCommand;
        private readonly ViewApplicationsStencilCommand _viewApplicationsStencilCommand;
        private readonly ViewFlowDiagramStencilCommand _viewFlowDiagramStencilCommand;
        private readonly ViewMessagesCommand _viewMessagesCommand;
        private readonly ViewPanAndZoomWindowCommand _viewPanAndZoomWindowCommand;
        private readonly ViewProjectExplorerCommand _viewProjectExplorerCommand;

        //controls
        private readonly IProjectExplorer _projectExplorer;
        private readonly IMessages _messages;

        public MDIParent(
            IApplicationCommandsFactory applicationCommandsFactory,
            ICheckSelectedApplication checkSelectedApplication,
            IConfigurationService configurationService,
            IConstructorListInitializer constructorListInitializer,
            ICreateProjectProperties createProjectProperties,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IFragmentListInitializer fragmentListInitializer,
            IFunctionListInitializer functionListInitializer,
            ILoadContextSponsor loadContextSponsor,
            ILoadProjectProperties loadProjectProperties,
            IMainWindow mainWindow,
            IServiceFactory serviceFactory,
            IThemeManager themeManager,
            IVariableListInitializer variableListInitializer,
            IUiNotificationService uiNotificationService,
            BuildActiveDocumentCommand buildActiveDocumentCommand,
            BuildSaveConsolidateSelectedDocumentsCommand buildSaveConsolidateSelectedDocumentsCommand,
            CloseProjectCommand closeProjectCommand,
            DeleteSelectionCommand deleteSelectionCommand,
            DisplayIndexInformationCommand displayIndexInformationCommand,
            EditActiveDocumentCommand editActiveDocumentCommand,
            EditConnectorObjectTypesCommand editConnectorObjectTypesCommand,
            EditConstructorsCommand editConstructorsCommand,
            EditFragmentsCommand editFragmentsCommand,
            EditFunctionsCommand editFunctionsCommand,
            EditProjectPropertiesCommand editProjectPropertiesCommand,
            EditVariablesCommand editVariablesCommand,
            ExitCommand exitCommand,
            FindConstructorCommand findConstructorCommand,
            FindConstructorInFilesCommand findConstructorInFilesCommand,
            FindCellCommand findCellCommand,
            FindFunctionCommand findFunctionCommand,
            FindFunctionInFilesCommand findFunctionInFilesCommand,
            FindReplaceConstructorCommand findReplaceConstructorCommand,
            FindReplaceFunctionCommand findReplaceFunctionCommand,
            FindReplaceTextCommand findReplaceTextCommand,
            FindReplaceVariableCommand findReplaceVariableCommand,
            FindShapeCommand findShapeCommand,
            FindTextCommand findTextommand,
            FindTextInFilesCommand findTextInFilesCommand,
            FindVariableCommand findVariableCommand,
            FindVariableInFilesCommand findVariableInFilesCommand,
            NewProjectCommand newProjectCommand,
            OpenProjectCommand openProjectCommand,
            PageSetupCommand pageSetupCommand,
            RedoCommand redoCommand,
            SaveActiveDocumentCommand saveActiveDocumentCommand,
            ShowAboutCommand showAboutCommand,
            ShowHelpContentsCommand showHelpContentsCommand,
            UndoCommand undoCommand,
            ValidateActiveDocumentCommand validateActiveDocumentCommand,
            ValidateSelectedDocumentsCommand validateSelectedDocumentsCommand,
            ViewApplicationsStencilCommand viewApplicationsStencilCommand,
            ViewFlowDiagramStencilCommand viewFlowDiagramStencilCommand,
            ViewMessagesCommand viewMessagesCommand,
            ViewPanAndZoomWindowCommand viewPanAndZoomWindowCommand,
            ViewProjectExplorerCommand viewProjectExplorerCommand,
            IMessages messages,
            IProjectExplorer projectExplorer)
        {
            _applicationCommandsFactory = applicationCommandsFactory;
            _checkSelectedApplication = checkSelectedApplication;
            _configurationService = configurationService;
            _constructorListInitializer = constructorListInitializer;
            _createProjectProperties = createProjectProperties;
            _deleteSelectionCommand = deleteSelectionCommand;
            _displayIndexInformationCommand = displayIndexInformationCommand;
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _fragmentListInitializer = fragmentListInitializer;
            _functionListInitializer = functionListInitializer;
            _loadContextSponsor = loadContextSponsor;
            _loadProjectProperties = loadProjectProperties;
            _mainWindow = mainWindow;
            _serviceFactory = serviceFactory;
            _themeManager = themeManager;
            _variableListInitializer = variableListInitializer;
            _uiNotificationService = uiNotificationService;
            _closeProjectCommand = closeProjectCommand;
            _editActiveDocumentCommand = editActiveDocumentCommand;
            _editConnectorObjectTypesCommand = editConnectorObjectTypesCommand;
            _editConstructorsCommand = editConstructorsCommand;
            _editFragmentsCommand = editFragmentsCommand;
            _editFunctionsCommand = editFunctionsCommand;
            _editProjectPropertiesCommand = editProjectPropertiesCommand;
            _editVariablesCommand = editVariablesCommand;
            _exitCommand = exitCommand;
            _findConstructorCommand = findConstructorCommand;
            _findConstructorInFilesCommand = findConstructorInFilesCommand;
            _findCellCommand = findCellCommand;
            _findFunctionCommand = findFunctionCommand;
            _findFunctionInFilesCommand = findFunctionInFilesCommand;
            _findReplaceConstructorCommand = findReplaceConstructorCommand;
            _findReplaceFunctionCommand = findReplaceFunctionCommand;
            _findReplaceTextCommand = findReplaceTextCommand;
            _findReplaceVariableCommand = findReplaceVariableCommand;
            _findShapeCommand = findShapeCommand;
            _findTextCommand = findTextommand;
            _findTextInFilesCommand = findTextInFilesCommand;
            _findVariableCommand = findVariableCommand;
            _findVariableInFilesCommand = findVariableInFilesCommand;
            _newProjectCommand = newProjectCommand;
            _openProjectCommand = openProjectCommand;
            _pageSetupCommand = pageSetupCommand;
            _redoCommand = redoCommand;
            _saveActiveDocumentCommand = saveActiveDocumentCommand;
            _showAboutCommand = showAboutCommand;
            _showHelpContentsCommand = showHelpContentsCommand;
            _undoCommand = undoCommand;
            _validateActiveDocumentCommand = validateActiveDocumentCommand;
            _validateSelectedDocumentsCommand = validateSelectedDocumentsCommand;
            _viewApplicationsStencilCommand = viewApplicationsStencilCommand;
            _viewFlowDiagramStencilCommand = viewFlowDiagramStencilCommand;
            _viewMessagesCommand = viewMessagesCommand;
            _viewPanAndZoomWindowCommand = viewPanAndZoomWindowCommand;
            _viewProjectExplorerCommand = viewProjectExplorerCommand;

            _buildActiveDocumentCommand = buildActiveDocumentCommand;
            _buildSaveConsolidateSelectedDocumentsCommand = buildSaveConsolidateSelectedDocumentsCommand;
            _messages = messages;
            _projectExplorer = projectExplorer;

            documentExplorerErrorCountChangedSubscription = _uiNotificationService.DocumentExplorerErrorCountChangedSubject.Subscribe(DocumentExplorerErrorCountChanged);
            logicBuilderExceptionSubscription = _uiNotificationService.LogicBuilderExceptionSubject.Subscribe(LogicBuilderExceptionOccurred);

            InitializeComponent();
            Initialize();
        }

        private readonly IDictionary<string, RadMenuItem> deployFileSystemApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> deleteFileSystemApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> deployWebApiApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> deleteWebApiApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> recentProjectsMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> selectedApplicationRulesMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> validateApplicationRulesMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDisposable logicBuilderExceptionSubscription;
        private readonly IDisposable documentExplorerErrorCountChangedSubscription;

        public RadCommandBar CommandBar => radCommandBar1;
        public CommandBarButton CommandBarButtonSave => commandBarButtonSave;
        public RadMenuItem RadMenuItemDelete => radMenuItemDelete;
        public RadMenuItem RadMenuItemSave => radMenuItemSave;
        public RadMenuItem RadMenuItemUndo => radMenuItemUndo;
        public RadMenuItem RadMenuItemRedo => radMenuItemRedo;
        public RadMenuItem RadMenuItemIndexInformation => radMenuItemIndexInformation;
        public RadMenuItem RadMenuItemChaining => radMenuItemChaining;
        public RadMenuItem RadMenuItemToggleActivateAll => radMenuItemToggleActivateAll;
        public RadMenuItem RadMenuItemToggleReevaluateAll => radMenuItemToggleReevaluateAll;
        public RadMenuItem RadMenuItemFullChaining => radMenuItemFullChaining;
        public RadMenuItem RadMenuItemNoneChaining => radMenuItemNoneChaining;
        public RadMenuItem RadMenuItemUpdateOnlyChaining => radMenuItemUpdateOnlyChaining;

        public SplitPanel SplitPanelMessages => this.splitPanelMessages;
        public SplitPanel SplitPanelExplorer => this.splitPanelExplorer;

        public IDocumentEditor? EditControl { get; set; }
        public IMessages Messages => _messages;
        public IProjectExplorer ProjectExplorer => _projectExplorer;

        #region Methods
        public void AddTableControl(IDocumentEditor documentEditor)
            => AddDocumentEditorControl(documentEditor, false, true);

        public void AddVisioControl(IDocumentEditor documentEditor)
            => AddDocumentEditorControl(documentEditor, true, false);

        public void ChangeCursor(Cursor cursor)
        {
            this.Cursor = cursor;
            this.Refresh();
        }

        public void ClearProgressBar()
        {
            TaskComplete(string.Empty);
        }

        public void CloseProject()
        {
            this.EditControl?.Close();

            _projectExplorer.ClearProfile();
            _projectExplorer.Visible = false;
            _messages.Clear(MessageTab.Documents);
            _messages.Clear(MessageTab.Rules);
            _messages.Clear(MessageTab.Messages);
            _messages.Clear(MessageTab.PageSearchResults);
            _messages.Visible = false;
            SetButtonStates(false);
            UpdateRecentProjectMenuItems();
            ClearApplicationMenuItems();

            _loadContextSponsor.UnloadAssembliesOnCloseProject();
            _configurationService.ClearConfigurationData();

            this.Refresh();
        }

        public void CreateNewProject(string projectFileFullName)
        {
            if (File.Exists(projectFileFullName))
            {
                OpenProject(projectFileFullName);
                return;
            }

            _createProjectProperties.Create(projectFileFullName);
            _configurationService.ProjectProperties = _loadProjectProperties.Load(projectFileFullName);
            UpdateApplicationMenuItems();
            SetSelectedApplication
            (
                /*Gets the first one if none have been selected*/
                _configurationService.GetSelectedApplication().Name
            );

            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();

            SetButtonStates(true);
            _projectExplorer.CreateProfile();
            _projectExplorer.Visible = true;
            RecentFilesList.Add(projectFileFullName);
        }

        public async void OpenProject(string fullName)
        {
            if (!System.IO.File.Exists(fullName))
                return;

            this.Refresh();
            this.Cursor = Cursors.WaitCursor;
            UpdateProgress(20, Strings.progressFormTaskInitializing);

            _configurationService.ProjectProperties = _loadProjectProperties.Load(fullName);
            UpdateApplicationMenuItems();
            SetSelectedApplication
            (
                /*Gets the first one if none have been selected*/
                _configurationService.GetSelectedApplication().Name
            );

            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();

            UpdateProgress(50, Strings.progressFormTaskInitializing);

            await LoadAssembliesOnProjectOpen();

            //enable menu items prior to generating explorer
            //if explorer has errors, some menu items must be disabled
            SetButtonStates(true);

            UpdateProgress(60, Strings.progressFormTaskInitializing);

            _projectExplorer.CreateProfile();
            _projectExplorer.Visible = true;

            UpdateProgress(80, Strings.progressFormTaskInitializing);
            RecentFilesList.Add(fullName);

            UpdateProgress(100, Strings.progressFormTaskInitializing);
            TaskComplete(Strings.statusBarReadyMessage);
            this.Cursor = Cursors.Default;
        }

        public async Task RunAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
        {
            var progress = new Progress<ProgressMessage>(percent =>
            {
                radProgressBarElement1.Value1 = percent.Progress;
                radLabelElement1.Text = string.Format(CultureInfo.CurrentCulture, Strings.progressFormStatusMessageFormat2, percent.Message, percent.Progress);
            });

            var cancellationTokenSource = new CancellationTokenSource();
            using IProgressForm progressForm = _serviceFactory.GetProgressForm(progress, cancellationTokenSource);
            progressForm.Show(this);

            try
            {
                await task(progress, cancellationTokenSource);
                await Task.Delay(40);
                TaskComplete(Strings.statusBarReadyMessage);
            }
            catch (LogicBuilderException ex)
            {
                TaskComplete(ex.Message);
                LogicBuilderExceptionOccurred(ex);
            }
            catch (OperationCanceledException ex)
            {
                TaskComplete(Strings.progressFormOperationCancelled);
                LogicBuilderExceptionOccurred(new LogicBuilderException(ex.Message, ex));
            }
            finally
            {
                if (!progressForm.IsDisposed)
                {
                    progressForm.Close();
                }

                cancellationTokenSource.Dispose();
                await Task.Delay(40);
                TaskComplete(Strings.statusBarReadyMessage);
            }
        }

        public async Task RunLoadContextAsync(Func<CancellationTokenSource, Task> task)
        {
            var progress = new Progress<ProgressMessage>(percent =>
            {
                UpdateProgress
                (
                    percent.Progress,
                    string.Format(CultureInfo.CurrentCulture, Strings.progressFormStatusMessageFormat2, percent.Message, percent.Progress)
                );
            });

            await _loadContextSponsor.RunAsync
            (
                async () =>
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    try
                    {
                        await task(cancellationTokenSource);
                        await Task.Delay(40);
                        TaskComplete(Strings.statusBarReadyMessage);
                    }
                    catch (LogicBuilderException ex)
                    {
                        TaskComplete(ex.Message);
                        LogicBuilderExceptionOccurred(ex);
                    }
                    catch (OperationCanceledException ex)
                    {
                        TaskComplete(ex.Message);
                        LogicBuilderExceptionOccurred(new LogicBuilderException(ex.Message, ex));
                    }
                    finally
                    {
                        cancellationTokenSource.Dispose();
                    }
                },
                progress
            );
        }

        public async Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
        {
            var progress = new Progress<ProgressMessage>(percent =>
            {
                UpdateProgress
                (
                    percent.Progress,
                    string.Format(CultureInfo.CurrentCulture, Strings.progressFormStatusMessageFormat2, percent.Message, percent.Progress)
                );
            });

            await _loadContextSponsor.RunAsync
            (
                async () =>
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    //Showing the progress form after LoadAssembiesIfNeededAsync runs in LoadContextSponsor.RunAsync
                    //Don't see any value in cancelling the process during assembly loading
                    using IProgressForm progressForm = _serviceFactory.GetProgressForm(progress, cancellationTokenSource);
                    progressForm.Show(this);

                    try
                    {
                        await task(progress, cancellationTokenSource);
                        await Task.Delay(40);
                        TaskComplete(Strings.statusBarReadyMessage);
                    }
                    catch (LogicBuilderException ex)
                    {
                        TaskComplete(ex.Message);
                        LogicBuilderExceptionOccurred(ex);
                    }
                    catch (OperationCanceledException ex)
                    {
                        TaskComplete(Strings.progressFormOperationCancelled);
                        LogicBuilderExceptionOccurred(new LogicBuilderException(ex.Message, ex));
                    }
                    finally
                    {
                        if (!progressForm.IsDisposed)
                        {
                            progressForm.Close();
                        }

                        cancellationTokenSource.Dispose();
                        await Task.Delay(40);
                        TaskComplete(Strings.statusBarReadyMessage);
                    }
                },
                progress
            );
        }

        /// <summary>
        /// Enables or disables buttons depending on whether a project is currently open
        /// </summary>
        /// <param name="projectOpen"></param>
        public void SetButtonStates(bool projectOpen)
        {
            //File Menu
            radMenuItemNewProject.Enabled = !projectOpen;
            radMenuItemOpenProject.Enabled = !projectOpen;
            radMenuItemCloseProject.Enabled = projectOpen;
            radMenuItemRecentProjects.Enabled = !projectOpen;
            if (projectOpen)
            {
                recentProjectsMenuItemList.Clear();
                radMenuItemRecentProjects.Items.Clear();
            }
            //File Menu

            //Edit Menu
            radMenuItemFindinFiles.Enabled = projectOpen;
            radMenuItemFindInFilesText.Enabled = projectOpen;//must be disabled(though not visible) because of shortcut keys

            //must be disabled (though not visible) because of shortcut keys
            radMenuItemFindText.Enabled = projectOpen;
            radMenuItemFindConstructor.Enabled = projectOpen;
            radMenuItemFindFunction.Enabled = projectOpen;
            radMenuItemFindVariable.Enabled = projectOpen;
            //must be disabled (though not visible) because of shortcut keys

            //must be disabled (though not visible) because of shortcut keys
            radMenuItemReplaceText.Enabled = projectOpen;
            radMenuItemReplaceConstructor.Enabled = projectOpen;
            radMenuItemReplaceFunction.Enabled = projectOpen;
            radMenuItemReplaceVariable.Enabled = projectOpen;
            //must be disabled (though not visible) because of shortcut keys
            //Edit Menu

            //View Menu
            radMenuItemProjectExplorer.Enabled = projectOpen;
            radMenuItemMessagesList.Enabled = projectOpen;
            //View Menu

            //Project Menu
            radMenuItemConstructors.Enabled = projectOpen;
            radMenuItemFunctions.Enabled = projectOpen;
            radMenuItemVariables.Enabled = projectOpen;
            radMenuItemConnectorObjectTypes.Enabled = projectOpen;
            radMenuItemXMLFragments.Enabled = projectOpen;
            radMenuItemProjectProperties.Enabled = projectOpen;
            //Project Menu

            //Tools Menu
            radMenuItemBuildRules.Enabled = projectOpen;
            //must be disabled (though not visible) because of shortcut keys
            radMenuItemBuildSelectedModules.Enabled = projectOpen;
            radMenuItemBuildActiveDrawing.Enabled = projectOpen;
            radMenuItemBuildActiveTable.Enabled = projectOpen;
            //must be disabled (though not visible) because of shortcut keys

            radMenuItemValidateDocuments.Enabled = projectOpen;
            //must be disabled (though not visible) because of shortcut keys
            radMenuItemValidateSelectedModules.Enabled = projectOpen;
            radMenuItemValidateActiveDrawing.Enabled = projectOpen;
            radMenuItemValidateActiveTable.Enabled = projectOpen;
            //must be disabled (though not visible) because of shortcut keys

            radMenuItemValidateRules.Enabled = projectOpen;
            radMenuItemFileSystemDeployment.Enabled = projectOpen;
            radMenuItemWebApiDeployment.Enabled = projectOpen;
            radMenuItemSelectApplication.Enabled = projectOpen;
            //Tools Menu
        }

        /// <summary>
        /// Hides or shows menu buttons depending on whether a diagram, table or neither is open
        /// </summary>
        /// <param name="visioOpen"></param>
        /// <param name="tableOpen"></param>
        public void SetEditControlMenuStates(bool visioOpen, bool tableOpen)
        {
            this.radCommandBar1.Enabled = visioOpen || tableOpen;

            //File Menu
            radMenuItemSave.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemPageSetup.Visibility = GetVisibility(visioOpen);
            radMenuSeparatorItemPageSetup.Visibility = GetVisibility(visioOpen);
            //File Menu

            //Edit Menu
            radMenuItemUndo.Visibility = GetVisibility(visioOpen);
            radMenuItemRedo.Visibility = GetVisibility(visioOpen);

            radMenuSeparatorItemUpdate.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemUpdate.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemDelete.Visibility = GetVisibility(visioOpen || tableOpen);

            radMenuSeparatorItemFind.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemFind.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemFindShape.Visibility = GetVisibility(visioOpen);
            radMenuItemFindCell.Visibility = GetVisibility(tableOpen);
            radMenuItemFindText.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemFindConstructor.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemFindFunction.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemFindVariable.Visibility = GetVisibility(visioOpen || tableOpen);

            radMenuItemReplace.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemReplaceText.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemReplaceConstructor.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemReplaceFunction.Visibility = GetVisibility(visioOpen || tableOpen);
            radMenuItemReplaceVariable.Visibility = GetVisibility(visioOpen || tableOpen);

            radMenuSeparatorItemIndexInformation.Visibility = GetVisibility(visioOpen);
            radMenuItemIndexInformation.Visibility = GetVisibility(visioOpen);
            //Edit Menu

            //View Menu
            radMenuItemFlowDiagramStencil.Visibility = GetVisibility(visioOpen);
            radMenuItemApplicationsStencil.Visibility = GetVisibility(visioOpen);
            radMenuItemPanZoomWindow.Visibility = GetVisibility(visioOpen);
            //View Menu

            //Rules Menu
            radMenuItemRules.Visibility = GetVisibility(tableOpen);
            //Rules Menu

            //Tools Menu
            radMenuItemBuildActiveDrawing.Visibility = GetVisibility(visioOpen);
            radMenuItemBuildActiveTable.Visibility = GetVisibility(tableOpen);
            radMenuItemValidateActiveDrawing.Visibility = GetVisibility(visioOpen);
            radMenuItemValidateActiveTable.Visibility = GetVisibility(tableOpen);
            //Tools Menu

            static ElementVisibility GetVisibility(bool visible)
                => visible ? ElementVisibility.Visible : ElementVisibility.Collapsed;
        }

        public void UpdateApplicationMenuItems()
        {
            List<Application> applicationList = new(_configurationService.ProjectProperties.ApplicationList.Values.OrderBy(a => a.Nickname));

            UpdateApplicationMenuItems
            (
                applicationList,
                radMenuItemFileSystemDeploy,
                deployFileSystemApplicationMenuItemList,
                applicationName => _applicationCommandsFactory.GetDeploySelectedFilesToFileSystemCommand(applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList,
                radMenuItemFileSystemDelete,
                deleteFileSystemApplicationMenuItemList,
                applicationName => _applicationCommandsFactory.GetDeleteSelectedFilesFromFileSystemCommand(applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList,
                radMenuItemWebApiDeploy,
                deployWebApiApplicationMenuItemList,
                applicationName => _applicationCommandsFactory.GetDeploySelectedFilesToApiCommand(applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList,
                radMenuItemWebApiDelete,
                deleteWebApiApplicationMenuItemList,
                applicationName => _applicationCommandsFactory.GetDeleteSelectedFilesFromApiCommand(applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList,
                radMenuItemValidateRules,
                validateApplicationRulesMenuItemList,
                applicationName => _applicationCommandsFactory.GetValidateSelectedRulesCommand(applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList,
                radMenuItemSelectApplication,
                selectedApplicationRulesMenuItemList,
                applicationName => _applicationCommandsFactory.GetSetSelectedApplicationCommand(radMenuItemSelectApplication, applicationName),
                false
            );
        }

        private void AddBuildActiveDocumentCommands()
        {
            void handler(object? sender, EventArgs args) => _buildActiveDocumentCommand.Execute();
            radMenuItemBuildActiveDrawing.Click += handler;
            radMenuItemBuildActiveTable.Click += handler;
            commandBarButtonBuild.Click += handler;
        }

        private static void AddClickCommand(RadMenuItem menuItem, IClickCommand command)
        {
            menuItem.Click += (sender, args) => command.Execute();
        }

        private void AddClickCommands()
        {
            #region File Menu
            AddClickCommand(this.radMenuItemNewProject, _newProjectCommand);
            AddClickCommand(this.radMenuItemOpenProject, _openProjectCommand);
            AddClickCommand(this.radMenuItemPageSetup, _pageSetupCommand);
            AddClickCommand(this.radMenuItemCloseProject, _closeProjectCommand);
            AddClickCommand(this.radMenuItemExit, _exitCommand);
            #endregion File Menu

            #region Tools Menu
            AddClickCommand
            (
                this.radMenuItemBuildSelectedModules,
                _buildSaveConsolidateSelectedDocumentsCommand
            );
            AddClickCommand
            (
                this.radMenuItemValidateSelectedModules,
                _validateSelectedDocumentsCommand
            );
            AddBuildActiveDocumentCommands();
            AddEditActiveDocumentCommands();
            AddSaveActiveDocumentCommands();
            AddValidateActiveDocumentCommands();
            #endregion Tools Menu

            #region Edit/Find
            AddClickCommand(this.radMenuItemUndo, _undoCommand);
            AddClickCommand(this.radMenuItemRedo, _redoCommand);
            AddClickCommand(this.radMenuItemDelete, _deleteSelectionCommand);
            AddClickCommand(this.radMenuItemFindConstructor, _findConstructorCommand);
            AddClickCommand(this.radMenuItemFindCell, _findCellCommand);
            AddClickCommand(this.radMenuItemFindFunction, _findFunctionCommand);
            AddClickCommand(this.radMenuItemFindShape, _findShapeCommand);
            AddClickCommand(this.radMenuItemFindText, _findTextCommand);
            AddClickCommand(this.radMenuItemFindVariable, _findVariableCommand);

            AddClickCommand(this.radMenuItemReplaceConstructor, _findReplaceConstructorCommand);
            AddClickCommand(this.radMenuItemReplaceFunction, _findReplaceFunctionCommand);
            AddClickCommand(this.radMenuItemReplaceText, _findReplaceTextCommand);
            AddClickCommand(this.radMenuItemReplaceVariable, _findReplaceVariableCommand);

            AddClickCommand(this.radMenuItemFindInFilesText, _findTextInFilesCommand);
            AddClickCommand(this.radMenuItemFindInFilesConstructor, _findConstructorInFilesCommand);
            AddClickCommand(this.radMenuItemFindInFilesFunction, _findFunctionInFilesCommand);
            AddClickCommand(this.radMenuItemFindInFilesVariable, _findVariableInFilesCommand);

            AddClickCommand(this.radMenuItemIndexInformation, _displayIndexInformationCommand);
            #endregion Edit/Find

            #region View Menu
            AddClickCommand
                (
                    this.radMenuItemMessagesList,
                    _viewMessagesCommand
                );
            AddClickCommand
            (
                this.radMenuItemProjectExplorer,
                _viewProjectExplorerCommand
            );
            AddClickCommand
            (
                this.radMenuItemApplicationsStencil,
                _viewApplicationsStencilCommand
            );
            AddClickCommand
            (
                this.radMenuItemFlowDiagramStencil,
                _viewFlowDiagramStencilCommand
            );
            AddClickCommand
            (
                this.radMenuItemPanZoomWindow,
                _viewPanAndZoomWindowCommand
            );
            #endregion View Menu

            #region Project Menu
            AddClickCommand
            (
                this.radMenuItemConnectorObjectTypes,
                _editConnectorObjectTypesCommand
            );
            AddClickCommand
            (
                this.radMenuItemConstructors,
                _editConstructorsCommand
            );
            AddClickCommand
            (
                this.radMenuItemXMLFragments,
                _editFragmentsCommand
            );
            AddClickCommand
            (
                this.radMenuItemFunctions,
                _editFunctionsCommand
            );
            AddClickCommand
            (
                this.radMenuItemProjectProperties,
                _editProjectPropertiesCommand
            );
            AddClickCommand
            (
                this.radMenuItemVariables,
                _editVariablesCommand
            );
            #endregion Project Menu

            #region Help Menu
            AddClickCommand
            (
                this.radMenuItemContents,
                _showHelpContentsCommand
            );
            AddClickCommand
            (
                this.radMenuItemAbout,
                _showAboutCommand
            );
            #endregion Help Menu

            AddPreferencesMenuItemClickCommands();
        }

        private void AddDocumentEditorControl(IDocumentEditor documentEditor, bool visioOpen, bool tableOpen)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            AddEditControl(documentEditor);
            SetEditControlMenuStates(visioOpen, tableOpen);
            this.Refresh();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        private void AddEditActiveDocumentCommands()
        {
            void handler(object? sender, EventArgs args) => _editActiveDocumentCommand.Execute();
            radMenuItemUpdate.Click += handler;
            commandBarButtonEdit.Click += handler;
        }

        private void AddEditControl(IDocumentEditor documentEditor)
        {
            Control editControl = (Control)documentEditor;
            editControl.Dock = DockStyle.Fill;
            EditControl = documentEditor;
            this.splitPanelEdit.Controls.Clear();//controls disposed in SplitPanelEdit_ControlRemoved
            this.splitPanelEdit.Controls.Add(editControl);
        }

        private void AddValidateActiveDocumentCommands()
        {
            void handler(object? sender, EventArgs args) => _validateActiveDocumentCommand.Execute();
            radMenuItemValidateActiveDrawing.Click += handler;
            radMenuItemValidateActiveTable.Click += handler;
            commandBarButtonValidate.Click += handler;
        }

        private void AddPreferencesMenuItemClickCommands()
        {
            foreach (RadMenuItem radMenuItem in radMenuItemColorTheme.Items.Cast<RadMenuItem>())
            {
                AddClickCommand(radMenuItem, _applicationCommandsFactory.GetSetColorThemeCommand(radMenuItemColorTheme, radMenuItemFontSize, (string)radMenuItem.Tag));
            }

            foreach (RadMenuItem radMenuItem in radMenuItemFontSize.Items.Cast<RadMenuItem>())
            {
                AddClickCommand
                (
                    radMenuItem,
                    _applicationCommandsFactory.GetSetFontSizeCommand
                    (
                        radMenuItemColorTheme,
                        radMenuItemFontSize,
                        int.Parse
                        (
                            radMenuItem.Tag?.ToString() ?? throw _exceptionHelper.CriticalException("{A31C4B29-E571-41AC-BB83-255B92CEA86F}"),
                            CultureInfo.InvariantCulture
                        )
                    )
                );
            }
        }

        private void AddSaveActiveDocumentCommands()
        {
            void handler(object? sender, EventArgs args) => _saveActiveDocumentCommand.Execute();
            radMenuItemSave.Click += handler;
            commandBarButtonSave.Click += handler;
        }

        private void ClearApplicationMenuItems()
        {
            radMenuItemSelectApplication.Items.Clear();
            selectedApplicationRulesMenuItemList.Clear();

            radMenuItemFileSystemDeploy.Items.Clear();
            deployFileSystemApplicationMenuItemList.Clear();

            radMenuItemFileSystemDelete.Items.Clear();
            deleteFileSystemApplicationMenuItemList.Clear();

            radMenuItemWebApiDeploy.Items.Clear();
            deployWebApiApplicationMenuItemList.Clear();

            radMenuItemWebApiDelete.Items.Clear();
            deleteWebApiApplicationMenuItemList.Clear();

            radMenuItemValidateRules.Items.Clear();
            validateApplicationRulesMenuItemList.Clear();
        }

        private static void Dispose(IDisposable disposable)
        {
            disposable?.Dispose();
        }

        private void DocumentExplorerErrorCountChanged(int errorCount)
        {
            bool enabled = errorCount == 0;

            //Project Menu
            radMenuItemConstructors.Enabled = enabled;
            radMenuItemFunctions.Enabled = enabled;
            radMenuItemVariables.Enabled = enabled;
            radMenuItemConnectorObjectTypes.Enabled = enabled;
            radMenuItemXMLFragments.Enabled = enabled;
            radMenuItemProjectProperties.Enabled = enabled;
            //why no (projectPropertiesToolStripMenuItem.Enabled = enabled) ?
            //Project Menu

            //Tools Menu
            radMenuItemBuildRules.Enabled = enabled;
            radMenuItemFileSystemDeployment.Enabled = enabled;
            radMenuItemWebApiDeployment.Enabled = enabled;
            radMenuItemSelectApplication.Enabled = enabled;
            //Tools Menu
        }

        private void Initialize()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            _mainWindow.Instance = this;
            _formInitializer.SetCenterScreen(this);
            _projectExplorer.Dock = DockStyle.Fill;
            this.splitPanelExplorer.SuspendLayout();
            this.splitPanelExplorer.Controls.Add((Control)_projectExplorer);
            this.splitPanelExplorer.ResumeLayout(false);
            this.splitPanelExplorer.PerformLayout();

            _messages.Dock = DockStyle.Fill;
            this.splitPanelMessages.SuspendLayout();
            this.splitPanelMessages.Controls.Add((Control)_messages);
            this.splitPanelMessages.ResumeLayout(false);
            this.splitPanelMessages.PerformLayout();

            SetShortCutKeys();
            this.Icon = _formInitializer.GetLogicBuilderIcon();

            this.radProgressBarElement1.Visibility = ElementVisibility.Collapsed;

            radCommandBar1.Enabled = false;
            this.commandBarStripElement1.Grip.Visibility = ElementVisibility.Collapsed;
            this.commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            this.commandBarStripElement1.BorderWidth = 0;

            _messages.Visible = false;
            _projectExplorer.Visible = false;

            SetButtonStates(false);
            SetEditControlMenuStates(false, false);
            UpdateRecentProjectMenuItems();
            _themeManager.CheckMenuItemsForCurrentSettings(this.radMenuItemColorTheme.Items, this.radMenuItemFontSize.Items);

            AddClickCommands();

            ThemeResolutionService.ApplicationThemeChanged += ThemeResolutionService_ApplicationThemeChanged;
            this.splitPanelEdit.ControlRemoved += SplitPanelEdit_ControlRemoved;
            this.Disposed += MDIParent_Disposed;
            this.FormClosing += MDIParent_FormClosing;

            //OpenProject(@"C:\Test\NewProject\NewProject.lbproj");
            //OpenProject(@"C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso\Contoso.lbproj");
            //OpenProject(@"C:\TelerikLogicBuilder\FlowProjects\Contoso.Test\Contoso.Test.lbproj");
        }

        private async Task LoadAssembliesOnProjectOpen()
        {
            radProgressBarElement1.Value1 = 50;
            radLabelElement1.Text = Strings.loadingAssemblies2;

            await _loadContextSponsor.LoadAssembiesOnOpenProject();

            radProgressBarElement1.Value1 = 0;
            radLabelElement1.Text = Strings.statusBarReadyMessage;
        }

        private void LogicBuilderExceptionOccurred(LogicBuilderException exception)
        {
            DisplayMessage.Show(this, exception.Message, _mainWindow.RightToLeft);
        }

        private void RefreshSize()
        {//workaround for title bar not being resized on font change.
            System.Drawing.Size originalSize = this.Size;

            Native.NativeMethods.LockWindowUpdate(this.Handle);

            //this.ElementTree.RootElement.ResumeLayout(true);
            //this.ElementTree.PerformLayout();
            //this.MaximumSize = new System.Drawing.Size(0, 0);
            //this.FormElement.TitleBar.MaxSize = new System.Drawing.Size(0, 0);
            //this.FormElement.TitleBar.MinSize = new System.Drawing.Size(0, 0);
            //this.Scale(1.0000001f);
            this.Scale(new System.Drawing.SizeF(1.0000001f, 1.0000001f));

            //Native.NativeMethods.LockWindowUpdate(this.Handle);
            //this.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height);
            //
            this.Size = originalSize;
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        private void SetSelectedApplication(string applicationName)
        {
            _configurationService.SetSelectedApplication(applicationName);
            _checkSelectedApplication.CheckSelectedItem(radMenuItemSelectApplication.Items);
        }

        private void SetShortCutKeys()
        {
            this.KeyPreview = true;

            //File Menu
            DoSet(radMenuItemNewProject, Keys.Control | Keys.Shift, Keys.N);
            DoSet(radMenuItemOpenProject, Keys.Control | Keys.Shift, Keys.O);
            DoSet(radMenuItemPageSetup, Keys.Control | Keys.Shift, Keys.P);
            DoSet(radMenuItemExit, Keys.Alt, Keys.X);

            //Edit Menu
            DoSet(radMenuItemUndo, Keys.Alt, Keys.Z);
            DoSet(radMenuItemRedo, Keys.Alt, Keys.Y);
            DoSet(radMenuItemUpdate, Keys.Alt, Keys.E);

            DoSet(radMenuItemFindText, Keys.Alt, Keys.F);
            DoSet(radMenuItemFindConstructor, Keys.Alt, Keys.C);
            DoSet(radMenuItemFindFunction, Keys.Alt, Keys.T);
            DoSet(radMenuItemFindVariable, Keys.Alt, Keys.V);
            DoSet(radMenuItemReplaceText, Keys.Alt, Keys.H);
            DoSet(radMenuItemReplaceConstructor, Keys.Alt, Keys.O);
            DoSet(radMenuItemReplaceFunction, Keys.Alt, Keys.U);
            DoSet(radMenuItemReplaceVariable, Keys.Alt, Keys.A);

            DoSet(radMenuItemFindInFilesText, Keys.Alt | Keys.Shift, Keys.F);

            //Tools Menu
            DoSet(radMenuItemBuildSelectedModules, Keys.Alt, Keys.D1);
            DoSet(radMenuItemBuildActiveDrawing, Keys.Alt | Keys.Shift, Keys.D1);//(should also activate Build Table when thee table is open - no harm done since they both call BuildRules on the current IChildControl)
            DoSet(radMenuItemValidateSelectedModules, Keys.Alt, Keys.D2);
            DoSet(radMenuItemValidateActiveDrawing, Keys.Alt | Keys.Shift, Keys.D2);//(should also activate Validate Table when thee table is open - no harm done since they both call ValidateRules (validate the document) on the current IChildControl)

            //Help Menu
            DoSet(radMenuItemContents, Keys.Control, keys: Keys.F1);

            static void DoSet(RadMenuItem menuItem, Keys modifiers, params Keys[] keys)
            {
                menuItem.Shortcuts.Add(new RadShortcut(modifiers, keys));
            }
        }

        private static void UpdateApplicationMenuItems(IList<Application> applicationList, RadMenuItem parentMenuItem, IDictionary<string, RadMenuItem> itemDictionary, Func<string, IClickCommand> getClickCommand, bool addEllipsis = true)
        {
            itemDictionary.Clear();
            parentMenuItem.Items.Clear();
            foreach (Application application in applicationList)
            {
                RadMenuItem radMenuItem = new
                (
                    addEllipsis
                    ? string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.displayMemuItemWithEllipsisFormat,
                            application.Nickname
                        )
                    : application.Nickname,
                    application.Name
                );

                AddClickCommand(radMenuItem, getClickCommand(application.Name));
                itemDictionary.Add(application.Name, radMenuItem);
                parentMenuItem.Items.Add(radMenuItem);
            }
        }

        private void UpdateRecentProjectMenuItems()
        {
            RecentFilesList.Refresh();
            foreach (string key in RecentFilesList.FileList.Keys)
            {
                RadMenuItem radMenuItem = new
                (
                    RecentFilesList.FileList[key],
                    key
                )
                {
                    ToolTipText = key
                };

                if (recentProjectsMenuItemList.ContainsKey(key))//Inconsistent issue.  Application failing to close after opening closing
                    continue;                                   //a project and opening a new one.

                radMenuItem.Click += (sender, args) => OpenProject(((RadMenuItem)sender!).Tag.ToString()!);
                recentProjectsMenuItemList.Add(key, radMenuItem);
                radMenuItemRecentProjects.Items.Add(radMenuItem);
            }
        }

        private void UpdateProgress(int percentComplete, string message)
        {
            radProgressBarElement1.Visibility = ElementVisibility.Visible;
            radProgressBarElement1.Value1 = percentComplete;
            radLabelElement1.Text = message;
        }

        private void TaskComplete(string message)
        {
            radProgressBarElement1.Visibility = ElementVisibility.Collapsed;
            radProgressBarElement1.Value1 = 0;
            radLabelElement1.Text = message;
        }

        public void RemoveEditControl()
        {
            this.splitPanelEdit.Controls.Clear();
            if (this.EditControl != null)
                Dispose((IDisposable)this.EditControl);

            this.EditControl = null;
        }
        #endregion Methods

        #region Event Handlers
        private void MDIParent_Disposed(object? sender, EventArgs e)
        {
            ThemeResolutionService.ApplicationThemeChanged -= ThemeResolutionService_ApplicationThemeChanged;
            Dispose(documentExplorerErrorCountChangedSubscription);
            Dispose(logicBuilderExceptionSubscription);
        }

        private void MDIParent_FormClosing(object? sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void SplitPanelEdit_ControlRemoved(object? sender, ControlEventArgs e)
        {
            if (e.Control is ContainerControl control)
                control.Dispose();
        }

        private void ThemeResolutionService_ApplicationThemeChanged(object sender, ThemeChangedEventArgs args)
        {
            if (this.IsDisposed)
                return;

            RefreshSize();
        }
        #endregion Event Handlers
    }
}
