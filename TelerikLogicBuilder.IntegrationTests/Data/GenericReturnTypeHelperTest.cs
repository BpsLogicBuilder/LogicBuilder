using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.Data
{
    public class GenericReturnTypeHelperTest : IClassFixture<GenericReturnTypeHelperFixture>
    {
        private readonly GenericReturnTypeHelperFixture _fixture;
        private static readonly IServiceProvider serviceProvider;

        static GenericReturnTypeHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        public GenericReturnTypeHelperTest(GenericReturnTypeHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateGenericParametersHelper()
        {
            //arrange
            IGenericReturnTypeHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericReturnTypeHelper>();

            //assert
            Assert.NotNull(helper);
        }

        public static List<object[]> GenericTypeConfigurations_Data
        {

            get
            {
                IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
                return new List<object[]>
                {
                    new object[]
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
                            contextProvider
                        ),
                        typeof(LiteralReturnType),
                        typeof(ListOfLiteralsReturnType)
                    },
                    new object[]
                    {
                        new ObjectGenericConfig
                        (
                            "A",
                            "Contoso.Domain.Entities.DepartmentModel",
                            true,
                            false,
                            false,
                            contextProvider
                        ),
                        typeof(ObjectReturnType),
                        typeof(ListOfObjectsReturnType)
                    },
                    new object[]
                    {
                        new LiteralListGenericConfig
                        (
                            "A",
                            LiteralParameterType.String,
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            "",
                            "",
                            new List<string>(),
                            new List<string>(),
                            contextProvider
                        ),
                        typeof(ListOfLiteralsReturnType),
                        typeof(ListOfObjectsReturnType)
                    },
                    new object[]
                    {
                        new ObjectListGenericConfig
                        (
                            "A",
                            "Contoso.Domain.Entities.DepartmentModel",
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm,
                            contextProvider
                        ),
                        typeof(ListOfObjectsReturnType),
                        typeof(ListOfObjectsReturnType)
                    },
                };
            }
        }

        class InvalidGenericConfig : GenericConfigBase
        {
            public InvalidGenericConfig() : base("A")
            {
            }

            internal override GenericConfigCategory GenericConfigCategory => throw new NotImplementedException();

            internal override string ToXml => throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(GenericTypeConfigurations_Data))]
        internal void CanConvertGenericReturnTypeFromAllGenericConfigBaseTypes(GenericConfigBase config, Type expectedGenericReturnType, Type expectedListOfGenericsReturnType)
        {
            //act
            ReturnTypeBase genericReturnTypeResult = _fixture.GenericReturnTypeHelper.GetConvertedReturnType
            (
                new GenericReturnType
                (
                    "A",
                    _fixture.ContextProvider
                ),
                new List<GenericConfigBase>
                {
                    config
                },
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
            );
            ReturnTypeBase listOfGenericsReturnTypeResult = _fixture.GenericReturnTypeHelper.GetConvertedReturnType
            (
                new ListOfGenericsReturnType
                (
                    "A",
                    ListType.GenericList,
                    _fixture.ContextProvider
                ),
                new List<GenericConfigBase>
                {
                    config
                },
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
            );

            //assert
            Assert.Equal(expectedGenericReturnType, genericReturnTypeResult.GetType());
            Assert.Equal(expectedListOfGenericsReturnType, listOfGenericsReturnTypeResult.GetType());
        }

        [Fact]
        internal void ConvertGenericsAndListOfGenerisThrowForInvalidConfigurationType()
        {
            InvalidGenericConfig config = new();
            //act
            Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                _fixture.GenericReturnTypeHelper.GetConvertedReturnType
                (
                    new GenericReturnType
                    (
                        "A",
                        _fixture.ContextProvider
                    ),
                    new List<GenericConfigBase>
                    {
                        config
                    },
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );

            Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                _fixture.GenericReturnTypeHelper.GetConvertedReturnType
                (
                    new ListOfGenericsReturnType
                    (
                        "A",
                        ListType.GenericList,
                        _fixture.ContextProvider
                    ),
                    new List<GenericConfigBase>
                    {
                        config
                    },
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );
        }
    }

    public class GenericReturnTypeHelperFixture : IDisposable
    {
        public GenericReturnTypeHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            GenericReturnTypeHelper = ServiceProvider.GetRequiredService<IGenericReturnTypeHelper>();
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
        internal IGenericReturnTypeHelper GenericReturnTypeHelper;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
