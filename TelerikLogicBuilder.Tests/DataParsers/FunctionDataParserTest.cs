using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class FunctionDataParserTest
    {
        public FunctionDataParserTest()
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
            IFunctionDataParser helper = serviceProvider.GetRequiredService<IFunctionDataParser>();
            XmlElement xml = GetXmlElement(@"<not>
                                              <function name=""equals"" visibleText=""visibleText"">
                                                <genericArguments>
                                                    <objectParameter genericArgumentName=""To"">
                                                        <objectType>System.Object</objectType>
                                                        <useForEquality>false</useForEquality>
                                                        <useForHashCode>false</useForHashCode>
                                                        <useForToString>true</useForToString>
                                                    </objectParameter>
                                                </genericArguments>
                                                <parameters>
                                                  <literalParameter name=""val1"">
                                                    <variable name=""desc"" visibleText=""visibleText"" />
                                                  </literalParameter>
                                                  <literalParameter name=""val2"">PHYI</literalParameter>
                                                </parameters>
                                              </function>
                                            </not>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("equals", result.Name);
            Assert.Equal("visibleText", result.VisibleText);
            Assert.Single(result.GenericArguments);
            Assert.Equal(2, result.ParameterElementsList.Count);
            Assert.Equal(XmlDataConstants.NOTELEMENT, result.FunctionElement.Name);
        }

        [Fact]
        public void ConstructorDataParserThrowsForInvalidElement()
        {
            //arrange
            IFunctionDataParser helper = serviceProvider.GetRequiredService<IFunctionDataParser>();
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
