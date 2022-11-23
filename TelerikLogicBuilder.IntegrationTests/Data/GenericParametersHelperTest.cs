using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
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
    public class GenericParametersHelperTest : IClassFixture<GenericParametersHelperFixture>
    {
        private readonly GenericParametersHelperFixture _fixture;
        private static readonly IServiceProvider serviceProvider;

        static GenericParametersHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        public GenericParametersHelperTest(GenericParametersHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateGenericParametersHelper()
        {
            //arrange
            IGenericParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericParametersHelper>();

            //assert
            Assert.NotNull(helper);
        }

        public static List<object[]> GenericTypeConfigurations_Data
        {

            get
            {
                IGenericConfigFactory genericConfigFactory = serviceProvider.GetRequiredService<IGenericConfigFactory>();
                return new List<object[]>
                {
                    new object[]
                    {
                        genericConfigFactory.GetLiteralGenericConfig
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
                        typeof(LiteralParameter),
                        typeof(ListOfLiteralsParameter)
                    },
                    new object[]
                    {
                        genericConfigFactory.GetObjectGenericConfig
                        (
                            "A",
                            "Contoso.Domain.Entities.DepartmentModel",
                            true,
                            false,
                            false
                        ),
                        typeof(ObjectParameter),
                        typeof(ListOfObjectsParameter)
                    },
                    new object[]
                    {
                        genericConfigFactory.GetLiteralListGenericConfig
                        (
                            "A",
                            LiteralParameterType.String,
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            "",
                            "",
                            new List<string>(),
                            new List<string>()
                        ),
                        typeof(ListOfLiteralsParameter),
                        typeof(ListOfObjectsParameter)
                    },
                    new object[]
                    {
                        genericConfigFactory.GetObjectListGenericConfig
                        (
                            "A",
                            "Contoso.Domain.Entities.DepartmentModel",
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm
                        ),
                        typeof(ListOfObjectsParameter),
                        typeof(ListOfObjectsParameter)
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
        internal void CanConvertGenericParameterFromAllGenericConfigBaseTypes(GenericConfigBase config, Type expectedGenericParameterType, Type expectedListOfGenericsParameterType)
        {
            //arrange
            IGenericParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericParametersHelper>();

            //act
            List<ParameterBase> genericParameterResult = helper.GetConvertedParameters
            (
                new List<ParameterBase>
                {
                    _fixture.ParameterFactory.GetGenericParameter
                    (
                        "aProperty",
                        false,
                        "",
                        "A"
                    )
                },
                new List<GenericConfigBase>
                {
                    config
                },
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
            );
            List<ParameterBase> listOfGenericsParameterResult = helper.GetConvertedParameters
            (
                new List<ParameterBase>
                {
                    _fixture.ParameterFactory.GetListOfGenericsParameter
                    (
                        "aProperty",
                        false,
                        "",
                        "A",
                        ListType.GenericList,
                        ListParameterInputStyle.ListForm
                    )
                },
                new List<GenericConfigBase>
                {
                    config
                },
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
            );

            //assert
            Assert.Equal(expectedGenericParameterType, genericParameterResult.First().GetType());
            Assert.Equal(expectedListOfGenericsParameterType, listOfGenericsParameterResult.First().GetType());
        }

        [Fact]
        internal void ConvertGenericsAndListOfGenerisThrowForInvalidConfigurationType()
        {
            //arrange
            IGenericParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IGenericParametersHelper>();

            InvalidGenericConfig config = new();
            //act
            Assert.Throws<CriticalLogicBuilderException>
            (
                () =>
                helper.GetConvertedParameters
                (
                    new List<ParameterBase>
                    {
                        _fixture.ParameterFactory.GetGenericParameter
                        (
                            "aProperty",
                            false,
                            "",
                            "A"
                        )
                    },
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
                helper.GetConvertedParameters
                (
                    new List<ParameterBase>
                    {
                        _fixture.ParameterFactory.GetListOfGenericsParameter
                        (
                            "aProperty",
                            false,
                            "",
                            "A",
                            ListType.GenericList,
                            ListParameterInputStyle.ListForm
                        )
                    },
                    new List<GenericConfigBase>
                    {
                        config
                    },
                    _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
                )
            );
        }
    }

    public class GenericParametersHelperFixture : IDisposable
    {
        public GenericParametersHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
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
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
