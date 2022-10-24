using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests
{
    public class ConstructorTypeHelperTest : IClassFixture<ConstructorTypeHelperFixture>
    {
        private readonly ConstructorTypeHelperFixture _constructorTypeHelperFixture;

        public ConstructorTypeHelperTest(ConstructorTypeHelperFixture constructorTypeHelperFixture)
        {
            _constructorTypeHelperFixture = constructorTypeHelperFixture;
        }

        [Fact]
        public void CanLoadConstructorsGivenConcreteTypeName()
        {
            //act
            var result = _constructorTypeHelperFixture.ConstructorTypeHelper.GetConstructors
            (
                "Contoso.Test.Business.Responses.TestResponseA",
                _constructorTypeHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_constructorTypeHelperFixture.ConfigurationService.GetSelectedApplication().Name)
            );

            //assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void CanLoadConstructorsGivenConcreteType()
        {
            //arrange
            var applicationTypeInfo = _constructorTypeHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_constructorTypeHelperFixture.ConfigurationService.GetSelectedApplication().Name);
            _constructorTypeHelperFixture.TypeLoadHelper.TryGetSystemType("Contoso.Test.Business.Responses.TestResponseA", applicationTypeInfo, out Type? type);

            //act
            var result = _constructorTypeHelperFixture.ConstructorTypeHelper.GetConstructors
            (
                type!,
                applicationTypeInfo
            );

            //assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void CanLoadConstructorsGivenBaseTypeName()
        {
            //act
            var result = _constructorTypeHelperFixture.ConstructorTypeHelper.GetConstructors
            (
                "Contoso.Test.Business.Responses.BaseResponse",
                _constructorTypeHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_constructorTypeHelperFixture.ConfigurationService.GetSelectedApplication().Name)
            );

            //assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void CanLoadConstructorsGivenBaseType()
        {
            //arrange
            var applicationTypeInfo = _constructorTypeHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_constructorTypeHelperFixture.ConfigurationService.GetSelectedApplication().Name);
            _constructorTypeHelperFixture.TypeLoadHelper.TryGetSystemType("Contoso.Test.Business.Responses.BaseResponse", applicationTypeInfo, out Type? type);

            //act
            var result = _constructorTypeHelperFixture.ConstructorTypeHelper.GetConstructors
            (
                type!,
                applicationTypeInfo
            );

            //assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void ReturnsConfiguredOpenGenericTypeGivenClosedGenericTypeName()
        {
            //act
            var result = _constructorTypeHelperFixture.ConstructorTypeHelper.GetConstructors
            (
                "Contoso.Test.Business.Responses.GenericResponse`2[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Int32, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                _constructorTypeHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_constructorTypeHelperFixture.ConfigurationService.GetSelectedApplication().Name)
            );

            //assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public void ReturnsConfiguredOpenGenericTypeGivenClosedGenericType()
        {
            //arrange
            var applicationTypeInfo = _constructorTypeHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_constructorTypeHelperFixture.ConfigurationService.GetSelectedApplication().Name);
            _constructorTypeHelperFixture.TypeLoadHelper.TryGetSystemType("Contoso.Test.Business.Responses.GenericResponse`2[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Int32, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", applicationTypeInfo, out Type? type);

            //act
            var result = _constructorTypeHelperFixture.ConstructorTypeHelper.GetConstructors
            (
                type!,
                applicationTypeInfo
            );

            //assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }

    public class ConstructorTypeHelperFixture : IDisposable
    {
        public ConstructorTypeHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorTypeHelper = ServiceProvider.GetRequiredService<IConstructorTypeHelper>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            ConfigurationService.ProjectProperties = ConfigurationItemFactory.GetProjectProperties
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
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
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

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = ConstructorFactory.GetConstructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "stringProperty",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ""
                    ),
                    ["TestResponseB"] = ConstructorFactory.GetConstructor
                    (
                        "TestResponseB",
                        "Contoso.Test.Business.Responses.TestResponseB",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "stringProperty",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "intProperty",
                                false,
                                "",
                                LiteralParameterType.Integer,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ""
                    ),
                    ["GenericResponse"] = ConstructorFactory.GetConstructor
                    (
                        "GenericResponse",
                        "Contoso.Test.Business.Responses.GenericResponse`2",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetGenericParameter
                            (
                                "aProperty",
                                false,
                                "",
                                "A"
                            ),
                            ParameterFactory.GetGenericParameter
                            (
                                "bProperty",
                                false,
                                "",
                                "B"
                            )
                        },
                        new List<string> { "A", "B" },
                        ""
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        internal IServiceProvider ServiceProvider;
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorTypeHelper ConstructorTypeHelper;
        internal IConstructorFactory ConstructorFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
