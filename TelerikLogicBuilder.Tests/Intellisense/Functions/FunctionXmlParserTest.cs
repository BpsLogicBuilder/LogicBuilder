﻿using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class FunctionXmlParserTest
    {
        public FunctionXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.IntegrationTest)]
        public void GetFunctionFromXml()
        {
            //arrange
            IFunctionXmlParser functionXmlParser = serviceProvider.GetRequiredService<IFunctionXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<function name=""Action"">
                                                      <memberName>agent broadcast commit.action</memberName>
                                                      <functionCategory>Standard</functionCategory>
                                                      <typeName />
                                                      <referenceName>referenceNameHere</referenceName>
                                                      <referenceDefinition />
                                                      <castReferenceAs />
                                                      <referenceCategory>This</referenceCategory>
                                                      <parametersLayout>Sequential</parametersLayout>
                                                      <parameters>
                                                        <literalParameter name=""value"">
                                                          <literalType>String</literalType>
                                                          <control>SingleLineTextBox</control>
                                                          <optional>false</optional>
                                                          <useForEquality>true</useForEquality>
                                                          <useForHashCode>false</useForHashCode>
                                                          <useForToString>true</useForToString>
                                                          <propertySource />
                                                          <propertySourceParameter />
                                                          <defaultValue />
                                                          <domain />
                                                          <comments />
                                                        </literalParameter>
                                                        <literalParameter name=""updatecommit"">
                                                          <literalType>String</literalType>
                                                          <control>SingleLineTextBox</control>
                                                          <optional>true</optional>
                                                          <useForEquality>true</useForEquality>
                                                          <useForHashCode>false</useForHashCode>
                                                          <useForToString>true</useForToString>
                                                          <propertySource />
                                                          <propertySourceParameter />
                                                          <defaultValue />
                                                          <domain />
                                                          <comments />
                                                        </literalParameter>
                                                      </parameters>
                                                      <genericArguments />
                                                      <returnType>
                                                        <literal>
                                                          <literalType>Void</literalType>
                                                        </literal>
                                                      </returnType>
                                                      <summary>Updates the action code.</summary>
                                                    </function>");

            //act
            Function result = functionXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Action", result.Name);
            Assert.Equal(2, result.Parameters.Count);
            Assert.True(result.ReturnType is LiteralReturnType);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        private static XmlElement GetXmlElement(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument.DocumentElement;
        }
    }
}
