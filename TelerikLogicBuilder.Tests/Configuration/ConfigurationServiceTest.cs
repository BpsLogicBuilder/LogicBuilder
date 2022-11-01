﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
            configurationService.ProjectProperties = GetProjectProperties(serviceProvider.GetRequiredService<IConfigurationItemFactory>());

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
            configurationService.ProjectProperties = GetProjectProperties(serviceProvider.GetRequiredService<IConfigurationItemFactory>());

            //act
            Application? application = configurationService.GetApplicationFromPath(@"C:\ProjectPath\RULES\App01\diagram\someFile.module");

            //assert
            Assert.True(application != null);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        static ProjectProperties GetProjectProperties(IConfigurationItemFactory ConfigurationItemFactory)
        {
            return ConfigurationItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );
        }
    }
}
