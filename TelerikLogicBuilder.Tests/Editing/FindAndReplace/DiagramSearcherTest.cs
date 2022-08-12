using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.Tests.Editing.FindAndReplace
{
    public class DiagramSearcherTest : IClassFixture<DiagramSearcherFixture>
    {
        private readonly DiagramSearcherFixture _fixture;

        public DiagramSearcherTest(DiagramSearcherFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateDiagramSearcher()
        {
            //arrange
            IDiagramSearcher helper = _fixture.ServiceProvider.GetRequiredService<IDiagramSearcher>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData("required", true, true, true)]
        [InlineData("requir", true, true, false)]
        [InlineData("requir", true, false, true)]
        [InlineData("REQUIR", true, false, false)]
        [InlineData("REQUIRED", false, true, true)]
        [InlineData("REQUIREDD", false, true, false)]
        public async Task TextSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            IDiagramSearcher helper = _fixture.ServiceProvider.GetRequiredService<IDiagramSearcher>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(nameof(TextSearchReturnsTheExpectedResults));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            var result = await helper.Search
            (
                sourceFile,
                visioDocument,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindTextMatches,
                progress,
                cancellationToken
            );

            CloseVisioDocument(visioDocument);

            Assert.Equal
            (
                found,
                result.ShapeCount > 0
            );
        }

        [Theory]
        [InlineData("CommandButtonParameters", true, true, true)]
        [InlineData("CommandButtonParameter", true, true, false)]
        [InlineData("CommandButtonParamet", true, false, true)]
        [InlineData("COMMANDBUTTONPARAMET", true, false, false)]
        [InlineData("COMMANDBUTTONPARAMETERS", false, true, true)]
        [InlineData("COMMANDBUTTONPARAMETERSs", false, true, false)]
        public async Task ConstructorSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            IDiagramSearcher helper = _fixture.ServiceProvider.GetRequiredService<IDiagramSearcher>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(nameof(ConstructorSearchReturnsTheExpectedResults));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            var result = await helper.Search
            (
                sourceFile,
                visioDocument,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindConstructorMatches,
                progress,
                cancellationToken
            );

            CloseVisioDocument(visioDocument);

            Assert.Equal
            (
                found,
                result.ShapeCount > 0
            );
        }

        [Theory]
        [InlineData("Set Variable", true, true, true)]
        [InlineData("Set Variabl", true, true, false)]
        [InlineData("Set Variabl", true, false, true)]
        [InlineData("SET VARIABL", true, false, false)]
        [InlineData("SET VARIABLE", false, true, true)]
        [InlineData("SET VARIABLEs", false, true, false)]
        public async Task FunctionSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            IDiagramSearcher helper = _fixture.ServiceProvider.GetRequiredService<IDiagramSearcher>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(nameof(FunctionSearchReturnsTheExpectedResults));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            var result = await helper.Search
            (
                sourceFile,
                visioDocument,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindFunctionMatches,
                progress,
                cancellationToken
            );

            CloseVisioDocument(visioDocument);

            Assert.Equal
            (
                found,
                result.ShapeCount > 0
            );
        }

        [Theory]
        [InlineData("SomeObject", true, true, true)]
        [InlineData("SomeObjec", true, true, false)]
        [InlineData("SomeObjec", true, false, true)]
        [InlineData("SOMEOBJEC", true, false, false)]
        [InlineData("SOMEOBJECT", false, true, true)]
        [InlineData("SOMEOBJECTs", false, true, false)]
        public async Task VariableSearchReturnsTheExpectedResults(string searchString, bool matchCase, bool matchWholeWord, bool found)
        {
            //arrange
            IDiagramSearcher helper = _fixture.ServiceProvider.GetRequiredService<IDiagramSearcher>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            string sourceFile = GetFullSourceFilePath(nameof(VariableSearchReturnsTheExpectedResults));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            var result = await helper.Search
            (
                sourceFile,
                visioDocument,
                searchString,
                matchCase,
                matchWholeWord,
                searchFunctions.FindVariableMatches,
                progress,
                cancellationToken
            );

            CloseVisioDocument(visioDocument);

            Assert.Equal
            (
                found,
                result.ShapeCount > 0
            );
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(DiagramSearcherTest)}\{fileNameNoExtension}.vsd");

        private static void CloseVisioDocument(Document visioDocument)
        {
            visioDocument.Saved = true;
            visioDocument.Close();
        }
    }

    public class DiagramSearcherFixture : IDisposable
    {
        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;

        public DiagramSearcherFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
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
                    ["DisplayEditForm"] = new Function
                    (
                        "DisplayEditForm",
                        "DisplayEditForm",
                        FunctionCategories.DialogForm,
                        "",
                        "flowManager.DialogFunctions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            new ObjectParameter
                            (
                                "setting",
                                false,
                                "",
                                "Contoso.Forms.Parameters.DataForm.DataFormSettingsParameters",
                                true,
                                false,
                                true,
                                ContextProvider
                             )
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["SetupNavigationMenu"] = new Function
                    (
                        "SetupNavigationMenu",
                        "UpdateNavigationBar",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.Actions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            new ObjectParameter
                            (
                                "navBar",
                                false,
                                "",
                                "Contoso.Forms.Parameters.Navigation.NavigationBarParameters",
                                true,
                                false,
                                true,
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
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }
    }
}
