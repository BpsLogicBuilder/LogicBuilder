using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TelerikLogicBuilder.Tests.Constants;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;
using SearchOptions = ABIS.LogicBuilder.FlowBuilder.Enums.SearchOptions;

namespace TelerikLogicBuilder.Tests.Editing.FindAndReplace
{
    public class GetSourceFilesForDocumentSearchTest : IClassFixture<GetSourceFilesForDocumentSearchFixture>
    {
        private readonly GetSourceFilesForDocumentSearchFixture _fixture;

        public GetSourceFilesForDocumentSearchTest(GetSourceFilesForDocumentSearchFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateGetSourceFilesForDocumentSearch()
        {
            //arrange
            IGetSourceFilesForDocumentSearch service = _fixture.ServiceProvider.GetRequiredService<IGetSourceFilesForDocumentSearch>();

            //assert
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(SearchOptions.All, true)]
        [InlineData(SearchOptions.OpenDocuments, false)]
        internal void GetSourceFilesWorks(SearchOptions searchOptions, bool found)
        {
            //arrange
            IGetSourceFilesForDocumentSearch service = _fixture.ServiceProvider.GetRequiredService<IGetSourceFilesForDocumentSearch>();
            using System.Windows.Forms.Form form = new MockMdiParent();
            IMainWindow mainWindow = _fixture.ServiceProvider.GetRequiredService<IMainWindow>();
            mainWindow.Instance = form;

            //act
            var result = service.GetSourceFiles("*", searchOptions);

            //assert
            Assert.Equal(found, result.Count > 0);
        }
    }

    public class GetSourceFilesForDocumentSearchFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;

        public GetSourceFilesForDocumentSearchFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                $@"{TestFolders.TestAssembliesFolder}\FlowProjects\Contoso.Test",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
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
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
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

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
