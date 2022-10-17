using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
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
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.Data
{
    public class GetValidConfigurationFromDataTest : IClassFixture<GetValidConfigurationFromDataFixture>
    {
        private readonly GetValidConfigurationFromDataFixture _fixture;

        public GetValidConfigurationFromDataTest(GetValidConfigurationFromDataFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateGetValidConfigurationFromData()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void TryGetConstructorReturnsTrueForVaidConfiguredConstructorData()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IConstructorDataParser constructorDataParser = _fixture.ServiceProvider.GetRequiredService<IConstructorDataParser>();
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
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetConstructor
            (
                constructorDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.True(result);
        }

        [Fact]
        public void TryGetConstructorReturnsFalseForConstructorNotConfigured()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IConstructorDataParser constructorDataParser = _fixture.ServiceProvider.GetRequiredService<IConstructorDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""NotConfigured"" visibleText=""NotConfigured"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty""> XX</literalParameter>
                                                        </parameters>
                                                    </constructor>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetConstructor
            (
                constructorDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetConstructorReturnsFalseIfGenericParameterValidationFails()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IConstructorDataParser constructorDataParser = _fixture.ServiceProvider.GetRequiredService<IConstructorDataParser>();
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
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
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
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetConstructor
            (
                constructorDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetConstructorReturnsFalseIfConstructorTypeCannotBeLoaded()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IConstructorDataParser constructorDataParser = _fixture.ServiceProvider.GetRequiredService<IConstructorDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<constructor name=""TypeNotFoundConstructor"" visibleText=""TypeNotFoundConstructor"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty"">SXX</literalParameter>
                                                        </parameters>
                                                    </constructor>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetConstructor
            (
                constructorDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetFunctionReturnsTrueForVaidConfiguredFunctionData()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IFunctionDataParser functionDataParser = _fixture.ServiceProvider.GetRequiredService<IFunctionDataParser>();
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
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetFunction
            (
                functionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.True(result);
        }

        [Fact]
        public void TryGetFunctionReturnsFalseForFunctionNotConfigured()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IFunctionDataParser functionDataParser = _fixture.ServiceProvider.GetRequiredService<IFunctionDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<function name=""NotConfigured"" visibleText=""NotConfigured"" >
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""stringProperty""> XX</literalParameter>
                                                        </parameters>
                                                    </function>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetFunction
            (
                functionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetFunctionReturnsFalseIfGenericParameterValidationFails()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IFunctionDataParser functionDataParser = _fixture.ServiceProvider.GetRequiredService<IFunctionDataParser>();
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
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
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
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetFunction
            (
                functionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetFunctionReturnsFalseIfFunctionReturnTypeCannotBeLoaded()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IFunctionDataParser functionDataParser = _fixture.ServiceProvider.GetRequiredService<IFunctionDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<function name=""ReturnTypeNotLoaded"" visibleText=""ReturnTypeNotLoaded"" >
                                                        <genericArguments />
                                                        <parameters />
                                                    </function>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetFunction
            (
                functionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetVariablerReturnsTrueForValidConfiguredvariableData()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IVariableDataParser varaibleDataParser = _fixture.ServiceProvider.GetRequiredService<IVariableDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""IntegerItem"" visibleText=""IntegerItem"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetVariable
            (
                varaibleDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.True(result);
        }

        [Fact]
        public void TryGetVariablerReturnsFalseIfVariableNotConfigured()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IVariableDataParser varaibleDataParser = _fixture.ServiceProvider.GetRequiredService<IVariableDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""NotFoundItem"" visibleText=""NotFoundItem"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetVariable
            (
                varaibleDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetVariablerReturnsFalseIfVariableTypeCannotBeLoaded()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IVariableDataParser varaibleDataParser = _fixture.ServiceProvider.GetRequiredService<IVariableDataParser>();
            XmlElement xmlElement = GetXmlElement(@"<variable name=""VariableTypeNotLoaded"" visibleText=""VariableTypeNotLoaded"" />");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetVariable
            (
                varaibleDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetVariablerReturnsTrueForValidConfiguredDecisionData()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IDecisionDataParser decisionDataParser = _fixture.ServiceProvider.GetRequiredService<IDecisionDataParser>();
            XmlElement xmlElement = GetXmlElement($@"<decision name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" >
                                                              <and>
                                                                <not>
                                                                  <function name=""Greater Than"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""val1"">
                                                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                      </literalParameter>
                                                                      <literalParameter name=""val2"">10000</literalParameter>
                                                                    </parameters>
                                                                  </function>
                                                                </not>
                                                                <function name=""Equals"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""val1"">
                                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                        </literalParameter>
                                                                        <literalParameter name=""val2"">0.11</literalParameter>
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
                                                            </decision>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetVariable
            (
                decisionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.True(result);
        }

        [Fact]
        public void TryGetVariablerReturnsFalseIfDecisionVariableNotConfigured()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IDecisionDataParser decisionDataParser = _fixture.ServiceProvider.GetRequiredService<IDecisionDataParser>();
            XmlElement xmlElement = GetXmlElement($@"<decision name=""ItemNotConfigured"" visibleText=""visibleText"" >
                                                              <and>
                                                                <function name=""Equals"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""val1"">
                                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                        </literalParameter>
                                                                        <literalParameter name=""val2"">0.11</literalParameter>
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
                                                            </decision>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetVariable
            (
                decisionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetVariablerReturnsFalseIfDecisionVariableNtypeaCannotBeLoaded()
        {
            //arrange
            IGetValidConfigurationFromData helper = _fixture.ServiceProvider.GetRequiredService<IGetValidConfigurationFromData>();
            IDecisionDataParser decisionDataParser = _fixture.ServiceProvider.GetRequiredService<IDecisionDataParser>();
            XmlElement xmlElement = GetXmlElement($@"<decision name=""VariableTypeNotLoaded"" visibleText=""visibleText"" >
                                                              <and>
                                                                <function name=""Equals"" visibleText=""visibleText"">
                                                                    <genericArguments />
                                                                    <parameters>
                                                                        <literalParameter name=""val1"">
                                                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                                        </literalParameter>
                                                                        <literalParameter name=""val2"">0.11</literalParameter>
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
                                                            </decision>");
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            var result = helper.TryGetVariable
            (
                decisionDataParser.Parse(xmlElement),
                applicationTypeInfo,
                out _
            );

            //assert
            Assert.False(result);
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

    public class GetValidConfigurationFromDataFixture : IDisposable
    {
        public GetValidConfigurationFromDataFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            XmlElementValidator = ServiceProvider.GetRequiredService<IXmlElementValidator>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
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
                    ["ReturnTypeNotLoaded"] = FunctionFactory.GetFunction
                    (
                        "ReturnTypeNotLoaded",
                        "ReturnTypeNotLoaded",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        ReturnTypeFactory.GetObjectReturnType("XYZ"),
                        ""
                    ),
                    ["FunctionWithVariableIndexerInReference"] = FunctionFactory.GetFunction
                    (
                        "FunctionWithVariableIndexerInReference",
                        "Equals",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.Items.StringIndexerVariable",
                        "Field.Property.Property.VariableKeyIndexer",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["FunctionWithMissingVariableIndexerInReference"] = FunctionFactory.GetFunction
                    (
                        "FunctionWithMissingVariableIndexerInReference",
                        "Equals",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.Items.StringIndexerVariableNotFound",
                        "Field.Property.Property.VariableKeyIndexer",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["FunctionWithVariableArrayIndexerInReference"] = FunctionFactory.GetFunction
                    (
                        "FunctionWithVariableArrayIndexerInReference",
                        "Equals",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.ObjectArray.ArrayIndexerVariable",
                        "Field.Property.Property.VariableArrayIndexer",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false
                            )
                        },
                        new List<string>(),
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
                    ),
                    ["FunctionWithMissingVariableArrayIndexerInReference"] = FunctionFactory.GetFunction
                    (
                        "FunctionWithMissingVariableArrayIndexerInReference",
                        "Equals",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.ObjectArray.ArrayIndexerVariableNotFound",
                        "Field.Property.Property.VariableArrayIndexer",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false
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
                ContextProvider.TypeHelper.ToId(ContextProvider.EnumHelper.GetSystemType(literalVariableType)),
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
        internal IXmlElementValidator XmlElementValidator;
        internal IConstructorFactory ConstructorFactory;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
