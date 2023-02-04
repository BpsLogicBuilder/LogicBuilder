using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
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
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
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
                    new List<string>()
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
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
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
                    new List<string>()
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
                    _fixture.EnumHelper.GetVisibleEnumText(ReferenceCategories.Type),
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
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
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
                    new List<string>()
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
                    _fixture.EnumHelper.GetVisibleEnumText(ReferenceCategories.Type),
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
                    "SomeTypeNotFound",
                    true,
                    false,
                    false
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
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
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
                    new List<string>()
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
                    _fixture.EnumHelper.GetVisibleEnumText(ReferenceCategories.Type),
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
                _fixture.GenericConfigFactory.GetLiteralGenericConfig
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
                    new List<string>()
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
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            GenericConfigFactory = ServiceProvider.GetRequiredService<IGenericConfigFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
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
                    ["StaticMethodOneArgument"] = FunctionFactory.GetFunction
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
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["StaticMethodWrongCategory"] = FunctionFactory.GetFunction
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
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
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
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
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
        internal IEnumHelper EnumHelper;
        internal IFunctionFactory FunctionFactory;
        internal IGenericConfigFactory GenericConfigFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
