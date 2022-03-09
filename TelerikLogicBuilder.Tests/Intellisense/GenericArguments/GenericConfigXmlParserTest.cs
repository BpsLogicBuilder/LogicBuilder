using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.GenericArguments
{
    public class GenericConfigXmlParserTest
    {
        public GenericConfigXmlParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void GetGenericLiteralParameterFromXml()
        {
            //arrange
            IGenericConfigXmlParser genericConfigXmlParser = serviceProvider.GetRequiredService<IGenericConfigXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalParameter genericArgumentName=""From"">
                                                        <literalType>String</literalType>
                                                        <control>SingleLineTextBox</control>
                                                        <useForEquality>true</useForEquality>
                                                        <useForHashCode>false</useForHashCode>
                                                        <useForToString>true</useForToString>
                                                        <propertySource />
                                                        <propertySourceParameter />
                                                        <defaultValue />
                                                        <domain>
						                                    <item>true</item>
						                                    <item>false</item>
					                                    </domain>
                                                    </literalParameter>");

            //act
            LiteralGenericConfig result = (LiteralGenericConfig)genericConfigXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("From", result.GenericArgumentName);
            Assert.Equal(2, result.Domain.Count);
        }

        [Fact]
        public void GetGenericObjectParameterFromXml()
        {
            //arrange
            IGenericConfigXmlParser genericConfigXmlParser = serviceProvider.GetRequiredService<IGenericConfigXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectParameter genericArgumentName=""To"">
                                                        <objectType>System.Object</objectType>
                                                        <useForEquality>false</useForEquality>
                                                        <useForHashCode>false</useForHashCode>
                                                        <useForToString>true</useForToString>
                                                    </objectParameter>");

            //act
            ObjectGenericConfig result = (ObjectGenericConfig)genericConfigXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("To", result.GenericArgumentName);
            Assert.Equal("System.Object", result.ObjectType);
        }

        [Fact]
        public void GetGenericLiteralListParameterFromXml()
        {
            //arrange
            IGenericConfigXmlParser genericConfigXmlParser = serviceProvider.GetRequiredService<IGenericConfigXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalListParameter genericArgumentName=""From"">
                                                        <literalType>String</literalType>
                                                        <listType>GenericList</listType>
                                                        <control>HashSetForm</control>
                                                        <elementControl>SingleLineTextBox</elementControl>
                                                        <propertySource />
                                                        <propertySourceParameter />
                                                        <defaultValue>
						                                    <item>hi</item>
						                                    <item>medium</item>
					                                    </defaultValue>
                                                        <domain>
						                                    <item>true</item>
						                                    <item>false</item>
					                                    </domain>
                                                    </literalListParameter>");

            //act
            LiteralListGenericConfig result = (LiteralListGenericConfig)genericConfigXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("From", result.GenericArgumentName);
            Assert.Equal(ListType.GenericList, result.ListType);
            Assert.Equal(2, result.DefaultValues.Count);
            Assert.Equal(2, result.Domain.Count);
        }

        [Fact]
        public void GetGenericObjectListParameterFromXml()
        {
            //arrange
            IGenericConfigXmlParser genericConfigXmlParser = serviceProvider.GetRequiredService<IGenericConfigXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectListParameter genericArgumentName=""From"">
                                                        <objectType>String</objectType>
                                                        <listType>GenericList</listType>
                                                        <control>HashSetForm</control>
                                                    </objectListParameter>");

            //act
            ObjectListGenericConfig result = (ObjectListGenericConfig)genericConfigXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("From", result.GenericArgumentName);
            Assert.Equal("String", result.ObjectType);
            Assert.Equal(ListType.GenericList, result.ListType);
            Assert.Equal(ListParameterInputStyle.HashSetForm, result.Control);
        }

        [Fact]
        public void InvalidXmlElementThrowsException()
        {
            //arrange
            IGenericConfigXmlParser genericConfigXmlParser = serviceProvider.GetRequiredService<IGenericConfigXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<parameter genericArgumentName=""From"">
                                                        <objectType>String</objectType>
                                                        <listType>GenericList</listType>
                                                        <control>HashSetForm</control>
                                                    </parameter>");

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => genericConfigXmlParser.Parse(xmlElement));
        }

        private static XmlElement GetXmlElement(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument.DocumentElement!;
        }
    }
}
