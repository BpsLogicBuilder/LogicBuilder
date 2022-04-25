using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests
{
    public class ModuleNamesReaderTest : IClassFixture<ModuleNamesReaderFixture>
    {
        private readonly ModuleNamesReaderFixture _fixture;

        public ModuleNamesReaderTest(ModuleNamesReaderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateModuleNamesReader()
        {
            //arrange
            IModuleNamesReader reader = _fixture.ServiceProvider.GetRequiredService<IModuleNamesReader>();

            //assert
            Assert.NotNull(reader);
        }

        [Fact]
        public void ModuleNamesReaderWorks()
        {
            //arrange
            IModuleNamesReader reader = _fixture.ServiceProvider.GetRequiredService<IModuleNamesReader>();

            //act
            var result = reader.GetNames();

            //assert
            Assert.NotEmpty(result);
        }
    }

    public class ModuleNamesReaderFixture : IDisposable
    {
        public ModuleNamesReaderFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso.Test",
                @"C:\TelerikLogicBuilder\FlowProjects\Contoso.Test",
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

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
    }
}
