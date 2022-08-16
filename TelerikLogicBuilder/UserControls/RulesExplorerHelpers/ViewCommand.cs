using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers.Forms;
using LogicBuilder.Workflow.Activities.Rules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers
{
    internal class ViewCommand : ClickCommandBase
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly ILongStringManager _longStringManager;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;
        private readonly IRuleSetLoader _ruleSetLoader;
        private readonly IRulesValidator _rulesValidator;
        private readonly UiNotificationService _uiNotificationService;

        public ViewCommand(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            ILongStringManager longStringManager,
            IMainWindow mainWindow,
            ITreeViewService treeViewService,
            IRuleSetLoader ruleSetLoader,
            IRulesValidator rulesValidator,
            UiNotificationService uiNotificationService)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _longStringManager = longStringManager;
            _mainWindow = mainWindow;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _ruleSetLoader = ruleSetLoader;
            _rulesValidator = rulesValidator;
        }

        public async override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            try
            {
                RadTreeNode? selectedNode = _mainWindow.RulesExplorer.TreeView.SelectedNode;
                if (selectedNode == null ||
                    !_treeViewService.IsFileNode(selectedNode))
                    return;

                mdiParent.ChangeCursor(Cursors.WaitCursor);
                await mdiParent.RunLoadContextAsync(View);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                mdiParent.ChangeCursor(Cursors.Default);
            }

            async Task View(CancellationTokenSource cancellationTokenSource)
            {
                RadTreeNode? selectedNode = _mainWindow.RulesExplorer.TreeView.SelectedNode;
                if (selectedNode == null)
                    throw _exceptionHelper.CriticalException("{57A4930B-6DB2-4C66-A5FB-FB2404672394}");

                Application application = _configurationService.GetApplicationFromPath(selectedNode.Name);

                if (selectedNode.Name.EndsWith(FileExtensions.RULESFILEEXTENSION, true, CultureInfo.InvariantCulture))
                {
                    await ViewSelectedRules();
                }
                else if (selectedNode.Name.EndsWith(FileExtensions.RESOURCEFILEEXTENSION, true, CultureInfo.InvariantCulture))
                {
                    
                    using IResourceReader reader = _fileIOHelper.GetResourceReader(selectedNode.Name);
                    string[] entries = reader.OfType<DictionaryEntry>()
                                        .Select
                                        (
                                            entry => $"{entry.Key}{Strings.equalSign}{_longStringManager.GetLongStringForText((string)entry.Value!, application.Runtime)}"
                                        )
                                        .ToArray();

                    using IScopedDisposableManager<TextViewer> textViewerManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
                    TextViewer textViewer = textViewerManager.ScopedService;
                    textViewer.SetText(entries);
                    textViewer.ShowDialog(_mainWindow.Instance);

                }
                else if (selectedNode.Name.EndsWith(FileExtensions.RESOURCETEXTFILEEXTENSION, true, CultureInfo.InvariantCulture))
                {
                    using StreamReader inStream = new(selectedNode.Name, Encoding.Unicode);
                    using IScopedDisposableManager<TextViewer> textViewerManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
                    TextViewer textViewer = textViewerManager.ScopedService;
                    textViewer.SetText(inStream.ReadToEnd());
                    textViewer.ShowDialog(_mainWindow.Instance);
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{C9C28F1D-EA89-48F5-8F40-4D8E23B3E536}");
                }

                async Task ViewSelectedRules()
                {
                    ApplicationTypeInfo applicationTypeInfo = _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name);
                    if (!applicationTypeInfo.AssemblyAvailable)
                    {
                        //validationMessages.Add(new ResultMessage(applicationTypeInfo.UnavailableMessage));
                        DisplayMessage.Show(_mainWindow.Instance, applicationTypeInfo.UnavailableMessage);
                        return;
                    }

                    RuleSet ruleSet = _ruleSetLoader.LoadRuleSet(selectedNode.Name);
                    IList<ResultMessage> validationMessages = await _rulesValidator.Validate
                    (
                        ruleSet,
                        applicationTypeInfo,
                        cancellationTokenSource
                    );

                    if (validationMessages.Count > 0)
                    {
                        using IScopedDisposableManager<TextViewer> textViewerManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
                        TextViewer textViewer = textViewerManager.ScopedService;
                        textViewer.SetText(validationMessages.Select(m => m.Message).ToArray());
                        textViewer.ShowDialog(_mainWindow.Instance);
                    }

                    using IScopedDisposableManager<RadRuleSetDialog> ruleSetDialogManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<RadRuleSetDialog>>();
                    RadRuleSetDialog ruleSetDialog = ruleSetDialogManager.ScopedService;
                    ruleSetDialog.Setup(applicationTypeInfo.ActivityType, ruleSet, applicationTypeInfo.AllAssemblies);
                    ruleSetDialog.ShowDialog(_mainWindow.Instance);
                }
            }
        }
    }
}
