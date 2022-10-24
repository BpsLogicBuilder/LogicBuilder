using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.Data
{
    public class GenericFunctionHelperTest : IClassFixture<GenericFunctionHelperFixture>
    {
        private readonly GenericFunctionHelperFixture _fixture;

        public GenericFunctionHelperTest(GenericFunctionHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateGenericFunctionHelper()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void MakeGenericTypeWorksForValidFunctionrAndValidGenericArguments()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                _fixture.GenericConfigFactory.GetObjectGenericConfig
                (
                    "B",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false
                )
            };

            //act
            Type closedType = helper.MakeGenericType
            (
                _fixture.ConfigurationService.FunctionList.Functions["StaticMethodGenericReturn"],
                genericConfigs,
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
            );

            //assert
            Assert.NotNull(closedType);
            Assert.False(closedType.IsGenericTypeDefinition);
        }

        [Fact]
        public void MakeGenericTypeThrowsIfConfiguredGenricArgumentCountDoesNNotMatchTheDataCount()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                )
            };

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.MakeGenericType
                (
                    _fixture.ConfigurationService.FunctionList.Functions["StaticMethodGenericReturn"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{3B5DB5EC-DD17-41F8-8DFD-FD6ABD039599}"),
                exception.Message
            );
        }

        [Fact]
        public void MakeGenericTypeThrowsIfFunctionTypeCannotBeLoaded()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
            };

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.MakeGenericType
                (
                    _fixture.ConfigurationService.FunctionList.Functions["StaticMethodTypeNotFound"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{EC4695BB-D8E0-40FE-9F74-BCCDC28C1B75}"),
                exception.Message
            );
        }

        [Fact]
        public void MakeGenericTypeThrowsIfConfiguredGenericArgumentNameDoesNotMatchTheConfiguredDataName()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                _fixture.GenericConfigFactory.GetObjectGenericConfig
                (
                    "C",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false
                )
            };

            //act
            Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.MakeGenericType
                (
                    _fixture.ConfigurationService.FunctionList.Functions["StaticMethod"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );
        }

        [Fact]
        public void MakeGenericTypeThrowsIfFunctionTypeIsNotGenericTypeDefinition()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                _fixture.GenericConfigFactory.GetObjectGenericConfig
                (
                    "B",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false
                )
            };

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.MakeGenericType
                (
                    _fixture.ConfigurationService.FunctionList.Functions["StaticNonGenericMethod"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{3B5DB5EC-DD17-41F8-8DFD-FD6ABD039599}"),
                exception.Message
            );
        }

        [Fact]
        public void ConvertGenericTypesWorksForValidFunctionAndValidGenericArguments()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                _fixture.GenericConfigFactory.GetObjectGenericConfig
                (
                    "B",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false
                )
            };
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            Function function = helper.ConvertGenericTypes
            (
                _fixture.ConfigurationService.FunctionList.Functions["StaticMethodGenericReturn"],
                genericConfigs,
                application
            );
            _fixture.TypeLoadHelper.TryGetSystemType(function.TypeName, application, out Type? closedType);

            //assert
            Assert.NotNull(closedType);
            Assert.False(closedType!.IsGenericTypeDefinition);
            Assert.True(function.Parameters[0] is LiteralParameter);
            Assert.True(function.Parameters[1] is ObjectParameter);
            Assert.True(function.ReturnType is ObjectReturnType);
        }

        [Fact]
        public void ConvertGenericTypesThrowsIfConfiguredGenricArgumentCountDoesNNotMatchTheDataCount()
        {
            //arrange
            IGenericFunctionHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericFunctionHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                )
            };

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.ConvertGenericTypes
                (
                    _fixture.ConfigurationService.FunctionList.Functions["StaticMethodGenericReturn"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{B9480848-F58F-4D09-B6C8-4989FBE19D00}"),
                exception.Message
            );
        }
    }

    public class GenericFunctionHelperFixture : IDisposable
    {
        public GenericFunctionHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            GenericConfigFactory = ServiceProvider.GetRequiredService<IGenericConfigFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
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

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["StaticMethod"] = FunctionFactory.GetFunction
                    (
                        "StaticMethod",
                        "StaticMethod",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["StaticMethodTypeNotFound"] = FunctionFactory.GetFunction
                    (
                        "StaticMethodTypeNotFound",
                        "StaticMethodTypeNotFound",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticMethodTypeNotFound`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["StaticMethodGenericReturn"] = FunctionFactory.GetFunction
                    (
                        "StaticMethodGenericReturn",
                        "StaticMethodGenericReturn",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
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
                        ReturnTypeFactory.GetGenericReturnType("B"),
                        ""
                    ),
                    ["StaticNonGenericMethod"] = FunctionFactory.GetFunction
                    (
                        "StaticNonGenericMethod",
                        "StaticNonGenericMethod",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticNonGenericClass",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
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
        internal IGenericConfigFactory GenericConfigFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
