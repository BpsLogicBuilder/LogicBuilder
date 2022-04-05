using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
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
                new LiteralGenericConfig
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
                    new List<string>(),
                    _fixture.ContextProvider
                ),
                new ObjectGenericConfig
                (
                    "B",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false,
                    _fixture.ContextProvider
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
                new LiteralGenericConfig
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
                    new List<string>(),
                    _fixture.ContextProvider
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
                new LiteralGenericConfig
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
                    new List<string>(),
                    _fixture.ContextProvider
                ),
                new ObjectGenericConfig
                (
                    "C",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false,
                    _fixture.ContextProvider
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
                new LiteralGenericConfig
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
                    new List<string>(),
                    _fixture.ContextProvider
                ),
                new ObjectGenericConfig
                (
                    "B",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false,
                    _fixture.ContextProvider
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
                new LiteralGenericConfig
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
                    new List<string>(),
                    _fixture.ContextProvider
                ),
                new ObjectGenericConfig
                (
                    "B",
                    "Contoso.Domain.Entities.DepartmentModel",
                    true,
                    false,
                    false,
                    _fixture.ContextProvider
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
                new LiteralGenericConfig
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
                    new List<string>(),
                    _fixture.ContextProvider
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
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
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
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["StaticMethod"] = new Function
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
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethodTypeNotFound"] = new Function
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
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethodGenericReturn"] = new Function
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
                        new GenericReturnType("B", ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticNonGenericMethod"] = new Function
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
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
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
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
