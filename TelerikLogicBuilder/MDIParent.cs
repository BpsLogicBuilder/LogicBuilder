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
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal partial class MDIParent : Telerik.WinControls.UI.RadForm
    {
        private readonly IBuildSaveAssembleRulesForSelectedDocuments _buildSaveConsolidateSelectedDocumentRules;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorListInitializer _constructorListInitializer;
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
            IConfigurationService configurationService,
            IConstructorListInitializer constructorListInitializer,
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
            _configurationService = configurationService;
            _constructorListInitializer = constructorListInitializer;
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

        private void Initialize()
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Icon = _formInitializer.GetLogicBuilderIcon();
            }

            commandBarStripElement1.OverflowButton.Visibility = ElementVisibility.Collapsed;
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);

            _configurationService.ProjectProperties = _loadProjectProperties.Load(@"C:\TelerikLogicBuilder\FlowProjects\Contoso.Test\Contoso.Test.lbproj");
            _configurationService.ConstructorList = _constructorListInitializer.InitializeList();
            _configurationService.FragmentList = _fragmentListInitializer.InitializeList();
            _configurationService.FunctionList = _functionListInitializer.InitializeList();
            _configurationService.VariableList = _variableListInitializer.InitializeList();
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

        private async void CommandBarButtonEdit_Click(object sender, EventArgs e)
        {
            using IScopedDisposableManager<SelectDocumentsForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<SelectDocumentsForm>>();
            SelectDocumentsForm selectDocunentsForm = disposableManager.ScopedService;
            selectDocunentsForm.ShowDialog();

            if (selectDocunentsForm.DialogResult != System.Windows.Forms.DialogResult.OK
                || selectDocunentsForm.SourceFiles.Count == 0)
                return;
            /*_configurationService.SetSelectedApplication("App01");
            using IScopedDisposableManager<SelectRulesForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<SelectRulesForm>>();
            SelectRulesForm selectRulesForm = disposableManager.ScopedService;
            selectRulesForm.SetTitle("Select Rules to Validate");
            selectRulesForm.ShowDialog();

            if (selectRulesForm.DialogResult != System.Windows.Forms.DialogResult.OK
                || selectRulesForm.SourceFiles.Count == 0)
                return;*/

            //Save the Current file in Edit Control

            //Check Visio Version Installed

            //await RunLoadContextAsync(ValidateSelectedRules);
            await RunLoadContextAsync(BuildSelectedDocumentRules);
            //Task ValidateSelectedRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            //    => _validateSelectedRules.Validate
            //    (
            //        selectRulesForm.SourceFiles,
            //        _configurationService.GetSelectedApplication(),
            //        progress,
            //        cancellationTokenSource
            //    );

            //Task ValidateSelectedDocuments(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource) 
            //    => _validateSelectedDocuments.Validate
            //    (
            //        selectDocunentsForm.SourceFiles,
            //        _configurationService.GetSelectedApplication(),
            //        progress,
            //        cancellationTokenSource
            //    );

            Task BuildSelectedDocumentRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
                => _buildSaveConsolidateSelectedDocumentRules.BuildRules
                (
                    selectDocunentsForm.SourceFiles,
                    _configurationService.GetSelectedApplication(),
                    progress,
                    cancellationTokenSource
                );
        }

        private void RadThemeMenuItem_Click(object sender, EventArgs e)
        {
            RadMenuItem clickedItem = (RadMenuItem)sender;
            _themeManager.SetTheme((string)clickedItem.Tag);
            _themeManager.CheckMenuItemForCurrentTheme(this.radMenuItemTheme.Items);
        }
    }
}
