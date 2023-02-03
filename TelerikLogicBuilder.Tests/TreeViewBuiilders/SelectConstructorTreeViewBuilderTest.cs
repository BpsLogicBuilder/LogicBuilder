using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Constants;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class SelectConstructorTreeViewBuilderTest : IClassFixture<SelectConstructorTreeViewBuilderFixture>
    {
        private readonly SelectConstructorTreeViewBuilderFixture _fixture;

        public SelectConstructorTreeViewBuilderTest(SelectConstructorTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateEditConstructorTreeViewBuilderThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(_fixture.ServiceProvider.GetRequiredService<ISelectConstructorTreeViewBuilder>);
        }

        [Fact]
        public void CanBuildTreeView()
        {
            //arrange
            ITreeViewBuilderFactory factory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            RadTreeView radTreeView = new();
            Dictionary<string, string> expandedNodes = new();

            //act
            factory.GetSelectConstructorTreeViewBuilder
            (
                new SelectConstructorControlMock(expandedNodes)
            ).Build(radTreeView);

            //assert
            Assert.NotNull(radTreeView.Nodes[0]);
            Assert.True(radTreeView.Nodes[0].Nodes.Count > 0);
        }
    }

    public class SelectConstructorTreeViewBuilderFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
        internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IParameterFactory ParameterFactory;

        public SelectConstructorTreeViewBuilderFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
            WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
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

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = ConstructorFactory.GetConstructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "stringProperty",
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
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ""
                    ),
                    ["Object"] = ConstructorFactory.GetConstructor
                    (
                        "Object",
                        "System.Object",
                        new List<ParameterBase>
                        {
                        },
                        new List<string>(),
                        ""
                    ),
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            foreach (string key in ConfigurationService.ConstructorList.Constructors.Keys)
                ConfigurationService.ConstructorList.ConstructorsTreeFolder.FileNames.Add(key);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
