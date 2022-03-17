using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorTypeHelper = ServiceProvider.GetRequiredService<IConstructorTypeHelper>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            ConfigurationService.ProjectProperties = new ProjectProperties
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
                        @"C:\TelerikLogicBuilder\Contoso.Test.Flow\bin\Debug\netstandard2.0",
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
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = new Constructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
                        {
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    ),
                    ["TestResponseB"] = new Constructor
                    (
                        "TestResponseB",
                        "Contoso.Test.Business.Responses.TestResponseB",
                        new List<ParameterBase>
                        {
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                            ),
                            new LiteralParameter
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
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    ),
                    ["GenericResponse"] = new Constructor
                    (
                        "GenericResponse",
                        "Contoso.Test.Business.Responses.GenericResponse`2",
                        new List<ParameterBase>
                        {
                            new GenericParameter
                            (
                                "aProperty",
                                false,
                                "",
                                "A",
                                ContextProvider
                            ),
                            new GenericParameter
                            (
                                "bProperty",
                                false,
                                "",
                                "B",
                                ContextProvider
                            )
                        },
                        new List<string> { "A", "B" },
                        "",
                        ContextProvider
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
        internal IConfigurationService ConfigurationService;
        internal IConstructorTypeHelper ConstructorTypeHelper;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
