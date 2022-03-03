using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Variables
{
    public class VariablesXmlParserTest
    {
        public VariablesXmlParserTest()
        {
			serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
		}

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
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

        [Fact]
        public void GetVariableDictionaryWorks()
        {
            //arrange
            IVariablesXmlParser variablesXmlParser = serviceProvider.GetRequiredService<IVariablesXmlParser>();
            XmlDocument xmlDocument = GetXmlDocument(@"<folder name=""Folder1"">
                                                        <literalVariable name=""Listeral"">
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
		                                                </literalVariable>
                                                        <objectVariable name=""Object"">
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
                                                        </objectVariable>
                                                        <folder name=""Subfolder"">
                                                            <literalListVariable name=""literalList"">
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
		                                                    </literalListVariable>
                                                            <objectListVariable name=""objectList"">
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
		                                                    </objectListVariable>
		                                                </folder>
		                                            </folder>");

            //act
            var result = variablesXmlParser.GetVariablesDictionary(xmlDocument);

            //assert
            Assert.Equal(4, result.Count);
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
}
