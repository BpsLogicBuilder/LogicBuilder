using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class LoadContextSponsorTest
    {
        public LoadContextSponsorTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public async Task CanLoadAndUnloadAssemblies()
        {
            //arrange
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IAssemblyLoadContextManager assemblyLoadContextService = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ILoadContextSponsor loadContextSponsor = serviceProvider.GetRequiredService<ILoadContextSponsor>();
            
            configurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                @"C:\.github\BlaiseD\LogicBuilder.Samples\FlowProjects\Contoso",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
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
                        new WebApiDeployment("","","","", contextProvider),
                        contextProvider
                    )
                },
                new HashSet<string>(),
                contextProvider
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


        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
