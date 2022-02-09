﻿using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Variables
{
    public class VariablesXmlParserTest
    {
        public VariablesXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetLiteralVariableFromXml()
        {
            //arrange
            IVariablesXmlParser variablesXmlParser = serviceProvider.GetRequiredService<IVariablesXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalVariable name=""Refresh"">
			                                    <memberName>acrg</memberName>
			                                    <variableCategory>Property</variableCategory>
			                                    <castVariableAs />
			                                    <typeName />
			                                    <referenceName />
			                                    <referenceDefinition />
			                                    <castReferenceAs />
			                                    <referenceCategory>This</referenceCategory>
			                                    <evaluation>Implemented</evaluation>
			                                    <comments />
			                                    <metadata />
			                                    <literalType>Decimal</literalType>
			                                    <control>SingleLineTextBox</control>
			                                    <propertySource />
			                                    <defaultValue />
			                                    <domain>
						                            <item>true</item>
						                            <item>false</item>
					                            </domain>
		                                    </literalVariable>");

            //act
            LiteralVariable result = (LiteralVariable)variablesXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Refresh", result.Name);
            Assert.Equal(2, result.Domain.Count);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetObjectVariableFromXml()
        {
            //arrange
            IVariablesXmlParser variablesXmlParser = serviceProvider.GetRequiredService<IVariablesXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectVariable name=""Refresh"">
                                      <memberName>acrg</memberName>
                                      <variableCategory>Property</variableCategory>
                                      <castVariableAs />
                                      <typeName />
                                      <referenceName />
                                      <referenceDefinition />
                                      <castReferenceAs />
                                      <referenceCategory>This</referenceCategory>
                                      <evaluation>Implemented</evaluation>
                                      <comments>Comment</comments>
                                      <metadata />
                                      <objectType>System.Object</objectType>
                                    </objectVariable>");

            //act
            ObjectVariable result = (ObjectVariable)variablesXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Refresh", result.Name);
            Assert.Equal("System.Object", result.ObjectType);
            Assert.Equal("Comment", result.Comments);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetLiteralListVariableFromXml()
        {
            //arrange
            IVariablesXmlParser variablesXmlParser = serviceProvider.GetRequiredService<IVariablesXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalListVariable name=""Refresh"">
			                                    <memberName>acrg</memberName>
			                                    <variableCategory>Property</variableCategory>
			                                    <castVariableAs />
			                                    <typeName />
			                                    <referenceName />
			                                    <referenceDefinition />
			                                    <castReferenceAs />
			                                    <referenceCategory>This</referenceCategory>
			                                    <evaluation>Implemented</evaluation>
			                                    <comments />
			                                    <metadata />
			                                    <literalType>Decimal</literalType>
                                                <listType>GenericList</listType>
			                                    <control>HashSetForm</control>
			                                    <elementControl>SingleLineTextBox</elementControl>
			                                    <propertySource />
			                                    <defaultValue>
						                            <item>hi</item>
						                            <item>medium</item>
					                            </defaultValue>
			                                    <domain>
						                            <item>true</item>
						                            <item>false</item>
					                            </domain>
		                                    </literalListVariable>");

            //act
            ListOfLiteralsVariable result = (ListOfLiteralsVariable)variablesXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Refresh", result.Name);
            Assert.Equal(LiteralVariableInputStyle.SingleLineTextBox, result.ElementControl);
            Assert.Equal(ListVariableInputStyle.HashSetForm, result.Control);
            Assert.Equal(2, result.Domain.Count);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetObjectListVariableFromXml()
        {
            //arrange
            IVariablesXmlParser variablesXmlParser = serviceProvider.GetRequiredService<IVariablesXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectListVariable name=""Page Sizes"">
			                                    <memberName>acrg</memberName>
			                                    <variableCategory>Property</variableCategory>
			                                    <castVariableAs />
			                                    <typeName />
			                                    <referenceName />
			                                    <referenceDefinition />
			                                    <castReferenceAs />
			                                    <referenceCategory>This</referenceCategory>
			                                    <evaluation>Implemented</evaluation>
			                                    <comments>A Comment</comments>
			                                    <metadata />
			                                    <objectType>System.Object</objectType>
                                                <listType>GenericList</listType>
			                                    <control>HashSetForm</control>
		                                    </objectListVariable>");

            //act
            ListOfObjectsVariable result = (ListOfObjectsVariable)variablesXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Page Sizes", result.Name);
            Assert.Equal("System.Object", result.ObjectType);
            Assert.Equal(ListVariableInputStyle.HashSetForm, result.Control);
            Assert.Equal("A Comment", result.Comments);
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
