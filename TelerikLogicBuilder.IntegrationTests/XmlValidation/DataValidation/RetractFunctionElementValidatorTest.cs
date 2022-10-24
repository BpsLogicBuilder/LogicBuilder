using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
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
    public class RetractFunctionElementValidatorTest : IClassFixture<RetractFunctionElementValidatorFixture>
    {
        private readonly RetractFunctionElementValidatorFixture _fixture;

        public RetractFunctionElementValidatorTest(RetractFunctionElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateRetractFunctionElementValidator()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        public static List<object[]> FunctionElements_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                          </retractFunction>")
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""System_Object"" visibleText=""System_Object"" />
                                          </retractFunction>")
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""LiteralListVariable"" visibleText=""LiteralListVariable"" />
                                          </retractFunction>")
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""ObjectListVariable"" visibleText=""ObjectListVariable"" />
                                          </retractFunction>")
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(FunctionElements_Data))]
        public void RetractFunctionElementValidatorSucceedsForValidElements(XmlElement functionElement)
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void RetractFunctionElementValidatorThrowsForInvalidFunctionElement()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<function name=""Set To Null"" visibleText=""visibleText"">
                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                          </function>");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(functionElement, application, errors));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{D3603215-F74A-46A4-9E7E-C5C2217D92D7}"),
                exception.Message
            );
        }

        [Fact]
        public void RetractFunctionElementValidatorFailsForFunctionNotConfigured()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<retractFunction name=""Set Variable Function Not Configured"" visibleText=""visibleText"">
                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                          </retractFunction>");

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.functionNotConfiguredFormat, "Set Variable Function Not Configured"),
                errors.First()
            );
        }

        [Fact]
        public void RetractFunctionElementValidatorFailsForIncorrectFunctionCategory()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<retractFunction name=""Set Variable Wrong Category"" visibleText=""visibleText"">
                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                          </retractFunction>");
            Function function = _fixture.ConfigurationService.FunctionList.Functions["Set Variable Wrong Category"];
            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.invalidFunctionCategoryFormat, function.Name, Enum.GetName(typeof(FunctionCategories), function.FunctionCategory), functionElement.Name),
                errors.First()
            );
        }

        [Fact]
        public void RetractFunctionElementValidatorFailsForVariableNotConfigured()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                                            <variable name=""garbage"" visibleText=""visibleText"" />
                                                          </retractFunction>");

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotEvaluateVariableFormat, "garbage"),
                errors.First()
            );
        }

        [Fact]
        public void RetractFunctionElementValidatorFailsIfVariableTypeCannotBeLoaded()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement functionElement = GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                                            <variable name=""VariableTypeNotFound"" visibleText=""visibleText"" />
                                                          </retractFunction>");
            VariableBase variable = _fixture.ConfigurationService.VariableList.Variables["VariableTypeNotFound"];
            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name),
                errors.First()
            );
        }

        [Fact]
        public void RetractFunctionElementValidatorFailsForNoNullableValueType()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            string variableName = $"{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Integer)}Item";
            XmlElement functionElement = GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                                            <variable name=""{variableName}"" visibleText=""visibleText"" />
                                                          </retractFunction>");

            //act
            xmlValidator.Validate(functionElement, application, errors);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.cannotSetValueTypedVariableToNullFormat, variableName),
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

    public class RetractFunctionElementValidatorFixture : IDisposable
    {
        public RetractFunctionElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            TypeHelper = ServiceProvider.GetRequiredService<ITypeHelper>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            VariableFactory = ServiceProvider.GetRequiredService<IVariableFactory>();

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
                        RuntimeType.NetCore,
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

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = ConstructorFactory.GetConstructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
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
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["Set To Null"] = FunctionFactory.GetFunction
                    (
                        "Set To Null",
                        "assert",
                        FunctionCategories.Retract,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["Set Variable Wrong Category"] = FunctionFactory.GetFunction
                    (
                        "Set Variable Wrong Category",
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
                    ["System_Object"] = VariableFactory.GetObjectVariable
                    (
                        "System_Object",
                        "System_Object",
                        VariableCategory.StringKeyIndexer,
                        "System.Object",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "System.Object"
                    ),
                    ["VariableTypeNotFound"] = VariableFactory.GetObjectVariable
                    (
                        "VariableTypeNotFound",
                        "VariableTypeNotFound",
                        VariableCategory.StringKeyIndexer,
                        "VariableTypeNotFound",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "System.Object"
                    ),
                    ["LiteralListVariable"] = VariableFactory.GetListOfLiteralsVariable
                    (
                        "LiteralListVariable",
                        "LiteralListVariable",
                        VariableCategory.StringKeyIndexer,
                        "System.Collections.Generic.IList`1[[System.String, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        LiteralVariableType.String,
                        ListType.GenericList,
                        ListVariableInputStyle.ListForm,
                        LiteralVariableInputStyle.SingleLineTextBox,
                        "",
                        new List<string>(),
                        new List<string>()
                    ),
                    ["ObjectListVariable"] = VariableFactory.GetListOfObjectsVariable
                    (
                        "ObjectListVariable",
                        "ObjectListVariable",
                        VariableCategory.StringKeyIndexer,
                        "System.Collections.Generic.IList`1[[Contoso.Test.Business.Responses.TestResponseA, Contoso.Test.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        ListType.GenericList,
                        ListVariableInputStyle.ListForm
                    ),
                    ["InvalidVariableType"] = new InvalidVariableType
                    (
                        ServiceProvider.GetRequiredService<IEnumHelper>(),
                        ServiceProvider.GetRequiredService<IStringHelper>()
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );


            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        LiteralVariable GetLiteralVariable(string name, LiteralVariableType literalVariableType)
            => VariableFactory.GetLiteralVariable
            (
                name,
                name,
                VariableCategory.StringKeyIndexer,
                TypeHelper.ToId(EnumHelper.GetSystemType(literalVariableType)),
                "",
                "flowManager.FlowDataCache.Items",
                "Field.Property.Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                literalVariableType,
                LiteralVariableInputStyle.SingleLineTextBox,
                "",
                "",
                new List<string>()
            );

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        internal IServiceProvider ServiceProvider;
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IContextProvider ContextProvider;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
