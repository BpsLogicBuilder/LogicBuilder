using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator.Forms
{
    public class SelectRulesResourcesPairFormFactoryTest : IClassFixture<SelectRulesResourcesPairFormFactoryFixture>
    {
        private readonly SelectRulesResourcesPairFormFactoryFixture _fixture;

        public SelectRulesResourcesPairFormFactoryTest(SelectRulesResourcesPairFormFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void SelectRulesResourcesPairFormFactory()
        {
            //arrange
            using ISelectRulesResourcesPairFormFactory factory = _fixture.ServiceProvider.GetRequiredService<ISelectRulesResourcesPairFormFactory>();

            //assert
            Assert.NotNull(factory);
        }

        [Fact]
        public void DisposesOfScopedService()
        {
            //arrange
            ISelectRulesResourcesPairFormFactory factory = _fixture.ServiceProvider.GetRequiredService<ISelectRulesResourcesPairFormFactory>();
            SelectRulesResourcesPairForm scopedDisposable = factory.GetScopedService("App01");

            //act
            Assert.False(scopedDisposable.IsDisposed);
            factory.Dispose();
            Assert.True(scopedDisposable.IsDisposed);
        }
    }

    public class SelectRulesResourcesPairFormFactoryFixture : IDisposable
    {
        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;

        public SelectRulesResourcesPairFormFactoryFixture()
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
