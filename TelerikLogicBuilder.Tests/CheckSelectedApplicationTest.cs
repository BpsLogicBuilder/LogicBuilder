﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class CheckSelectedApplicationTest : IClassFixture<CheckSelectedApplicationFixture>
    {
        private readonly CheckSelectedApplicationFixture _fixture;

        public CheckSelectedApplicationTest(CheckSelectedApplicationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateCheckSelectedApplication()
        {
            //arrange
            ICheckSelectedApplication manager = _fixture.ServiceProvider.GetRequiredService<ICheckSelectedApplication>();

            //assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void CanCheckMenuItemToMatchCurrentTheme()
        {
            //arrange
            ICheckSelectedApplication manager = _fixture.ServiceProvider.GetRequiredService<ICheckSelectedApplication>();
            List<Application> applicationList = new(_fixture.ConfigurationService.ProjectProperties.ApplicationList.Values.OrderBy(a => a.Nickname));
            RadMenuItem radMenuItemParent = new();
            IDictionary<string, RadMenuItem> menuItemTable = new Dictionary<string, RadMenuItem>();
            applicationList.ForEach
            (
                application =>
                {
                    RadMenuItem radMenuItem = new
                    (
                        application.Nickname,
                        application.Name
                    );

                    menuItemTable.Add(application.Name, radMenuItem);
                    radMenuItemParent.Items.Add(radMenuItem);
                }
            );

            //act
            _fixture.ConfigurationService.SetSelectedApplication("App02");
            manager.CheckSelectedItem(radMenuItemParent.Items);

            //assert
            Assert.False(menuItemTable["App01"].IsChecked);
            Assert.True(menuItemTable["App02"].IsChecked);
        }
    }

    public class CheckSelectedApplicationFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;

        public CheckSelectedApplicationFixture()
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
