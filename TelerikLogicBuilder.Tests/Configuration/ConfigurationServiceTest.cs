using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class ConfigurationServiceTest
    {
        public ConfigurationServiceTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanCreateConfigurationService()
        {
            //act
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();

            //assert
            Assert.NotNull(configurationService);
        }

        [Theory]
        [InlineData("App01", true)]
        [InlineData("App02", true)]
        [InlineData("App03", false)]
        public void ReturnsConfiguredApplicationWhenConfiguredOtherwiseNull(string applicationName, bool returnsApplication)
        {
            //arrange
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            configurationService.ProjectProperties = GetProjectProperties(serviceProvider.GetRequiredService<IContextProvider>());

            //act
            Application? application = configurationService.GetApplication(applicationName);
            //assert
            Assert.Equal(returnsApplication, application != null);
        }

        [Fact]
        public void ReturnsConfiguredApplicationGivenRulesFilePath()
        {
            //arrange
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            configurationService.ProjectProperties = GetProjectProperties(serviceProvider.GetRequiredService<IContextProvider>());

            //act
            Application? application = configurationService.GetApplicationFromPath(@"C:\ProjectPath\RULES\App01\diagram\someFile.module");

            //assert
            Assert.True(application != null);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        static ProjectProperties GetProjectProperties(IContextProvider ContextProvider)
        {
            return new ProjectProperties
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
        }
    }
}
