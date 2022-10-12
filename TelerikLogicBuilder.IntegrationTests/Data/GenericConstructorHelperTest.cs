using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
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
    public class GenericConstructorHelperTest : IClassFixture<GenericConstructorHelperFixture>
    {
        private readonly GenericConstructorHelperFixture _fixture;

        public GenericConstructorHelperTest(GenericConstructorHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateGenericConstructorHelper()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void MakeGenericTypeWorksForValidConstructorAndValidGenericArguments()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
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
                _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"],
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
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
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
                    _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );
            
            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{4E9A10C7-3728-40C6-8658-3CD7FECA94CF}"), 
                exception.Message
            );
        }

        [Fact]
        public void MakeGenericTypeThrowsIfConstructorTypeCannotBeLoaded()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
            List<GenericConfigBase> genericConfigs = new()
            {
            };

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.MakeGenericType
                (
                    _fixture.ConfigurationService.ConstructorList.Constructors["TypeNotFoundConstructor"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{00C8F426-63F1-4C75-9861-160617E2CE1C}"),
                exception.Message
            );
        }

        [Fact]
        public void MakeGenericTypeThrowsIfConfiguredGenericArgumentNameDoesNotMatchTheConfiguredDataName()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
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
                    _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );
        }

        [Fact]
        public void MakeGenericTypeThrowsIfConstructorTypeIsNotGenericTypeDefinition()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
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
                    _fixture.ConfigurationService.ConstructorList.Constructors["TestResponseB"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{4E9A10C7-3728-40C6-8658-3CD7FECA94CF}"),
                exception.Message
            );
        }

        [Fact]
        public void ConvertGenericTypesWorksForValidConstructorAndValidGenericArguments()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
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
            Constructor constructor = helper.ConvertGenericTypes
            (
                _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"],
                genericConfigs,
                application
            );
            _fixture.TypeLoadHelper.TryGetSystemType(constructor.TypeName, application, out Type? closedType);

            //assert
            Assert.NotNull(closedType);
            Assert.False(closedType!.IsGenericTypeDefinition);
            Assert.True(constructor.Parameters[0] is LiteralParameter);
            Assert.True(constructor.Parameters[1] is ObjectParameter);
        }

        [Fact]
        public void ConvertGenericTypesThrowsIfConfiguredGenricArgumentCountDoesNNotMatchTheDataCount()
        {
            //arrange
            IGenericConstructorHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericConstructorHelper>();
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
                    _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"],
                    genericConfigs,
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{FE6A7D95-BDE6-43D6-93BE-1018DF6F6C82}"),
                exception.Message
            );
        }
    }

    public class GenericConstructorHelperFixture : IDisposable
    {
        public GenericConstructorHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            GenericConfigFactory = ServiceProvider.GetRequiredService<IGenericConfigFactory>();
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
                    ["TestResponseB"] = ConstructorFactory.GetConstructor
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
                        ""
                    ),
                    ["GenericResponse"] = ConstructorFactory.GetConstructor
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
                        ""
                    ),
                    ["TypeNotFoundConstructor"] = ConstructorFactory.GetConstructor
                    (
                        "TypeNotFoundConstructor",
                        "Contoso.Test.Business.Responses.TypeNotFoundConstructor",
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
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IContextProvider ContextProvider;
        internal IGenericConfigFactory GenericConfigFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
