﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
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
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
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
                    ["ReturnTypeNotLoaded"] = new Function
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
                        new ObjectReturnType("XYZ", ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["FunctionWithVariableIndexerInReference"] = new Function
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
                            new ObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false,
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["FunctionWithMissingVariableIndexerInReference"] = new Function
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
                            new ObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false,
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["FunctionWithVariableArrayIndexerInReference"] = new Function
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
                            new ObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false,
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["FunctionWithMissingVariableArrayIndexerInReference"] = new Function
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
                            new ObjectParameter
                            (
                                "obj",
                                false,
                                "",
                                "System.Object",
                                true,
                                false,
                                false,
                                ContextProvider
                            )
                        },
                        new List<string>(),
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
                    ["VariableTypeNotLoaded"] = new ObjectVariable
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
                        "VariableTypeNotLoaded",
                        ContextProvider
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
            => new
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
                new List<string>(),
                ContextProvider
            );

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