using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.Reflection
{
    public class LoadContextSponsorTest
    {
        public LoadContextSponsorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public async Task CanLoadAndUnloadAssemblies()
        {
            //arrange
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IConfigurationItemFactory configurationItemFactory = serviceProvider.GetRequiredService<IConfigurationItemFactory>();
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory = serviceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            IAssemblyLoadContextManager assemblyLoadContextService = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ILoadContextSponsor loadContextSponsor = serviceProvider.GetRequiredService<ILoadContextSponsor>();

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
            await loadContextSponsor.LoadAssembiesOnOpenProject();
            LogicBuilderAssemblyLoadContext logicBuilderAssemblyLoadContext = assemblyLoadContextService.GetAssemblyLoadContext();
            //assert
            Assert.NotEmpty(logicBuilderAssemblyLoadContext.Assemblies);


            //act
            loadContextSponsor.UnloadAssembliesOnCloseProject();
            //assert
            Assert.Empty(assemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }
    }
}
