using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator
{
    public class ValidateSelectedDocumentsTest : IClassFixture<ValidateSelectedDocumentsFixture>
    {
        private readonly ValidateSelectedDocumentsFixture _fixture;

        public ValidateSelectedDocumentsTest(ValidateSelectedDocumentsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateValidateSelectedDocuments()
        {
            //arrange
            IValidateSelectedDocuments validator = _fixture.ServiceProvider.GetRequiredService<IValidateSelectedDocuments>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public async Task ValidateSelectedDocumentsSucceeds()
        {
            //arrange
            using System.Windows.Forms.Form form = new MockMdiParent();
            ILoadContextSponsor loadContextSponsor = _fixture.LoadContextSponsor;
            IValidateSelectedDocuments validator = _fixture.ServiceProvider.GetRequiredService<IValidateSelectedDocuments>();
            IMainWindow mainWindow = _fixture.ServiceProvider.GetRequiredService<IMainWindow>();
            mainWindow.Instance = form;
            string sourceFile = GetFullSourceFilePath(nameof(ValidateSelectedDocumentsSucceeds));
            var application = _fixture.ConfigurationService.GetSelectedApplication();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> results = await _fixture.LoadContextSponsor.RunAsync
            (
                () => validator.Validate
                (
                    new List<string> { sourceFile },
                    application,
                    progress,
                    cancellationToken
                ),
                progress
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                Strings.validationSuccessful,
                results!.First().Message
            );
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
           => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(ValidateSelectedDocumentsTest)}\{fileNameNoExtension}.vsdx");

        private static void CloseVisioDocument(Document visioDocument)
        {
            visioDocument.Saved = true;
            visioDocument.Close();
        }

        class MockMessages : IMessages
        {
            public MessageTab SelectedMessageTab { set { } }

            public bool Visible { set { } }

            public void Clear(MessageTab messageTab)
            {
            }

            public void GoToNextEmptyLine(MessageTab messageTab)
            {
            }

            public void InsertLink(string text, string hyperlink, LinkType linkType, MessageTab messageTab)
            {
            }

            public void InsertLink(string text, string hyperlink, int position, LinkType linkType, MessageTab messageTab)
            {
            }

            public void InsertText(string text, MessageTab messageTab)
            {
            }

            public void InsertText(string text, int position, MessageTab messageTab)
            {
            }

            public void Select(int start, int length, MessageTab messageTab)
            {
            }
        }

        class MockMdiParent : System.Windows.Forms.Form, IMDIParent
        {
            public RadCommandBar CommandBar => throw new NotImplementedException();

            public CommandBarButton CommandBarButtonSave => throw new NotImplementedException();

            public IDocumentEditor? EditControl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public IMessages Messages => new MockMessages();

            public RadMenuItem RadMenuItemDelete => throw new NotImplementedException();

            public RadMenuItem RadMenuItemSave => throw new NotImplementedException();

            public RadMenuItem RadMenuItemUndo => throw new NotImplementedException();

            public RadMenuItem RadMenuItemRedo => throw new NotImplementedException();

            public RadMenuItem RadMenuItemIndexInformation => throw new NotImplementedException();

            public RadMenuItem RadMenuItemChaining => throw new NotImplementedException();

            public RadMenuItem RadMenuItemToggleActivateAll => throw new NotImplementedException();

            public RadMenuItem RadMenuItemToggleReevaluateAll => throw new NotImplementedException();

            public RadMenuItem RadMenuItemFullChaining => throw new NotImplementedException();

            public RadMenuItem RadMenuItemNoneChaining => throw new NotImplementedException();

            public RadMenuItem RadMenuItemUpdateOnlyChaining => throw new NotImplementedException();

            public void AddTableControl(IDocumentEditor documentEditor)
            {
                throw new NotImplementedException();
            }

            public void AddVisioControl(IDocumentEditor documentEditor)
            {
                throw new NotImplementedException();
            }

            public void ChangeCursor(System.Windows.Forms.Cursor cursor)
            {
                throw new NotImplementedException();
            }

            public void CloseProject()
            {
                throw new NotImplementedException();
            }

            public void OpenProject(string projectFileFullname)
            {
                throw new NotImplementedException();
            }

            public void RemoveEditControl()
            {
                throw new NotImplementedException();
            }

            public Task RunLoadContextAsync(Func<IProgress<ProgressMessage>, CancellationTokenSource, Task> task)
            {
                throw new NotImplementedException();
            }

            public Task RunLoadContextAsync(Func<CancellationTokenSource, Task> task)
            {
                throw new NotImplementedException();
            }

            public void SetEditControlMenuStates(bool visioOpen, bool tableOpen)
            {
                throw new NotImplementedException();
            }
        }
    }

    public class ValidateSelectedDocumentsFixture : IDisposable
    {
        public ValidateSelectedDocumentsFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"NotImportant",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    ),
                    ["app02"] = new Application
                    (
                        "App02",
                        "App02",
                        "Contoso.Test.Flow.dll",
                        $@"NotImportant",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["ClearErrorMessages"] = new Function
                    (
                        "ClearErrorMessages",
                        "Clear",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.Response.ErrorMessages",
                        "Field.Property.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Void, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["WriteToLog"] = new Function
                    (
                        "WriteToLog",
                        "WriteToLog",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            new LiteralParameter
                            (
                                "message",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                             )
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            VisioApplication = new InvisibleApp();
            //LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
            foreach (Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }

        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
    }
}
