using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.Tests.Editing.FindAndReplace
{
    public class SearchSelectedDocumentsTest : IClassFixture<SearchSelectedDocumentsFixture>
    {
        private readonly SearchSelectedDocumentsFixture _fixture;

        public SearchSelectedDocumentsTest(SearchSelectedDocumentsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateDiagramSearcher()
        {
            //arrange
            ISearchSelectedDocuments helper = _fixture.ServiceProvider.GetRequiredService<ISearchSelectedDocuments>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData("required", true)]
        [InlineData("StringNotPresent", false)]
        public async Task TextSearchWorks(string searchString, bool found)
        {
            //arrange
            using System.Windows.Forms.Form form = new MockMdiParent();
            ISearchSelectedDocuments helper = _fixture.ServiceProvider.GetRequiredService<ISearchSelectedDocuments>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            IMainWindow mainWindow = _fixture.ServiceProvider.GetRequiredService<IMainWindow>();
            mainWindow.Instance = form;
            string diagramSourceFile = GetFullDiagramSourceFilePath(nameof(TextSearchWorks));
            string tableourceFile = GetFullTableSourceFilePath();

            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await helper.Search
            (
                new string[] { diagramSourceFile, tableourceFile },
                searchString,
                false,
                false,
                searchFunctions.FindTextMatches,
                progress,
                cancellationToken
            );

            Assert.Equal(result.Count > 1, found);
        }

        [Theory]
        [InlineData("CommandButtonParameters", true)]
        [InlineData("StringNotPresent", false)]
        public async Task ConstructorSearchWorks(string searchString, bool found)
        {
            //arrange
            using System.Windows.Forms.Form form = new MockMdiParent();
            ISearchSelectedDocuments helper = _fixture.ServiceProvider.GetRequiredService<ISearchSelectedDocuments>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            IMainWindow mainWindow = _fixture.ServiceProvider.GetRequiredService<IMainWindow>();
            mainWindow.Instance = form;
            string diagramSourceFile = GetFullDiagramSourceFilePath(nameof(ConstructorSearchWorks));
            string tableourceFile = GetFullTableSourceFilePath();

            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await helper.Search
            (
                new string[] { diagramSourceFile, tableourceFile },
                searchString,
                false,
                false,
                searchFunctions.FindConstructorMatches,
                progress,
                cancellationToken
            );

            Assert.Equal(result.Count > 1, found);
        }

        [Theory]
        [InlineData("Set Variable", true)]
        [InlineData("StringNotPresent", false)]
        public async Task FunctionSearchWorks(string searchString, bool found)
        {
            //arrange
            using System.Windows.Forms.Form form = new MockMdiParent();
            ISearchSelectedDocuments helper = _fixture.ServiceProvider.GetRequiredService<ISearchSelectedDocuments>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            IMainWindow mainWindow = _fixture.ServiceProvider.GetRequiredService<IMainWindow>();
            mainWindow.Instance = form;
            string diagramSourceFile = GetFullDiagramSourceFilePath(nameof(FunctionSearchWorks));
            string tableourceFile = GetFullTableSourceFilePath();

            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await helper.Search
            (
                new string[] { diagramSourceFile, tableourceFile },
                searchString,
                false,
                false,
                searchFunctions.FindFunctionMatches,
                progress,
                cancellationToken
            );

            Assert.Equal(result.Count > 1, found);
        }

        [Theory]
        [InlineData("SomeObject", true)]
        [InlineData("StringNotPresent", false)]
        public async Task VariableSearchWorks(string searchString, bool found)
        {
            //arrange
            using System.Windows.Forms.Form form = new MockMdiParent();
            ISearchSelectedDocuments helper = _fixture.ServiceProvider.GetRequiredService<ISearchSelectedDocuments>();
            ISearchFunctions searchFunctions = _fixture.ServiceProvider.GetRequiredService<ISearchFunctions>();
            IMainWindow mainWindow = _fixture.ServiceProvider.GetRequiredService<IMainWindow>();
            mainWindow.Instance = form;
            string diagramSourceFile = GetFullDiagramSourceFilePath(nameof(VariableSearchWorks));
            string tableourceFile = GetFullTableSourceFilePath();

            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            var result = await helper.Search
            (
                new string[] { diagramSourceFile, tableourceFile },
                searchString,
                false,
                false,
                searchFunctions.FindVariableMatches,
                progress,
                cancellationToken
            );

            Assert.Equal(result.Count > 1, found);
        }

        private static string GetFullDiagramSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(SearchSelectedDocumentsTest)}\{fileNameNoExtension}.vsd");

        private static string GetFullTableSourceFilePath()
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(SearchSelectedDocumentsTest)}\Module.tbl");
    }

    public class SearchSelectedDocumentsFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IFunctionFactory FunctionFactory;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;

        public SearchSelectedDocumentsFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ProjectPropertiesItemFactory.GetApplication
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["DisplayEditForm"] = FunctionFactory.GetFunction
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
                            ParameterFactory.GetObjectParameter
                            (
                                "setting",
                                false,
                                "",
                                "Contoso.Forms.Parameters.DataForm.DataFormSettingsParameters",
                                true,
                                false,
                                true
                             )
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["SetupNavigationMenu"] = FunctionFactory.GetFunction
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
                            ParameterFactory.GetObjectParameter
                            (
                                "navBar",
                                false,
                                "",
                                "Contoso.Forms.Parameters.Navigation.NavigationBarParameters",
                                true,
                                false,
                                true
                             )
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    )
                },
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
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
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
