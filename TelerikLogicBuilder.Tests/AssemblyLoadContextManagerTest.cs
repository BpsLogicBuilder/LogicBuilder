using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class AssemblyLoadContextManagerTest
    {
        public AssemblyLoadContextManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateAssemblyLoadContextManager()
        {
            //arrange
            IAssemblyLoadContextManager assemblyLoadContextManager = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();

            //assert
            Assert.NotNull(assemblyLoadContextManager);
        }

        [Fact]
        public void CanCreateLoadContexts()
        {
            //arrange
            IAssemblyLoadContextManager assemblyLoadContextManager = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IConfigurationItemFactory configurationItemFactory = serviceProvider.GetRequiredService<IConfigurationItemFactory>();
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory = serviceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            configurationService.ProjectProperties = configurationItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso",
                new Dictionary<string, Application>
                {
                    ["app01"] = configurationItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "Contoso.Web.Flow.dll",
                        @"C:\.github\BlaiseD\LogicBuilder.Samples\.NetCore\Contoso\Contoso.Web.Flow\bin\Debug\net6.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Web.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        webApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );


            //act
            assemblyLoadContextManager.CreateLoadContexts();

            //assert
            Assert.NotNull(assemblyLoadContextManager);
            Assert.NotEmpty(assemblyLoadContextManager.GetAssemblyLoadContextDictionary()!);
        }
    }
}
