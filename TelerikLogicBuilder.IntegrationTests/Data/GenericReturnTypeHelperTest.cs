﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
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
                        typeof(LiteralReturnType),
                        typeof(ListOfLiteralsReturnType)
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
                        typeof(ObjectReturnType),
                        typeof(ListOfObjectsReturnType)
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
                        typeof(ListOfLiteralsReturnType),
                        typeof(ListOfObjectsReturnType)
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
                _fixture.ReturnTypeFactory.GetGenericReturnType
                (
                    "A"
                ),
                new List<GenericConfigBase>
                {
                    config
                },
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name)
            );
            ReturnTypeBase listOfGenericsReturnTypeResult = _fixture.GenericReturnTypeHelper.GetConvertedReturnType
            (
                _fixture.ReturnTypeFactory.GetListOfGenericsReturnType
                (
                    "A",
                    ListType.GenericList
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
                    _fixture.ReturnTypeFactory.GetGenericReturnType
                    (
                        "A"
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
                    _fixture.ReturnTypeFactory.GetListOfGenericsReturnType
                    (
                        "A",
                        ListType.GenericList
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
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            GenericReturnTypeHelper = ServiceProvider.GetRequiredService<IGenericReturnTypeHelper>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
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
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IGenericReturnTypeHelper GenericReturnTypeHelper;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
