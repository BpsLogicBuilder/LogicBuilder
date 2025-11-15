using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
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
            IProjectPropertiesItemFactory projectPropertiesItemFactory = serviceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory = serviceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            IAssemblyLoadContextManager assemblyLoadContextService = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ILoadContextSponsor loadContextSponsor = serviceProvider.GetRequiredService<ILoadContextSponsor>();

            configurationService.ProjectProperties = projectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\.github\BpsLogicBuilder\LogicBuilder.Samples\FlowProjects\Contoso",
                new Dictionary<string, Application>
                {
                    ["app01"] = projectPropertiesItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "Contoso.Spa.Flow.dll",
                        @"C:\.github\BpsLogicBuilder\LogicBuilder.Samples\SPA\Contoso\Contoso.Spa.Flow\bin\Debug\net9.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Spa.Flow.FlowActivity",
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
