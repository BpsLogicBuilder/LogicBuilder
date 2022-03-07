using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests
{
    public class TypeLoadHelperTest
    {
        public TypeLoadHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public async Task CanLoadTypeByFullName()
        {
            //arrange
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IAssemblyLoadContextManager assemblyLoadContextService = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ILoadContextSponsor loadContextSponsor = serviceProvider.GetRequiredService<ILoadContextSponsor>();
            ITypeLoadHelper typeLoadHelper = serviceProvider.GetRequiredService<ITypeLoadHelper>();
            IApplicationTypeInfoManager applicationTypeInfoManager = serviceProvider.GetRequiredService<IApplicationTypeInfoManager>();

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
                        new WebApiDeployment("", "", "", "", contextProvider),
                        contextProvider
                    )
                },
                new HashSet<string>(),
                contextProvider
            );

            //act
            await loadContextSponsor.LoadAssembiesOnOpenProject();
            //LogicBuilderAssemblyLoadContext logicBuilderAssemblyLoadContext = assemblyLoadContextService.GetAssemblyLoadContext();
            Type? type = typeLoadHelper.TryGetType
            (
                "Contoso.Web.Flow.FlowActivity",
                applicationTypeInfoManager.GetApplicationTypeInfo(configurationService.GetSelectedApplication().Name)
            );
            //assert

            Assert.NotNull(type);


            //act
            loadContextSponsor.UnloadAssembliesOnCloseProject();
            //assert
            Assert.Empty(assemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        [Fact]
        public async Task CanLoadTypeUsingAssemblyQaulifieldName()
        {
            //arrange
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            IAssemblyLoadContextManager assemblyLoadContextService = serviceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            ILoadContextSponsor loadContextSponsor = serviceProvider.GetRequiredService<ILoadContextSponsor>();
            ITypeLoadHelper typeLoadHelper = serviceProvider.GetRequiredService<ITypeLoadHelper>();
            IApplicationTypeInfoManager applicationTypeInfoManager = serviceProvider.GetRequiredService<IApplicationTypeInfoManager>();

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
                        new WebApiDeployment("", "", "", "", contextProvider),
                        contextProvider
                    )
                },
                new HashSet<string>(),
                contextProvider
            );

            //act
            await loadContextSponsor.LoadAssembiesOnOpenProject();
            //LogicBuilderAssemblyLoadContext logicBuilderAssemblyLoadContext = assemblyLoadContextService.GetAssemblyLoadContext();
            Type? type = typeLoadHelper.TryGetType
            (
                "System.Collections.Generic.List`1[[Contoso.Web.Flow.FlowActivity, Contoso.Web.Flow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                applicationTypeInfoManager.GetApplicationTypeInfo(configurationService.GetSelectedApplication().Name)
            );
            //assert

            Assert.NotNull(type);


            //act
            loadContextSponsor.UnloadAssembliesOnCloseProject();
            //assert
            Assert.Empty(assemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }
    }
}
