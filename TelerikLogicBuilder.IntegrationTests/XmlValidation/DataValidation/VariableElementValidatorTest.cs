using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
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
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class VariableElementValidatorTest : IClassFixture<VariableElementValidatorFixture>
    {
        private readonly VariableElementValidatorFixture _fixture;

        public VariableElementValidatorTest(VariableElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateVariableElementValidator()
        {
            //arrange
            IVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ValidateThrowsForInvalidElementType()
        {
            //arrange
            IVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IVariableElementValidator>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<text name=""stringProperty"">AAA</text>");
            List<string> errors = new();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(xmlElement, typeof(string), applicationTypeInfo, errors));
        }

        [Fact]
        public void ValidatorSucceedsForConfiguredVariable()
        {
            //arrange
            IVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""IntItem"" visibleText=""IntItem"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                typeof(int),
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.False(errors.Any());
        }

        [Fact]
        public void ValidatorFailsIfVariableNotConfigured()
        {
            //arrange
            IVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""NotFoundItem"" visibleText=""NotFoundItem"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                typeof(object),
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, "NotFoundItem"),
                errors.First()
            );
        }

        [Fact]
        public void ValidatorFailsIfVariableTypeCannotBeLoaded()
        {
            //arrange
            IVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""ObjectVariableNotFound"" visibleText=""ObjectVariableNotFound"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                typeof(object),
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadCastAsVariableTypeFormat, "Contoso.Test.Business.Responses.TypeNotFound", "ObjectVariableNotFound"),
                errors.First()
            );
        }

        [Fact]
        public void ValidatorFailsIfVariableTypeNotAssignableToTheAssignedToType()
        {
            //arrange
            IVariableElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IVariableElementValidator>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""IntItem"" visibleText=""IntItem"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate
            (
                xmlElement,
                typeof(string),
                applicationTypeInfo,
                errors
            );

            //assert
            Assert.True(errors.Any());
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableNotAssignableFormat, "IntItem", typeof(string).ToString()),
                errors.First()
            );
        }

        private static XmlElement GetXmlElement(string xmlString)
            => GetXmlDocument(xmlString).DocumentElement!;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class VariableElementValidatorFixture : IDisposable
    {
        public VariableElementValidatorFixture()
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
                    ),
                    ["ObjectVariableNotFound"] = new ObjectVariable
                    (
                        "ObjectVariableNotFound",
                        "ObjectVariableNotFound",
                        VariableCategory.StringKeyIndexer,
                        "Contoso.Test.Business.Responses.TypeNotFound",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "System.Object",
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
