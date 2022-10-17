﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
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
    public class SelectDocunentsTreeViewBuilderTest : IClassFixture<SelectDocunentsTreeViewBuilderFixture>
    {
        private readonly SelectDocunentsTreeViewBuilderFixture _fixture;

        public SelectDocunentsTreeViewBuilderTest(SelectDocunentsTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateSelectDocunentsTreeViewBuilder()
        {
            //arrange
            ISelectDocunentsTreeViewBuilder service = _fixture.ServiceProvider.GetRequiredService<ISelectDocunentsTreeViewBuilder>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CanBuildTreeView()
        {
            //arrange
            ISelectDocunentsTreeViewBuilder service = _fixture.ServiceProvider.GetRequiredService<ISelectDocunentsTreeViewBuilder>();
            RadTreeView radTreeView = new();

            //act
            service.Build(radTreeView);
            
            //assert
            Assert.NotNull(radTreeView.Nodes[0]);
            Assert.True(radTreeView.Nodes[0].Nodes.Count > 0);
        }
    }

    public class SelectDocunentsTreeViewBuilderFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;

        public SelectDocunentsTreeViewBuilderFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = ConfigurationItemFactory.GetProjectProperties
            (
                "Contoso",
                $@"{TestFolders.TestAssembliesFolder}\FlowProjects\Contoso.Test",
                new Dictionary<string, Application>
                {
                    ["app01"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
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
