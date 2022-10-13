using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
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
    public class ConstructorGenericsConfigrationValidatorTest : IClassFixture<ConstructorGenericsConfigrationValidatorFixture>
    {
        private readonly ConstructorGenericsConfigrationValidatorFixture _fixture;

        public ConstructorGenericsConfigrationValidatorTest(ConstructorGenericsConfigrationValidatorFixture constructorGenericsConfigrationValidatorFixture)
        {
            _fixture = constructorGenericsConfigrationValidatorFixture;
        }

        #region Fields
        #endregion Fields

        [Fact]
        public void CanCreateConstructorGenericsConfigrationValidator()
        {
            //arrange
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void ValidateReturnsFalseIfConstructorTypeNotFound()
        {
            //arrange
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Constructor constructor = _fixture.ConfigurationService.ConstructorList.Constructors["TypeNotFoundConstructor"];

            //act
            var result = validator.Validate(constructor, new List<GenericConfigBase>(), applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForConstructorFormat, constructor.TypeName, constructor.Name),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfConfiguredConstructorGenericArgumentNamesDoNotMatchTheConstructorDataGenericArguments()
        {
            //arrange
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Constructor constructor = _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"];
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
            var result = validator.Validate(constructor, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.constructorGenericArgsMisMatchFormat,
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
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Constructor constructor = _fixture.ConfigurationService.ConstructorList.Constructors["OneGenericArgument"];
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
            var result = validator.Validate(constructor, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.constructorGenericArgsMisMatchFormat2,
                    constructor.TypeName,
                    string.Join(Strings.itemsCommaSeparator, constructor.GenericArguments)
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfDataGenericTypeForGenericArgumentCannotBeLoaded()
        {
            //arrange
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Constructor constructor = _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"];
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
            var result = validator.Validate(constructor, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.cannotLoadTypeForGenericArgumentForConstructorFormat,
                    "B",
                    constructor.Name
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsFalseIfTypeIsNotGenericTypeDefinitionWithGenricParameters()
        {
            //arrange
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Constructor constructor = _fixture.ConfigurationService.ConstructorList.Constructors["TypeWithNoGenericArguments"];

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
            var result = validator.Validate(constructor, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.constructorGenericArgsMisMatchFormat2,
                    constructor.TypeName,
                    string.Join(Strings.itemsCommaSeparator, constructor.GenericArguments)
                ),
                errors.First()
            );
        }

        [Fact]
        public void ValidateReturnsTrueForValidConfiguration()
        {
            //arrange
            IConstructorGenericsConfigrationValidator validator = _fixture.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            Constructor constructor = _fixture.ConfigurationService.ConstructorList.Constructors["GenericResponse"];
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
                    LiteralParameterType.Integer,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
            };

            //act
            var result = validator.Validate(constructor, dataConfiguredGenericArguments, applicationTypeInfo, errors);

            //assert
            Assert.True(result);
        }
    }

    public class ConstructorGenericsConfigrationValidatorFixture : IDisposable
    {
        public ConstructorGenericsConfigrationValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            ConstructorGenericsConfigrationValidator = ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            GenericConfigFactory = ServiceProvider.GetRequiredService<IGenericConfigFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
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
                    ["GenericResponse"] = ConstructorFactory.GetConstructor
                    (
                        "GenericResponse",
                        "Contoso.Test.Business.Responses.GenericResponse`2",
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
                        new List<string> { "A", "B"},
                        ""
                    ),
                    ["TypeNotFoundConstructor"] = ConstructorFactory.GetConstructor
                    (
                        "TypeNotFoundConstructor",
                        "Contoso.Test.Business.Responses.TypeNotFoundConstructor",
                        new List<ParameterBase>
                        {
                            ParameterFactory.GetLiteralParameter
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
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ""
                    ),
                    ["OneGenericArgument"] = ConstructorFactory.GetConstructor
                    (
                        "OneGenericArgument",
                        "Contoso.Test.Business.OneGenericArgument`1",
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
                        ""
                    ),
                    ["TypeWithNoGenericArguments"] = ConstructorFactory.GetConstructor
                    (
                        "TypeWithNoGenericArguments",
                        "Contoso.Test.Business.Responses.TestResponseA",
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
        internal IConstructorGenericsConfigrationValidator ConstructorGenericsConfigrationValidator;
        internal IConstructorFactory ConstructorFactory;
        internal IContextProvider ContextProvider;
        internal IGenericConfigFactory GenericConfigFactory;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
