using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class SelectModulesForDeploymentTreeViewBuilderTest : IClassFixture<SelectModulesForDeploymentTreeViewBuilderFixture>
    {
        private readonly SelectModulesForDeploymentTreeViewBuilderFixture _fixture;

        public SelectModulesForDeploymentTreeViewBuilderTest(SelectModulesForDeploymentTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateSelectRulesTreeViewBuilder()
        {
            //arrange
            ISelectModulesForDeploymentTreeViewBuilder service = _fixture.ServiceProvider.GetRequiredService<ISelectModulesForDeploymentTreeViewBuilder>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CanBuildTreeView()
        {
            //arrange
            ISelectModulesForDeploymentTreeViewBuilder service = _fixture.ServiceProvider.GetRequiredService<ISelectModulesForDeploymentTreeViewBuilder>();
            RadTreeView radTreeView = new();

            //act
            service.Build(radTreeView, "App01");

            //assert
            Assert.NotNull(radTreeView.Nodes[0]);
            Assert.True(radTreeView.Nodes[0].Nodes.Count > 0);
            Assert.True(radTreeView.Nodes[0].Nodes[0].Tag is RulesResourcesPair);
            RulesResourcesPair rulesResourcesPair = (RulesResourcesPair)radTreeView.Nodes[0].Nodes[0].Tag;
            Assert.True(File.Exists(rulesResourcesPair.RulesFile));
            Assert.True(File.Exists(rulesResourcesPair.ResourcesFile));
        }
    }

    public class SelectModulesForDeploymentTreeViewBuilderFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;

        public SelectModulesForDeploymentTreeViewBuilderFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                $@"{TestFolders.TestAssembliesFolder}\FlowProjects\Contoso.Test",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ProjectPropertiesItemFactory.GetApplication
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
