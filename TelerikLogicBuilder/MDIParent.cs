using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Forms;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : RadForm
    {
       private readonly ICheckSelectedApplication _checkSelectedApplication;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
        private readonly IFormInitializer _formInitializer;
        private readonly IFragmentListInitializer _fragmentListInitializer;
        private readonly IFunctionListInitializer _functionListInitializer;
        private readonly ILoadContextSponsor _loadContextSponsor;
        private readonly ILoadProjectProperties _loadProjectProperties;
        private readonly IThemeManager _themeManager;
        private readonly IVariableListInitializer _variableListInitializer;

        private readonly Func<MDIParent, BuildSaveConsolidateSelectedDocumentsCommand> _getBuildSaveConsolidateSelectedDocumentsCommand;
        private readonly Func<MDIParent, string, DeleteSelectedFilesFromApiCommand> _getDeleteSelectedFilesFromApiCommand;
        private readonly Func<MDIParent, string, DeleteSelectedFilesFromFileSystemCommand> _getDeleteSelectedFilesFromFileSystemCommand;
        private readonly Func<MDIParent, string, DeploySelectedFilesToApiCommand> _getDeploySelectedFilesToApiCommand;
        private readonly Func<MDIParent, string, DeploySelectedFilesToFileSystemCommand> _getDeploySelectedFilesToFileSystemCommand;
        private readonly Func<RadMenuItem, string, SetSelectedApplicationCommand> _getSetSelectedApplicationCommand;
        private readonly Func<RadMenuItem, string, SetThemeCommand> _getSetThemeCommand;
        private readonly Func<MDIParent, ValidateSelectedDocumentsCommand> _getValidateSelectedDocumentsCommand;
        private readonly Func<MDIParent, string, ValidateSelectedRulesCommand> _getValidateSelectedRulesCommand;

        //controls
        private readonly ProjectExplorer _projectExplorer;

        public MDIParent(
            ICheckSelectedApplication checkSelectedApplication,
            IConfigurationService configurationService,
            IConstructorListInitializer constructorListInitializer,
            IFormInitializer formInitializer,
            IFragmentListInitializer fragmentListInitializer,
            IFunctionListInitializer functionListInitializer,
            ILoadContextSponsor loadContextSponsor,
            ILoadProjectProperties loadProjectProperties,
            IThemeManager themeManager,
            IVariableListInitializer variableListInitializer,
            Func<MDIParent, BuildSaveConsolidateSelectedDocumentsCommand> getBuildSaveConsolidateSelectedDocumentsCommand,
            Func<MDIParent, string, DeleteSelectedFilesFromApiCommand> getDeleteSelectedFilesFromApiCommand,
            Func<MDIParent, string, DeleteSelectedFilesFromFileSystemCommand> getDeleteSelectedFilesFromFileSystemCommand,
            Func<MDIParent, string, DeploySelectedFilesToApiCommand> getDeploySelectedFilesToApiCommand,
            Func<MDIParent, string, DeploySelectedFilesToFileSystemCommand> getDeploySelectedFilesToFileSystemCommand,
            Func<RadMenuItem, string, SetSelectedApplicationCommand> getSetSelectedApplicationCommand,
            Func<RadMenuItem, string, SetThemeCommand> getSetThemeCommand,
            Func<MDIParent, ValidateSelectedDocumentsCommand> getValidateSelectedDocumentsCommand,
            Func<MDIParent, string, ValidateSelectedRulesCommand> getValidateSelectedRulesCommand,
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
            _themeManager = themeManager;
            _getSetSelectedApplicationCommand = getSetSelectedApplicationCommand;
            _getSetThemeCommand = getSetThemeCommand;
            _getValidateSelectedDocumentsCommand = getValidateSelectedDocumentsCommand;
            _getValidateSelectedRulesCommand = getValidateSelectedRulesCommand;
            _variableListInitializer = variableListInitializer;

            _getBuildSaveConsolidateSelectedDocumentsCommand = getBuildSaveConsolidateSelectedDocumentsCommand;
            _projectExplorer = projectExplorer;

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
                _projectExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitPanelExplorer.SuspendLayout();
                this.splitPanelExplorer.Controls.Add(_projectExplorer);
                this.splitPanelExplorer.ResumeLayout();
            }

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

            AddThemeMenuItemClickCommands(this.radMenuItemTheme);

            await LoadAssembliesOnProjectOpen();
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

        internal async Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
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

        private void AddClickCommand(RadMenuItem menuItem, IClickCommand command)
        {
            menuItem.Click += (sender, args) => command.Execute();
        }

        private async Task LoadAssembliesOnProjectOpen()
        {
            radProgressBarElement1.Value1 = 50;
            radLabelElement1.Text = Strings.loadingAssemblies2;

            await _loadContextSponsor.LoadAssembiesOnOpenProject();

            radProgressBarElement1.Value1 = 0;
            radLabelElement1.Text = Strings.statusBarReadyMessage;
        }

        private void SetSelectedApplication(string applicationName)
        {
            _configurationService.SetSelectedApplication(applicationName);
            _checkSelectedApplication.CheckSelectedItem(radMenuItemSelectApplication.Items);
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

        private void UpdateApplicationMenuItems(IList<Application> applicationList, RadMenuItem parentMenuItem, IDictionary<string, RadMenuItem> itemDictionary, Func<string, IClickCommand> getClickCommand, bool addEllipsis = true)
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

        private void AddThemeMenuItemClickCommands(RadMenuItem themetMenuItem)
        {
            foreach (RadMenuItem radMenuItem in themetMenuItem.Items)
            {
                AddClickCommand(radMenuItem, _getSetThemeCommand(themetMenuItem, (string)radMenuItem.Tag));
            }
        }
        #endregion Methods

        #region Event Handlers
        #endregion Event Handlers
    }
}
