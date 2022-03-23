using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class FunctionGenericsConfigrationValidatorTest : IClassFixture<FunctionGenericsConfigrationValidatorFixture>
    {
        private readonly FunctionGenericsConfigrationValidatorFixture _fixture;

        public FunctionGenericsConfigrationValidatorTest(FunctionGenericsConfigrationValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanFunctionGenericsConfigrationValidator()
        {
            //arrange
            IFunctionGenericsConfigrationValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ValidateReturnsFalseIfConfiguredFunctionGenericArgumentNamesDoNotMatchTheFunctionDataGenericArguments()
        {
            //arrange
            IFunctionGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["StaticMethod"];

            List<GenericConfigBase> dataConfiguredGenericArguments = new()
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
                new LiteralGenericConfig
                (
                    "C",
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
            var result = validator.Validate(function, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.functionGenericArgsMisMatchFormat,
                    string.Join(Strings.itemsCommaSeparator, new List<string> { "A, B" }),
                    string.Join(Strings.itemsCommaSeparator, new List<string> { "A, C" })
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfLoadedTypesGenericArgumentCountDoesNotMatchTheConfiguredGenericArgumentCount()
        {
            //arrange
            IFunctionGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["StaticMethodOneArgument"];

            List<GenericConfigBase> dataConfiguredGenericArguments = new()
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
                new LiteralGenericConfig
                (
                    "B",
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
            var result = validator.Validate(function, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.functionGenericArgsMisMatchFormat2,
                    _fixture.ContextProvider.EnumHelper.GetVisibleEnumText(ReferenceCategories.Type),
                    function.TypeName,
                    string.Join(Strings.itemsCommaSeparator, function.GenericArguments)
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfConfiguredFunctionHasGenericTypesAndReferenceCategoryIsNotEqualToType()
        {
            //arrange
            IFunctionGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["StaticMethodWrongCategory"];

            List<GenericConfigBase> dataConfiguredGenericArguments = new()
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
                new LiteralGenericConfig
                (
                    "B",
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
            var result = validator.Validate(function, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.functionGenericArgsMisMatchFormat2,
                    _fixture.ContextProvider.EnumHelper.GetVisibleEnumText(ReferenceCategories.Type),
                    function.TypeName,
                    string.Join(Strings.itemsCommaSeparator, function.GenericArguments)
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfDataGenericTypeForGenericArgumentCannotBeLoaded()
        {
            //arrange
            IFunctionGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["StaticMethod"];

            List<GenericConfigBase> dataConfiguredGenericArguments = new()
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
                    "SomeTypeNotFound",
                    true,
                    false,
                    false,
                    _fixture.ContextProvider
                )
            };

            //act
            var result = validator.Validate(function, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.cannotLoadTypeForGenericArgumentForFunctionFormat,
                    "B",
                    function.Name
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfTypeIsNotGenericTypeDefinitionWithGenricParameters()
        {
            //arrange
            IFunctionGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["StaticNonGenericMethod"];

            List<GenericConfigBase> dataConfiguredGenericArguments = new()
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
                new LiteralGenericConfig
                (
                    "B",
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
            var result = validator.Validate(function, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.functionGenericArgsMisMatchFormat2,
                    _fixture.ContextProvider.EnumHelper.GetVisibleEnumText(ReferenceCategories.Type),
                    function.TypeName,
                    string.Join(Strings.itemsCommaSeparator, function.GenericArguments)
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsTrueForValidConfiguration()
        {
            //arrange
            IFunctionGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Function function = _fixture.ConfigurationService.FunctionList.Functions["StaticMethod"];

            List<GenericConfigBase> dataConfiguredGenericArguments = new()
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
                new LiteralGenericConfig
                (
                    "B",
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
            var result = validator.Validate(function, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.True(result);
        }
    }

    public class FunctionGenericsConfigrationValidatorFixture : IDisposable
    {
        public FunctionGenericsConfigrationValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            XmlElementValidator = ServiceProvider.GetRequiredService<IXmlElementValidator>();
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
                    ["TestResponseC"] = new Constructor
                    (
                        "TestResponseC",
                        "Contoso.Test.Business.Responses.TestResponseC",
                        new List<ParameterBase>
                        {
                            new ObjectParameter
                            (
                                "objectProperty",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                true,
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
                    ),
                    ["TypeNotFoundConstructor"] = new Constructor
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
                        "",
                        ContextProvider
                    ),
                    ["String"] = new Constructor
                    (
                        "String",
                        "System.String",
                        new List<ParameterBase>
                        {
                            new ListOfLiteralsParameter
                            (
                                "charArray",
                                false,
                                "",
                                LiteralParameterType.String,
                                ListType.Array,
                                ListParameterInputStyle.HashSetForm,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                "",
                                "",
                                new List<string>(),
                                new char[] { ',' },
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["GetString"] = new Function
                    (
                        "GetString",
                        "GetString",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        new LiteralReturnType(LiteralFunctionReturnType.String, ContextProvider),
                        "",
                        ContextProvider
                    ),
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
                    ),
                    ["StaticMethodOneArgument"] = new Function
                    (
                        "StaticMethodOneArgument",
                        "StaticMethodOneArgument",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClassOneArgument`1",
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
                    ["StaticMethodWrongCategory"] = new Function
                    (
                        "StaticMethodWrongCategory",
                        "StaticMethodWrongCategory",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.This,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
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

            ConfigurationService.VariableList = new VariableList
            (
                new Dictionary<string, VariableBase>
                {
                    ["StringItem"] = new LiteralVariable
                    (
                        "StringItem",
                        "StringItem",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.String,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        "",
                        new List<string>(),
                        ContextProvider
                    ),
                    ["IntItem"] = new LiteralVariable
                    (
                        "IntItem",
                        "IntItem",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.Integer,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        "",
                        new List<string>(),
                        ContextProvider
                    ),
                    ["NullableIntItem"] = new LiteralVariable
                    (
                        "NullableIntItem",
                        "NullableIntItem",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.NullableInteger,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        "",
                        new List<string>(),
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
        internal IXmlElementValidator XmlElementValidator;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
