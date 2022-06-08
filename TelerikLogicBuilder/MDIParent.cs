using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : RadForm
    {
        private readonly IBuildSaveAssembleRulesForSelectedDocuments _buildSaveConsolidateSelectedDocumentRules;
        private readonly ICheckSelectedApplication _checkSelectedApplication;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly IDeleteSelectedFilesFromApi _deleteSelectedFilesFromApi;
        private readonly IDeleteSelectedFilesFromFileSystem _deleteSelectedFilesFromFileSystem;
        private readonly IDeploySelectedFilesToApi _deploySelectedFilesToApi;
        private readonly IDeploySelectedFilesToFileSystem _deploySelectedFilesToFileSystem;
        private readonly IFormInitializer _formInitializer;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly ILoadContextSponsor _loadContextSponsor;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IThemeManager _themeManager;
        private readonly IValidateSelectedDocuments _validateSelectedDocuments;
        private readonly IValidateSelectedRules _validateSelectedRules;
        private readonly IVariableListInitializer _variableListInitializer;

        public MDIParent(
            IBuildSaveAssembleRulesForSelectedDocuments buildSaveConsolidateSelectedDocumentRules,
            ICheckSelectedApplication checkSelectedApplication,
            IConfigurationService configurationService,
            IConstructorListInitializer constructorListInitializer,
            IDeleteSelectedFilesFromApi deleteSelectedFilesFromApi,
            IDeleteSelectedFilesFromFileSystem deleteSelectedFilesFromFileSystem,
            IDeploySelectedFilesToApi deploySelectedFilesToApi,
            IDeploySelectedFilesToFileSystem deploySelectedFilesToFileSystem,
            IFormInitializer formInitializer,
            IFragmentListInitializer fragmentListInitializer,
            IFunctionListInitializer functionListInitializer,
            ILoadContextSponsor loadContextSponsor,
            ILoadProjectProperties loadProjectProperties,
            IThemeManager themeManager,
            IValidateSelectedDocuments validateSelectedDocuments,
            IValidateSelectedRules validateSelectedRules,
            IVariableListInitializer variableListInitializer)
        {
            _buildSaveConsolidateSelectedDocumentRules = buildSaveConsolidateSelectedDocumentRules;
            _checkSelectedApplication = checkSelectedApplication;
            _configurationService = configurationService;
            _constructorListInitializer = constructorListInitializer;
            _deleteSelectedFilesFromApi = deleteSelectedFilesFromApi;
            _deleteSelectedFilesFromFileSystem = deleteSelectedFilesFromFileSystem;
            _deploySelectedFilesToApi = deploySelectedFilesToApi;
            _deploySelectedFilesToFileSystem = deploySelectedFilesToFileSystem;
            _formInitializer = formInitializer;
            _fragmentListInitializer = fragmentListInitializer;
            _functionListInitializer = functionListInitializer;
            _loadContextSponsor = loadContextSponsor;
            _loadProjectProperties = loadProjectProperties;
            _themeManager = themeManager;
            _validateSelectedDocuments = validateSelectedDocuments;
            _validateSelectedRules = validateSelectedRules;
            _variableListInitializer = variableListInitializer;

            InitializeComponent();
            Initialize();
        }

        private readonly IDictionary<string, RadMenuItem> deployFileSystemApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> deleteFileSystemApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> deployWebApiApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> deleteWebApiApplicationMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> selectedApplicationRulesMenuItemList = new Dictionary<string, RadMenuItem>();
        private readonly IDictionary<string, RadMenuItem> validateApplicationRulesMenuItemList = new Dictionary<string, RadMenuItem>();

        #region Methods
        private async void Initialize()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Icon = _formInitializer.GetLogicBuilderIcon();
            }

            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);

            //_configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso\Contoso.lbproj");
            _configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\TelerikLogicBuilder\FlowProjects\Contoso.Test\Contoso.Test.lbproj");
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();

            UpdateApplicationMenuItems();

            SetSelectedApplication
            (
                /*Gets the first one if none have been selected*/
                _configurationService.GetSelectedApplication().Name
            );

            await LoadAssembliesOnProjectOpen();
        }

        private async Task LoadAssembliesOnProjectOpen()
        {
            radProgressBarElement1.Value1 = 50;
            radLabelElement1.Text = Strings.loadingAssemblies2;

            await _loadContextSponsor.LoadAssembiesOnOpenProject();

            radProgressBarElement1.Value1 = 0;
            radLabelElement1.Text = Strings.statusBarReadyMessage;
        }

        private async Task RunAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
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
                RadMessageBox.Show(ex.Message);
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
            }
        }

        private async Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
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
                        radProgressBarElement1.Value1 = 0;
                        radLabelElement1.Text = Strings.statusBarReadyMessage;
                    }
                    catch (LogicBuilderException ex)
                    {
                        radProgressBarElement1.Value1 = 0;
                        radLabelElement1.Text = ex.Message;
                        RadMessageBox.Show(ex.Message);
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
                    }
                },
                progress
            );
        }

        private void SetSelectedApplication(string applicationName)
        {
            _configurationService.SetSelectedApplication(applicationName);
            _checkSelectedApplication.CheckSelectedItem(radMenuItemSelectApplication.Items);
        }

        private static bool TryGetSelectedDocuments(out IList<string> selectedDocuments)
        {
            using IScopedDisposableManager<SelectDocumentsForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<SelectDocumentsForm>>();
            SelectDocumentsForm selectDocunentsForm = disposableManager.ScopedService;
            selectDocunentsForm.ShowDialog();

            if (selectDocunentsForm.DialogResult != System.Windows.Forms.DialogResult.OK
                || selectDocunentsForm.SourceFiles.Count == 0)
            {
                selectedDocuments = Array.Empty<string>();
                return false;
            }

            selectedDocuments = selectDocunentsForm.SourceFiles;
            return true;
        }

        private static bool TryGetSelectedRules(string selctedApplication, string title, out IList<string> selectedRules)
        {
            using IScopedDisposableManager<SelectRulesForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<SelectRulesForm>>();
            SelectRulesForm selectRulesForm = disposableManager.ScopedService;
            selectRulesForm.SetTitle(title);
            selectRulesForm.BuildTreeView(selctedApplication);
            selectRulesForm.ShowDialog();

            if (selectRulesForm.DialogResult != System.Windows.Forms.DialogResult.OK
                || selectRulesForm.SourceFiles.Count == 0)
            {
                selectedRules = Array.Empty<string>();
                return false;
            }

            selectedRules = selectRulesForm.SourceFiles;
            return true;
        }

        private static bool TryGetSelectedRulesResourcesPairs(string selctedApplication, string title, out IList<RulesResourcesPair> sourceFiles)
        {
            using IScopedDisposableManager<SelectRulesResourcesPairForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<SelectRulesResourcesPairForm>>();
            SelectRulesResourcesPairForm selectRulesForm = disposableManager.ScopedService;
            selectRulesForm.SetTitle(title);
            selectRulesForm.BuildTreeView(selctedApplication);
            selectRulesForm.ShowDialog();

            if (selectRulesForm.DialogResult != System.Windows.Forms.DialogResult.OK
                || selectRulesForm.SourceFiles.Count == 0)
            {
                sourceFiles = Array.Empty<RulesResourcesPair>();
                return false;
            }

            sourceFiles = selectRulesForm.SourceFiles;
            return true;
        }

        private void UpdateApplicationMenuItems()
        {
            List<Application> applicationList = new(_configurationService.ProjectProperties.ApplicationList.Values.OrderBy(a => a.Nickname));

            UpdateApplicationMenuItems(applicationList, radMenuItemFileSystemDeploy, deployFileSystemApplicationMenuItemList, DeployFileSystemRadMenuItem_Click);
            UpdateApplicationMenuItems(applicationList, radMenuItemFileSystemDelete, deleteFileSystemApplicationMenuItemList, DeleteFileSystemRadMenuItem_Click);
            UpdateApplicationMenuItems(applicationList, radMenuItemWebApiDeploy, deployWebApiApplicationMenuItemList, DeployWebApiRadMenuItem_Click);
            UpdateApplicationMenuItems(applicationList, radMenuItemWebApiDelete, deleteWebApiApplicationMenuItemList, DeleteWebApiRadMenuItem_Click);
            UpdateApplicationMenuItems(applicationList, radMenuItemValidateRules, validateApplicationRulesMenuItemList, ValidateRulesRadMenuItem_Click);
            UpdateApplicationMenuItems(applicationList, radMenuItemSelectApplication, selectedApplicationRulesMenuItemList, SelectedApplicationItem_Click, false);
        }

        private static void UpdateApplicationMenuItems(IList<Application> applicationList, RadMenuItem parentMenuItem, IDictionary<string, RadMenuItem> itemDictionary, EventHandler handler, bool addEllipsis = true)
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

                radMenuItem.Click += handler;
                itemDictionary.Add(application.Name, radMenuItem);
                parentMenuItem.Items.Add(radMenuItem);
            }
        }
        #endregion Methods

        #region Event Handlers
        private async void DeleteFileSystemRadMenuItem_Click(object? sender, EventArgs e)
        {
            string applicationName = (string)((RadMenuItem)sender!).Tag;
            if (
                    !TryGetSelectedRulesResourcesPairs
                    (
                        applicationName,
                        Strings.selectRulesToDelete,
                        out IList<RulesResourcesPair> sourceFiles
                    )
               )
            {
                return;
            }

            await RunAsync(DeleteSelectedRules);

            Task DeleteSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deleteSelectedFilesFromFileSystem.Delete
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }

        private async void DeployFileSystemRadMenuItem_Click(object? sender, EventArgs e)
        {
            string applicationName = (string)((RadMenuItem)sender!).Tag;
            if (
                    !TryGetSelectedRulesResourcesPairs
                    (
                        applicationName,
                        Strings.selectRulesToDeploy,
                        out IList<RulesResourcesPair> sourceFiles
                    )
               )
            {
                return;
            }

            await RunAsync(DeploySelectedRules);

            Task DeploySelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deploySelectedFilesToFileSystem.Deploy
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }

        private async void DeleteWebApiRadMenuItem_Click(object? sender, EventArgs e)
        {
            string applicationName = (string)((RadMenuItem)sender!).Tag;
            if (
                    !TryGetSelectedRulesResourcesPairs
                    (
                        applicationName,
                        Strings.selectRulesToDelete,
                        out IList<RulesResourcesPair> sourceFiles
                    )
               )
            {
                return;
            }

            await RunAsync(DeleteSelectedRules);

            Task DeleteSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deleteSelectedFilesFromApi.Delete
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }

        private async void DeployWebApiRadMenuItem_Click(object? sender, EventArgs e)
        {
            string applicationName = (string)((RadMenuItem)sender!).Tag;
            if (
                    !TryGetSelectedRulesResourcesPairs
                    (
                        applicationName,
                        Strings.selectRulesToDeploy,
                        out IList<RulesResourcesPair> sourceFiles
                    )
               )
            {
                return;
            }

            await RunAsync(DeploySelectedRules);

            Task DeploySelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _deploySelectedFilesToApi.Deploy
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }

        private async void ValidateRulesRadMenuItem_Click(object? sender, EventArgs e)
        {
            string applicationName = (string)((RadMenuItem)sender!).Tag;
            if (
                    !TryGetSelectedRules
                    (
                        applicationName, 
                        Strings.selectRulesToValidate, 
                        out IList<string> sourceFiles
                    )
               )
            {
                return;
            }

            await RunLoadContextAsync(ValidateSelectedRules);

            Task ValidateSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _validateSelectedRules.Validate
                (
                    sourceFiles,
                    _configurationService.GetApplication(applicationName)!,
                    progress,
                    cancellationTokenSource
                );
        }

        private void CommandBarButtonEdit_Click(object sender, EventArgs e)
        {
        }

        private void RadThemeMenuItem_Click(object sender, EventArgs e)
        {
            RadMenuItem clickedItem = (RadMenuItem)sender;
            _themeManager.SetTheme((string)clickedItem.Tag);
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);
        }

        private async void RadMenuItemBuildSelectedModules_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedDocuments(out IList<string> sourceFiles))
                return;

            await RunLoadContextAsync(BuildSelectedDocumentRules);

            Task BuildSelectedDocumentRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _buildSaveConsolidateSelectedDocumentRules.BuildRules
                (
                    sourceFiles,
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }

        private async void RadMenuItemValidateSelectedModules_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedDocuments(out IList<string> sourceFiles))
                return;

            await RunLoadContextAsync(ValidateSelectedDocuments);

            Task ValidateSelectedDocuments(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _validateSelectedDocuments.Validate
                (
                    sourceFiles,
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }

        private void SelectedApplicationItem_Click(object? sender, EventArgs e)
        {
            SetSelectedApplication((string)((RadMenuItem)sender!).Tag);
        }
        #endregion Event Handlers
    }
}
