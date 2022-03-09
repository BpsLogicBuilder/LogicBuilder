using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class DecisionDataParserTest
    {
        public DecisionDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void DecisionDataParserWorks()
        {
            //arrange
            IDecisionDataParser helper = serviceProvider.GetRequiredService<IDecisionDataParser>();
            XmlElement xml = GetXmlElement(@"<decision name=""IsMedeionGrade"" visibleText=""Grade GT 4 and LT 8"">
                                                <and>
                                                  <function name=""Greater ThanUS"" visibleText=""MLT ACRG GT 4"">
                                                    <genericArguments />
                                                    <parameters>
                                                      <literalParameter name=""value1US"">
                                                        <variable name=""MLT ACRG"" visibleText=""MLT ACRG"" />
                                                      </literalParameter>
                                                      <literalParameter name=""value2US"">4</literalParameter>
                                                    </parameters>
                                                  </function>
                                                  <function name=""Less ThanUS"" visibleText=""MLT ACRG LT 8"">
                                                    <genericArguments />
                                                    <parameters>
                                                      <literalParameter name=""value1US"">
                                                        <variable name=""MLT ACRG"" visibleText=""MLT ACRG"" />
                                                      </literalParameter>
                                                      <literalParameter name=""value2US"">8</literalParameter>
                                                    </parameters>
                                                  </function>
                                                </and>
                                              </decision>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("and", result.FirstChildElementName);
            Assert.Equal(2, result.FunctionElements.Count);
            Assert.Equal(XmlDataConstants.DECISIONELEMENT, result.DecisionElement.Name);
        }

        [Fact]
        public void DecisionDataParserThrowsForInvalidElement()
        {
            //arrange
            IDecisionDataParser helper = serviceProvider.GetRequiredService<IDecisionDataParser>();
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
