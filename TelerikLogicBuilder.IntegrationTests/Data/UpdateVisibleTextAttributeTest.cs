using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.Data
{
    public class UpdateVisibleTextAttributeTest : IClassFixture<UpdateVisibleTextAttributeFixture>
    {
        private readonly UpdateVisibleTextAttributeFixture _fixture;

        public UpdateVisibleTextAttributeTest(UpdateVisibleTextAttributeFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void UpdateVisibleTextAttributeHelper()
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void UpdateConstructorVisibleTextWorks()
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""GenericResponse"" visibleText=""GenericResponse"">
                                                        <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                                <literalType>String</literalType>
                                                                <control>SingleLineTextBox</control>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                                <propertySource />
                                                                <propertySourceParameter />
                                                                <defaultValue />
                                                                <domain>
						                                            <item>true</item>
						                                            <item>false</item>
					                                            </domain>
                                                            </literalParameter>
                                                            <objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TestResponseA</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>
                                                        </genericArguments>
                                                        <parameters>
                                                            <literalParameter name=""aProperty"">SomeValue</literalParameter>
                                                            <objectParameter name=""bProperty"">
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""stringProperty""> XX</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </objectParameter>
                                                        </parameters>
                                                    </constructor>");


            //act
            helper.UpdateConstructorVisibleText(xmlElement, applicationTypeInfo);
            string visibleTextAttribute = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal("GenericResponse: aProperty=SomeValue;TestResponseA: bProperty", visibleTextAttribute);
        }

        [Fact]
        public void UpdateDecisionVisibleTextWorks()
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement($@"<decision name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" >
                                                            <and>
                                                            <not>
                                                                <function name=""Greater Than"" visibleText=""visibleText"">
                                                                <genericArguments />
                                                                <parameters>
                                                                    <literalParameter name=""value1"">
                                                                    <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                    </literalParameter>
                                                                    <literalParameter name=""value2"">10000</literalParameter>
                                                                </parameters>
                                                                </function>
                                                            </not>
                                                            <function name=""Equals"" visibleText=""visibleText"">
                                                                <genericArguments />
                                                                <parameters>
                                                                    <literalParameter name=""value1"">
                                                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                    </literalParameter>
                                                                    <literalParameter name=""value2"">0.11</literalParameter>
                                                                </parameters>
                                                            </function>
                                                            <function name=""Less Than"" visibleText=""visibleText"">
                                                                <genericArguments />
                                                                <parameters>
                                                                    <literalParameter name=""value1"">
                                                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                    </literalParameter>
                                                                    <literalParameter name=""value2"">99.99</literalParameter>
                                                                </parameters>
                                                            </function>
                                                            </and>
                                                        </decision>");


            //act
            helper.UpdateDecisionVisibleText(xmlElement, applicationTypeInfo);
            string visibleTextAttribute = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal
            (
                "value1=<DoubleItem> Greater Than value2=10000 And value1=<DoubleItem> Equals value2=0.11 And value1=<DoubleItem> Less Than value2=99.99",
                visibleTextAttribute
            );
        }

        [Fact]
        public void UpdateFunctionVisibleTextWorks()
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            XmlElement xmlElement = GetXmlElement(@"<function name=""StaticMethod"" visibleText=""StaticMethod"">
                                                        <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                                <literalType>String</literalType>
                                                                <control>SingleLineTextBox</control>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                                <propertySource />
                                                                <propertySourceParameter />
                                                                <defaultValue />
                                                                <domain>
						                                            <item>true</item>
						                                            <item>false</item>
					                                            </domain>
                                                            </literalParameter>
                                                            <objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TestResponseA</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>
                                                        </genericArguments>
                                                        <parameters>
                                                            <literalParameter name=""aProperty"">SomeValue</literalParameter>
                                                            <objectParameter name=""bProperty"">
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""stringProperty""> XX</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </objectParameter>
                                                        </parameters>
                                                    </function>");


            //act
            helper.UpdateFunctionVisibleText(xmlElement, applicationTypeInfo);
            string visibleTextAttribute = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal("StaticMethod: aProperty=SomeValue;TestResponseA: bProperty", visibleTextAttribute);
        }

        public static List<object[]> SetVariableFunctionElements_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                            <variableValue>
                                              <literalVariable>CB</literalVariable>
                                            </variableValue>
                                          </assertFunction>"),
                        "Set Variable: {<StringItem> Equals CB}"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""System_Object"" visibleText=""System_Object"" />
                                            <variableValue>
                                                <objectVariable>
                                                    <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty"">XX</literalParameter>
                                                        </parameters>
                                                    </constructor>
                                                </objectVariable>
                                            </variableValue>
                                          </assertFunction>"),
                        "Set Variable: {<System_Object> Equals {TestResponseA: stringProperty=XX}}"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""LiteralListVariable"" visibleText=""LiteralListVariable"" />
                                            <variableValue>
                                                <literalListVariable>
                                                    <literalList literalType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                                        <literal>Field1</literal>
                                                        <literal>Field2</literal>
                                                    </literalList>
                                                </literalListVariable>
                                            </variableValue>
                                          </assertFunction>"),
                        "Set Variable: {<LiteralListVariable> Equals (Generic List Of String: Count(2))}"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                            <variable name=""ObjectListVariable"" visibleText=""ObjectListVariable"" />
                                            <variableValue>
                                                <objectListVariable>
                                                    <objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""visibleText"">
                                                        <object>
                                                            <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                <genericArguments />
                                                                <parameters>
                                                                    <literalParameter name=""stringProperty"">XX</literalParameter>
                                                                </parameters>
                                                            </constructor>
                                                        </object>
                                                        <object>
                                                            <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                <genericArguments />
                                                                <parameters>
                                                                    <literalParameter name=""stringProperty"">YY</literalParameter>
                                                                </parameters>
                                                            </constructor>
                                                        </object>
                                                    </objectList>
                                                </objectListVariable>
                                            </variableValue>
                                          </assertFunction>"),
                        "Set Variable: {<ObjectListVariable> Equals ({Generic List Of TestResponseA: Count(2)})}"
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(SetVariableFunctionElements_Data))]
        public void UpdateAssertFunctionVisibleTextsWorks(XmlElement xmlElement, string expectedVisibleText)
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            helper.UpdateAssertFunctionVisibleText(xmlElement, applicationTypeInfo);
            string visibleTextAttribute = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal(expectedVisibleText, visibleTextAttribute);
        }

        public static List<object[]> SetToNullFunctionElements_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                          </retractFunction>"),
                        "Set To Null: {<StringItem>}"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""System_Object"" visibleText=""System_Object"" />
                                          </retractFunction>"),
                        "Set To Null: {<System_Object>}"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""LiteralListVariable"" visibleText=""LiteralListVariable"" />
                                          </retractFunction>"),
                        "Set To Null: {<LiteralListVariable>}"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                            <variable name=""ObjectListVariable"" visibleText=""ObjectListVariable"" />
                                          </retractFunction>"),
                        "Set To Null: {<ObjectListVariable>}"
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(SetToNullFunctionElements_Data))]
        public void UpdateRetractFunctionVisibleTextWorks(XmlElement xmlElement, string expectedVisibleText)
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();

            //act
            helper.UpdateRetractFunctionVisibleText(xmlElement);
            string visibleTextAttribute = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal(expectedVisibleText, visibleTextAttribute);
        }

        [Fact]
        public void UpdateVariableVisibleTextWorks()
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            XmlElement xmlElement = GetXmlElement($@"<variable name=""DoubleItem"" visibleText=""visibleText"" />");


            //act
            helper.UpdateVariableVisibleText(xmlElement);

            //assert
            string visibleText = xmlElement.GetAttribute("visibleText");
            Assert.Equal
            (
                "DoubleItem",
                visibleText
            );
        }

        public static List<object[]> LiteralListElements_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        GetXmlElement(@$"<literalList literalType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                            <literal>Field1</literal>
                                            <literal>Field2</literal>
                                        </literalList>"),
                        null!,
                        "(Generic List Of String: Count(2))"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<literalList literalType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                            <literal>Field1</literal>
                                            <literal>Field2</literal>
                                        </literalList>"),
                        "courses",
                        "(courses: Count(2))"
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(LiteralListElements_Data))]
        public void UpdateLiteralListVisibleTextWorks(XmlElement xmlElement, string literalListParameterName, string? expectedVisibleText)
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            helper.UpdateLiteralListVisibleText(xmlElement, applicationTypeInfo, literalListParameterName);
            string literalListElementVisibleText = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal(expectedVisibleText, literalListElementVisibleText);
        }

        public static List<object[]> ObjectListElements_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        GetXmlElement(@$"<objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""visibleText"">
                                            <object>
                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                    <genericArguments />
                                                    <parameters>
                                                        <literalParameter name=""stringProperty"">XX</literalParameter>
                                                    </parameters>
                                                </constructor>
                                            </object>
                                            <object>
                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                    <genericArguments />
                                                    <parameters>
                                                        <literalParameter name=""stringProperty"">YY</literalParameter>
                                                    </parameters>
                                                </constructor>
                                            </object>
                                        </objectList>"),
                        null!,
                        "({Generic List Of TestResponseA: Count(2)})"
                    },
                    new object[]
                    {
                        GetXmlElement(@$"<objectList objectType=""Contoso.Test.Business.Responses.TestResponseA"" listType=""GenericList"" visibleText=""visibleText"">
                                            <object>
                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                    <genericArguments />
                                                    <parameters>
                                                        <literalParameter name=""stringProperty"">XX</literalParameter>
                                                    </parameters>
                                                </constructor>
                                            </object>
                                            <object>
                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                    <genericArguments />
                                                    <parameters>
                                                        <literalParameter name=""stringProperty"">YY</literalParameter>
                                                    </parameters>
                                                </constructor>
                                            </object>
                                        </objectList>"),
                        "courses",
                        "({courses: Count(2)})"
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(ObjectListElements_Data))]
        public void UpdateObjectListVisibleTextWorks(XmlElement xmlElement, string? objectListParameterName, string expectedVisibleText)
        {
            //arrange
            IUpdateVisibleTextAttribute helper = _fixture.ServiceProvider.GetRequiredService<IUpdateVisibleTextAttribute>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            helper.UpdateObjectListVisibleText(xmlElement, applicationTypeInfo, objectListParameterName);
            string objectListElementVisibleText = xmlElement.GetAttribute("visibleText");

            //assert
            Assert.Equal(expectedVisibleText, objectListElementVisibleText);
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

    public class UpdateVisibleTextAttributeFixture : IDisposable
    {
        public UpdateVisibleTextAttributeFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
  
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
                    ),
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
                        new List<string> { "A", "B" },
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
                    )
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
                    ),
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
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
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
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["Set Variable"] = FunctionFactory.GetFunction
                    (
                        "Set Variable",
                        "assert",
                        FunctionCategories.Assert,
                        "",
                        "",
                        "",
                        "",
                        ReferenceCategories.None,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
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
                    ["VariableTypeNotLoaded"] = VariableFactory.GetObjectVariable
                    (
                        "VariableTypeNotLoaded",
                        "VariableTypeNotLoaded",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "VariableTypeNotLoaded"
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
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
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
