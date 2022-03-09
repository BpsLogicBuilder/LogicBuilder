using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class ConstructorDataParserTest
    {
        public ConstructorDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ConstructorDataParserWorks()
        {
            //arrange
            IConstructorDataParser helper = serviceProvider.GetRequiredService<IConstructorDataParser>();
            XmlElement xml = GetXmlElement(@"<constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                                <genericArguments>
                                                    <literalListParameter genericArgumentName=""From"">
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
                                                    </literalListParameter>
                                                    <objectParameter genericArgumentName=""To"">
                                                        <objectType>System.Object</objectType>
                                                        <useForEquality>false</useForEquality>
                                                        <useForHashCode>false</useForHashCode>
                                                        <useForToString>true</useForToString>
                                                    </objectParameter>
                                                </genericArguments>
                                                <parameters>
                                                    <literalParameter name=""val1"">
                                                        <variable name=""ZBU"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                    <literalParameter name=""val2"">CS</literalParameter>
                                                </parameters>
                                            </constructor>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("StringQuestionDataParameters", result.Name);
            Assert.Equal("StringQuestionDataParameters", result.VisibleText);
            Assert.Equal(2, result.GenericArguments.Count);
            Assert.Equal(2, result.ParameterElementsList.Count);
            Assert.Equal(XmlDataConstants.CONSTRUCTORELEMENT, result.ConstructorElement.Name);
        }

        [Fact]
        public void ConstructorDataParserThrowsForInvalidElement()
        {
            //arrange
            IConstructorDataParser helper = serviceProvider.GetRequiredService<IConstructorDataParser>();
            XmlElement xml = GetXmlElement(@"<assert name=""Set Variable"" visibleText=""visibleText"">
                                              </assert>");


            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => helper.Parse(xml));
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
