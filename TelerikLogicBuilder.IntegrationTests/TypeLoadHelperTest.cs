using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
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
    public class TypeLoadHelperTest : IClassFixture<TypeLoadHelperFixture>
    {
        static TypeLoadHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
        public TypeLoadHelperTest(TypeLoadHelperFixture typeLoadHelperFixture)
        {
            _typeLoadHelperFixture = typeLoadHelperFixture;
        }

        #region Fields
        private readonly TypeLoadHelperFixture _typeLoadHelperFixture;
        private static readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLoadTypeByFullName()
        {
            //act
            bool result = _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
            (
                "Contoso.Test.Flow.FlowActivity",
                _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                out Type? type
            );

            //assert
            Assert.True(result);
            Assert.NotNull(type);
        }

        [Fact]
        public void CanLoadTypeUsingAssemblyQaulifieldName()
        {
            //act
            bool result = _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
            (
                "System.Collections.Generic.List`1[[Contoso.Test.Flow.FlowActivity, Contoso.Test.Flow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                out Type? type
            );

            //assert
            Assert.True(result);
            Assert.NotNull(type);
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
                        typeof(string).FullName!
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
                        "Contoso.Domain.Entities.DepartmentModel"
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
                        "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
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
                        "System.Collections.Generic.List`1[[Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                    },
                };
            }
        }

        class InvalidGenericConfig : GenericConfigBase
        {
            public InvalidGenericConfig() : base("")
            {
            }

            internal override GenericConfigCategory GenericConfigCategory => throw new NotImplementedException();

            internal override string ToXml => throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(GenericTypeConfigurations_Data))]
        internal void CanLoadValidConfiguredGenericTypes(GenericConfigBase config, string expectedResultFullName)
        {
            //act
            bool result = _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
            (
                config,
                _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                out Type? type
            );

            //assert
            Assert.True(result);
            Assert.NotNull(type);
            Assert.Equal(expectedResultFullName, type!.FullName);
        }

        [Fact]
        public void TryGetConfiguredGenericTypeThrowsForInvalidType()
        {
            //assert
            Assert.Throws<CriticalLogicBuilderException>
            (
                () => _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
                (
                    new InvalidGenericConfig(),
                    _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                    out Type? type
                )
            );
        }

        public static List<object[]> Parameters_Data
        {

            get
            {
                IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
                return new List<object[]>
                {
                    new object[]
                    {
                        parameterFactory.GetLiteralParameter
                        (
                            "A",
                            false,
                            "",
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
                        typeof(string).FullName!
                    },
                    new object[]
                    {
                        parameterFactory.GetObjectParameter
                        (
                            "A",
                            false,
                            "",
                            "Contoso.Domain.Entities.DepartmentModel",
                            true,
                            false,
                            false
                        ),
                        "Contoso.Domain.Entities.DepartmentModel"
                    },
                    new object[]
                    {
                        parameterFactory.GetListOfLiteralsParameter
                        (
                            "A",
                            false,
                            "",
                            LiteralParameterType.String,
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            "",
                            "",
                            new List<string>(),
                            Array.Empty<char>(),
                            new List<string>()
                        ),
                        "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                    },
                    new object[]
                    {
                        parameterFactory.GetListOfObjectsParameter
                        (
                            "A",
                            false,
                            "",
                            "Contoso.Domain.Entities.DepartmentModel",
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm
                        ),
                        "System.Collections.Generic.List`1[[Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                    },
                };
            }
        }

        class InvalidParameterType : ParameterBase
        {
            public InvalidParameterType() : base("", false, "")
            {
            }

            internal override ParameterCategory ParameterCategory => throw new NotImplementedException();

            internal override string Description => throw new NotImplementedException();

            internal override string ToXml => throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(Parameters_Data))]
        internal void CanLoadValidParameters(ParameterBase parameter, string expectedResultFullName)
        {
            //act
            bool result = _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
            (
                parameter,
                _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                out Type? type
            );

            //assert
            Assert.True(result);
            Assert.NotNull(type);
            Assert.Equal(expectedResultFullName, type!.FullName);
        }

        [Fact]
        public void TryGetParameterTypeThrowsForInvalidType()
        {
            //assert
            Assert.Throws<CriticalLogicBuilderException>
            (
                () => _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
                (
                    new InvalidParameterType(),
                    _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                    out Type? type
                )
            );
        }

        public static List<object[]> ReturnTypes_Data
        {
            get
            {
                IGenericConfigFactory genericConfigFactory = serviceProvider.GetRequiredService<IGenericConfigFactory>();
                IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();

                return new List<object[]>
                {
                    new object[]
                    {
                        returnTypeFactory.GetLiteralReturnType
                        (
                            LiteralFunctionReturnType.Integer
                        ),
                        new List<GenericConfigBase>(),
                        typeof(int).FullName!
                    },
                    new object[]
                    {
                        returnTypeFactory.GetObjectReturnType
                        (
                            "Contoso.Domain.Entities.DepartmentModel"
                        ),
                        new List<GenericConfigBase>(),
                        "Contoso.Domain.Entities.DepartmentModel"
                    },
                    new object[]
                    {
                        returnTypeFactory.GetGenericReturnType
                        (
                            "A"
                        ),
                        new List<GenericConfigBase>()
                        {
                            genericConfigFactory.GetObjectGenericConfig
                            (
                                "A",
                                "Contoso.Domain.Entities.DepartmentModel",
                                true,
                                false,
                                false
                            )
                        },
                        "Contoso.Domain.Entities.DepartmentModel"
                    },
                    new object[]
                    {
                        returnTypeFactory.GetListOfLiteralsReturnType
                        (
                            LiteralFunctionReturnType.Integer,
                            ListType.GenericList
                        ),
                        new List<GenericConfigBase>(),
                        "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                    },
                    new object[]
                    {
                        returnTypeFactory.GetListOfObjectsReturnType
                        (
                            "Contoso.Domain.Entities.DepartmentModel",
                            ListType.GenericList
                        ),
                        new List<GenericConfigBase>(),
                        "System.Collections.Generic.List`1[[Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                    },
                    new object[]
                    {
                        returnTypeFactory.GetListOfGenericsReturnType
                        (
                            "A",
                            ListType.GenericList
                        ),
                        new List<GenericConfigBase>()
                        {
                            genericConfigFactory.GetObjectGenericConfig
                            (
                                "A",
                                "Contoso.Domain.Entities.DepartmentModel",
                                true,
                                false,
                                false
                            )
                        },
                        "System.Collections.Generic.List`1[[Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                    }
                };
            }
        }

        class InvalidReturnType : ReturnTypeBase
        {
            public InvalidReturnType()
            {
            }

            internal override ReturnTypeCategory ReturnTypeCategory => throw new NotImplementedException();

            internal override string ToXml => throw new NotImplementedException();

            internal override string Description => throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(ReturnTypes_Data))]
        internal void CanLoadValidReturnTypes(ReturnTypeBase returnType, List<GenericConfigBase> genericConfigList, string expectedResultFullName)
        {
            //act
            bool result = _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
            (
                returnType,
                genericConfigList,
                _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                out Type? type
            );

            //assert
            Assert.True(result);
            Assert.NotNull(type);
            Assert.Equal(expectedResultFullName, type!.FullName);
        }

        [Fact]
        public void TryGetReturnTypesThrowsForInvalidType()
        {
            //assert
            Assert.Throws<CriticalLogicBuilderException>
            (
                () => _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
                (
                    new InvalidReturnType(),
                    new List<GenericConfigBase>(),
                    _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                    out Type? type
                )
            );
        }

        public static List<object[]> Variables_Data
        {

            get
            {
                IVariableFactory variableFactory = serviceProvider.GetRequiredService<IVariableFactory>();
                return new List<object[]>
                {
                    new object[]
                    {
                        variableFactory.GetLiteralVariable
                        (
                            "name",
                            "memberName",
                            VariableCategory.Field,
                            "~",
                            "",
                            "referenceName",
                            "referenceDefinition",
                            "~",
                            ReferenceCategories.InstanceReference,
                            "comments",
                            LiteralVariableType.String,
                            LiteralVariableInputStyle.SingleLineTextBox,
                            "",
                            "",
                            new List<string>()
                        ),
                        typeof(string).FullName!
                    },
                    new object[]
                    {
                        variableFactory.GetObjectVariable
                        (
                            "name",
                            "memberName",
                            VariableCategory.Field,
                            "~",
                            "",
                            "referenceName",
                            "referenceDefinition",
                            "~",
                            ReferenceCategories.InstanceReference,
                            "comments",
                            "Contoso.Domain.Entities.DepartmentModel"
                        ),
                        "Contoso.Domain.Entities.DepartmentModel"
                    },
                    new object[]
                    {
                        variableFactory.GetListOfLiteralsVariable
                        (
                            "name",
                            "memberName",
                            VariableCategory.Field,
                            "~",
                            "",
                            "referenceName",
                            "referenceDefinition",
                            "~",
                            ReferenceCategories.InstanceReference,
                            "comments",
                            LiteralVariableType.String,
                            ListType.GenericList,
                            ListVariableInputStyle.HashSetForm,
                            LiteralVariableInputStyle.SingleLineTextBox,
                            "",
                            new List<string>(),
                            new List<string>()
                        ),
                        "System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]"
                    },
                    new object[]
                    {
                        variableFactory.GetListOfObjectsVariable
                        (
                            "name",
                            "memberName",
                            VariableCategory.Field,
                            "~",
                            "",
                            "referenceName",
                            "referenceDefinition",
                            "~",
                            ReferenceCategories.InstanceReference,
                            "comments",
                            "Contoso.Domain.Entities.DepartmentModel",
                            ListType.GenericList,
                            ListVariableInputStyle.HashSetForm
                        ),
                        "System.Collections.Generic.List`1[[Contoso.Domain.Entities.DepartmentModel, Contoso.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]"
                    },
                };
            }
        }

        class InvalidVariableType : VariableBase
        {
            public InvalidVariableType() 
                : base
                (
                    serviceProvider.GetRequiredService<IEnumHelper>(),
                    serviceProvider.GetRequiredService<IStringHelper>(),
                    "name",
                    "memberName",
                    VariableCategory.Field,
                    "~", 
                    "",
                    "referenceName", 
                    "referenceDefinition", 
                    "~",
                    ReferenceCategories.InstanceReference,
                    "comments"
                )
            {
            }

            internal override string ToXml => throw new NotImplementedException();

            internal override VariableTypeCategory VariableTypeCategory => throw new NotImplementedException();

            internal override string ObjectTypeString => throw new NotImplementedException();
        }

        [Theory]
        [MemberData(nameof(Variables_Data))]
        internal void CanLoadValidVariables(VariableBase variable, string expectedResultFullName)
        {
            //act
            bool result = _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
            (
                variable,
                _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                out Type? type
            );

            //assert
            Assert.True(result);
            Assert.NotNull(type);
            Assert.Equal(expectedResultFullName, type!.FullName);
        }

        [Fact]
        public void TryGetVariableTypeThrowsForInvalidType()
        {
            //assert
            Assert.Throws<CriticalLogicBuilderException>
            (
                () => _typeLoadHelperFixture.TypeLoadHelper.TryGetSystemType
                (
                    new InvalidVariableType(),
                    _typeLoadHelperFixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_typeLoadHelperFixture.ConfigurationService.GetSelectedApplication().Name),
                    out Type? type
                )
            );
        }
    }

    public class TypeLoadHelperFixture : IDisposable
    {
        public TypeLoadHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
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
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
