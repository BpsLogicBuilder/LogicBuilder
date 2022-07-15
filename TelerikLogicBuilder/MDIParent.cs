using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        private readonly ICheckSelectedApplication _checkSelectedApplication;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly IFormInitializer _formInitializer;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly ILoadContextSponsor _loadContextSponsor;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IMessageBoxOptionsHelper _messageBoxOptionsHelper;
        private readonly IThemeManager _themeManager;
        private readonly IVariableListInitializer _variableListInitializer;
        private readonly UiNotificationService _uiNotificationService;

        private readonly Func<IMDIParent, BuildActiveDocumentCommand> _getBuildActiveDocumentCommand;
        private readonly Func<MDIParent, BuildSaveConsolidateSelectedDocumentsCommand> _getBuildSaveConsolidateSelectedDocumentsCommand;
        private readonly Func<MDIParent, string, DeleteSelectedFilesFromApiCommand> _getDeleteSelectedFilesFromApiCommand;
        private readonly Func<MDIParent, string, DeleteSelectedFilesFromFileSystemCommand> _getDeleteSelectedFilesFromFileSystemCommand;
        private readonly Func<MDIParent, string, DeploySelectedFilesToApiCommand> _getDeploySelectedFilesToApiCommand;
        private readonly Func<MDIParent, string, DeploySelectedFilesToFileSystemCommand> _getDeploySelectedFilesToFileSystemCommand;
        private readonly Func<RadMenuItem, string, SetSelectedApplicationCommand> _getSetSelectedApplicationCommand;
        private readonly Func<RadMenuItem, string, SetThemeCommand> _getSetThemeCommand;
        private readonly Func<IMDIParent, ValidateActiveDocumentCommand> _getValidateActiveDocumentCommand;
        private readonly Func<MDIParent, ValidateSelectedDocumentsCommand> _getValidateSelectedDocumentsCommand;
        private readonly Func<MDIParent, string, ValidateSelectedRulesCommand> _getValidateSelectedRulesCommand;

        //controls
        private readonly ProjectExplorer _projectExplorer;
        private readonly Messages _messages;

        public MDIParent(
            ICheckSelectedApplication checkSelectedApplication,
            IConfigurationService configurationService,
            IConstructorListInitializer constructorListInitializer,
            IFormInitializer formInitializer,
            IFragmentListInitializer fragmentListInitializer,
            IFunctionListInitializer functionListInitializer,
            ILoadContextSponsor loadContextSponsor,
            ILoadProjectProperties loadProjectProperties,
            IMessageBoxOptionsHelper messageBoxOptionsHelper,
            IThemeManager themeManager,
            IVariableListInitializer variableListInitializer,
            UiNotificationService uiNotificationService,
            Func<IMDIParent, BuildActiveDocumentCommand> getBuildActiveDocumentCommand,
            Func<MDIParent, BuildSaveConsolidateSelectedDocumentsCommand> getBuildSaveConsolidateSelectedDocumentsCommand,
            Func<MDIParent, string, DeleteSelectedFilesFromApiCommand> getDeleteSelectedFilesFromApiCommand,
            Func<MDIParent, string, DeleteSelectedFilesFromFileSystemCommand> getDeleteSelectedFilesFromFileSystemCommand,
            Func<MDIParent, string, DeploySelectedFilesToApiCommand> getDeploySelectedFilesToApiCommand,
            Func<MDIParent, string, DeploySelectedFilesToFileSystemCommand> getDeploySelectedFilesToFileSystemCommand,
            Func<RadMenuItem, string, SetSelectedApplicationCommand> getSetSelectedApplicationCommand,
            Func<RadMenuItem, string, SetThemeCommand> getSetThemeCommand,
            Func<IMDIParent, ValidateActiveDocumentCommand> getValidateActiveDocumentCommand,
            Func<MDIParent, ValidateSelectedDocumentsCommand> getValidateSelectedDocumentsCommand,
            Func<MDIParent, string, ValidateSelectedRulesCommand> getValidateSelectedRulesCommand,
            Messages messages,
            ProjectExplorer projectExplorer)
        {
            _checkSelectedApplication = checkSelectedApplication;
            _configurationService = configurationService;
            _constructorListInitializer = constructorListInitializer;
            _getDeleteSelectedFilesFromApiCommand = getDeleteSelectedFilesFromApiCommand;
            _getDeleteSelectedFilesFromFileSystemCommand = getDeleteSelectedFilesFromFileSystemCommand;
            _getDeploySelectedFilesToApiCommand = getDeploySelectedFilesToApiCommand;
            _getDeploySelectedFilesToFileSystemCommand = getDeploySelectedFilesToFileSystemCommand;
            _formInitializer = formInitializer;
            _fragmentListInitializer = fragmentListInitializer;
            _functionListInitializer = functionListInitializer;
            _loadContextSponsor = loadContextSponsor;
            _loadProjectProperties = loadProjectProperties;
            _messageBoxOptionsHelper = messageBoxOptionsHelper;
            _themeManager = themeManager;
            _getSetSelectedApplicationCommand = getSetSelectedApplicationCommand;
            _getSetThemeCommand = getSetThemeCommand;
            _getValidateActiveDocumentCommand = getValidateActiveDocumentCommand;
            _getValidateSelectedDocumentsCommand = getValidateSelectedDocumentsCommand;
            _getValidateSelectedRulesCommand = getValidateSelectedRulesCommand;
            _variableListInitializer = variableListInitializer;
            _uiNotificationService = uiNotificationService;

            _getBuildActiveDocumentCommand = getBuildActiveDocumentCommand;
            _getBuildSaveConsolidateSelectedDocumentsCommand = getBuildSaveConsolidateSelectedDocumentsCommand;
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

        public IDocumentEditor? EditControl { get; set; }

        #region Methods
        public void AddTableControl(IDocumentEditor documentEditor) 
            => AddDocumentEditorControl(documentEditor, false, true);

        public void AddVisioControl(IDocumentEditor documentEditor) 
            => AddDocumentEditorControl(documentEditor, true, false);

        public void ChangeCursor(Cursor cursor) => this.Cursor = cursor;

        public void CloseProject()
        {
            if (this.EditControl != null)
                this.EditControl.Close();

            _projectExplorer.ClearProfile();
            _projectExplorer.Visible = false;
            /*TODO Clear Message Tabs*/
            _messages.Visible = false;
            SetButtonStates(false);
            UpdateRecentProjectMenuItems();
            ClearApplicationMenuItems();

            _loadContextSponsor.UnloadAssembliesOnCloseProject();
            _configurationService.ClearConfigurationData();

            this.Refresh();
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

        internal async Task RunAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
        {
            var progress = new Progress<ProgressMessage>(percent =>
            {
                radProgressBarElement1.Value1 = percent.Progress;
                radLabelElement1.Text = string.Format(CultureInfo.CurrentCulture, Strings.progressFormStatusMessageFormat2, percent.Message, percent.Progress);
            });

            var cancellationTokenSource = new CancellationTokenSource();
            using ProgressForm progressForm = new(_formInitializer, progress, cancellationTokenSource);
            progressForm.Show(this);

            try
            {
                await task(progress, cancellationTokenSource);
                await Task.Delay(40);
                radProgressBarElement1.Value1 = 0;
                radLabelElement1.Text = Strings.statusBarReadyMessage;
            }
            catch (LogicBuilderException ex)
            {
                radProgressBarElement1.Value1 = 0;
                radLabelElement1.Text = ex.Message;
                DisplayMessage.Show(this, ex.Message, _messageBoxOptionsHelper.MessageBoxOptions);
            }
            catch (OperationCanceledException)
            {
                radProgressBarElement1.Value1 = 0;
                radLabelElement1.Text = Strings.progressFormOperationCancelled;
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

        public async Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
        {
            var progress = new Progress<ProgressMessage>(percent =>
            {
                radProgressBarElement1.Value1 = percent.Progress;
                radLabelElement1.Text = string.Format(CultureInfo.CurrentCulture, Strings.progressFormStatusMessageFormat2, percent.Message, percent.Progress);
            });

            await _loadContextSponsor.RunAsync
            (
                async () =>
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    //Showing the progress form after LoadAssembiesIfNeededAsync runs in LoadContextSponsor.RunAsync
                    //Don't see any value in cancelling the process during assembly loading
                    using ProgressForm progressForm = new(_formInitializer, progress, cancellationTokenSource);
                    progressForm.Show(this);

                    try
                    {
                        await task(progress, cancellationTokenSource);
                        await Task.Delay(40);
                        UpdateProgress(0, Strings.statusBarReadyMessage);
                    }
                    catch (LogicBuilderException ex)
                    {
                        UpdateProgress(0, ex.Message);
                        LogicBuilderExceptionOccurred(ex);
                    }
                    catch (OperationCanceledException)
                    {
                        UpdateProgress(0, Strings.progressFormOperationCancelled);
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

        private void AddBuildActiveDocumentCommands()
        {
            void handler(object? sender, EventArgs args) => _getBuildActiveDocumentCommand(this).Execute();
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
            AddClickCommand
            (
                this.radMenuItemBuildSelectedModules,
                _getBuildSaveConsolidateSelectedDocumentsCommand(this)
            );
            AddClickCommand
            (
                this.radMenuItemValidateSelectedModules,
                _getValidateSelectedDocumentsCommand(this)
            );

            AddBuildActiveDocumentCommands();

            AddValidateActiveDocumentCommands();

            AddThemeMenuItemClickCommands(this.radMenuItemTheme);
        }

        private void AddDocumentEditorControl(IDocumentEditor documentEditor, bool visioOpen, bool tableOpen)
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            AddEditControl(documentEditor);
            SetEditControlMenuStates(visioOpen, tableOpen);
            this.Refresh();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        private void AddEditControl(IDocumentEditor documentEditor)
        {
            Control editControl = (Control)documentEditor;
            editControl.Dock = DockStyle.Fill;
            EditControl = documentEditor;
            this.splitPanelEdit.Controls.Clear();
            this.splitPanelEdit.Controls.Add(editControl);
        }

        private void AddValidateActiveDocumentCommands()
        {
            void handler(object? sender, EventArgs args) => _getValidateActiveDocumentCommand(this).Execute();
            radMenuItemValidateActiveDrawing.Click += handler;
            radMenuItemValidateActiveTable.Click += handler;
            commandBarButtonValidate.Click += handler;
        }

        private void AddThemeMenuItemClickCommands(RadMenuItem themeMenuItem)
        {
            foreach (RadMenuItem radMenuItem in themeMenuItem.Items)
            {
                AddClickCommand(radMenuItem, _getSetThemeCommand(themeMenuItem, (string)radMenuItem.Tag));
            }
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
            if (disposable != null)
                disposable.Dispose();
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

            _messageBoxOptionsHelper.MainWindow = this;
            _messageBoxOptionsHelper.MessageBoxOptions = this.RightToLeft;
            _formInitializer.SetCenterScreen(this);
            _projectExplorer.Dock = DockStyle.Fill;
            this.splitPanelExplorer.SuspendLayout();
            this.splitPanelExplorer.Controls.Add(_projectExplorer);
            this.splitPanelExplorer.ResumeLayout(false);
            this.splitPanelExplorer.PerformLayout();

            _messages.Dock = DockStyle.Fill;
            this.splitPanelMessages.SuspendLayout();
            this.splitPanelMessages.Controls.Add(_messages);
            this.splitPanelMessages.ResumeLayout(false);
            this.splitPanelMessages.PerformLayout();

            SetShortCutKeys();
            this.Icon = _formInitializer.GetLogicBuilderIcon();

            this.radProgressBarElement1.Visibility = ElementVisibility.Collapsed;

            radCommandBar1.Enabled = false;
            this.commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            
            _messages.Visible = false;
            _projectExplorer.Visible = false;

            SetButtonStates(false);
            SetEditControlMenuStates(false, false);
            UpdateRecentProjectMenuItems();
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);

            AddClickCommands();

            this.splitPanelEdit.ControlRemoved += SplitPanelEdit_ControlRemoved;
            this.Disposed += MDIParent_Disposed;

            //OpenProject(@"C:\Test\SomeProject1\SomeProject1.lbproj");
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
            DisplayMessage.Show(this, exception.Message, _messageBoxOptionsHelper.MessageBoxOptions);
        }

        /// <summary>
        /// Enables or disables buttons depending on whether a project is currently open
        /// </summary>
        /// <param name="projectOpen"></param>
        private void SetButtonStates(bool projectOpen)
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

        private void UpdateApplicationMenuItems()
        {
            List<Application> applicationList = new(_configurationService.ProjectProperties.ApplicationList.Values.OrderBy(a => a.Nickname));

            UpdateApplicationMenuItems
            (
                applicationList, 
                radMenuItemFileSystemDeploy, 
                deployFileSystemApplicationMenuItemList,
                applicationName => _getDeploySelectedFilesToFileSystemCommand(this, applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList, 
                radMenuItemFileSystemDelete, 
                deleteFileSystemApplicationMenuItemList,
                applicationName => _getDeleteSelectedFilesFromFileSystemCommand(this, applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList, 
                radMenuItemWebApiDeploy, 
                deployWebApiApplicationMenuItemList,
                applicationName => _getDeploySelectedFilesToApiCommand(this, applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList, 
                radMenuItemWebApiDelete, 
                deleteWebApiApplicationMenuItemList,
                applicationName => _getDeleteSelectedFilesFromApiCommand(this, applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList, 
                radMenuItemValidateRules, 
                validateApplicationRulesMenuItemList,
                applicationName => _getValidateSelectedRulesCommand(this, applicationName)
            );
            UpdateApplicationMenuItems
            (
                applicationList, 
                radMenuItemSelectApplication, 
                selectedApplicationRulesMenuItemList, 
                applicationName => _getSetSelectedApplicationCommand(radMenuItemSelectApplication, applicationName), 
                false
            );
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
                );

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
            Dispose(documentExplorerErrorCountChangedSubscription);
            Dispose(logicBuilderExceptionSubscription);
        }

        private void SplitPanelEdit_ControlRemoved(object? sender, ControlEventArgs e)
        {
            if (e.Control is ContainerControl control)
                control.Dispose();
        }
        #endregion Event Handlers
    }
}
