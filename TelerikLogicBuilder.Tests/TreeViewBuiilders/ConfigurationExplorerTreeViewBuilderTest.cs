using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class ConfigurationExplorerTreeViewBuilderTest : IClassFixture<ConfigurationExplorerTreeViewBuilderFixture>
    {
        private readonly ConfigurationExplorerTreeViewBuilderFixture _fixture;

        public ConfigurationExplorerTreeViewBuilderTest(ConfigurationExplorerTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateConfigurationExplorerTreeViewBuilder()
        {
            //arrange
            IConfigurationExplorerTreeViewBuilder service = _fixture.ServiceProvider.GetRequiredService<IConfigurationExplorerTreeViewBuilder>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CanBuildTreeView()
        {
            //arrange
            IConfigurationExplorerTreeViewBuilder service = _fixture.ServiceProvider.GetRequiredService<IConfigurationExplorerTreeViewBuilder>();
            RadTreeView radTreeView = new();

            //act
            service.Build(radTreeView);

            //assert
            Assert.NotNull(radTreeView.Nodes[0]);//project
            Assert.True(radTreeView.Nodes[0].Nodes.Count  == 5);//files
        }
    }

    public class ConfigurationExplorerTreeViewBuilderFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;

        public ConfigurationExplorerTreeViewBuilderFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso.Test",
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
