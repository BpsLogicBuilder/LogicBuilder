using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
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
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ConditionsElementValidatorTest : IClassFixture<ConditionsElementValidatorFixture>
    {
        private readonly ConditionsElementValidatorFixture _fixture;

        public ConditionsElementValidatorTest(ConditionsElementValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateConditionsElementValidator()
        {
            //arrange
            IConditionsElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConditionsElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ConditionsElementValidatorWorksForValidFunctions()
        {
            //arrange
            IConditionsElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConditionsElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement conditionsElement = GetXmlElement($@"<conditions>
                                                              <and>
                                                                <not>
                                                                  <function name=""Greater Than"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""val1"">
                                                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Integer)}Item"" visibleText=""visibleText"" />
                                                                      </literalParameter>
                                                                      <literalParameter name=""val2"">0</literalParameter>
                                                                    </parameters>
                                                                  </function>
                                                                </not>
                                                                <function name=""Equals"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""val1"">
                                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                                        </literalParameter>
                                                                        <literalParameter name=""val2"">SomeString</literalParameter>
                                                                    </parameters>
                                                                </function>
                                                                <function name=""Less Than"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""val1"">
                                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                        </literalParameter>
                                                                        <literalParameter name=""val2"">99.99</literalParameter>
                                                                    </parameters>
                                                                </function>
                                                              </and>
                                                            </conditions>");

            //act
            xmlValidator.Validate(conditionsElement, application, errors);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ConditionsElementValidatorThrowsForInvalidElementType()
        {
            //arrange
            IConditionsElementValidator xmlValidator = _fixture.ServiceProvider.GetRequiredService<IConditionsElementValidator>();
            var application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            List<string> errors = new();
            XmlElement conditionsElement = GetXmlElement(@$"<function name=""Equals"" visibleText=""visibleText"">
                                                            <genericArguments />
                                                            <parameters>
                                                                <literalParameter name=""val1"">
                                                                <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                                </literalParameter>
                                                                <literalParameter name=""val2"">SomeString</literalParameter>
                                                            </parameters>
                                                        </function>");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => xmlValidator.Validate(conditionsElement, application, errors));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{4C53E295-8C5E-4FD4-9340-BF2BC9BC8F11}"),
                exception.Message
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

    public class ConditionsElementValidatorFixture : IDisposable
    {
        public ConditionsElementValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
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
                    ["Equals"] = FunctionFactory.GetFunction
                    (
                        "Equals",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.ValueEquality)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["Greater Than"] = FunctionFactory.GetFunction
                    (
                        "Greater Than",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.GreaterThan)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["Less Than"] = FunctionFactory.GetFunction
                    (
                        "Less Than",
                        Enum.GetName(typeof(CodeBinaryOperatorType), CodeBinaryOperatorType.LessThan)!,
                        FunctionCategories.BinaryOperator,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Binary,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "value1",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            ),
                            ParameterFactory.GetLiteralParameter
                            (
                                "value2",
                                false,
                                "",
                                LiteralParameterType.Any,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                false,
                                "",
                                "",
                                "",
                                new List<string>()
                            )
                        },
                        new List<string>(),
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
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IFunctionFactory FunctionFactory;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
