using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class DecisionsDataParserTest
    {
        public DecisionsDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void DecisionsDataParserWorks()
        {
            //arrange
            IDecisionsDataParser helper = serviceProvider.GetRequiredService<IDecisionsDataParser>();
            XmlElement xml = GetXmlElement(@"<decisions>
                                              <and>
                                                <decision name=""ACIS ZBU"" visibleText=""ACIS ZBU equals CB"">
                                                  <function name=""Contains"" visibleText=""Contains a_zbu"">
                                                    <genericArguments />
                                                    <parameters>
                                                      <literalParameter name=""val1"">
                                                        <variable name=""a_zbu"" visibleText=""visibleText"" />
                                                      </literalParameter>
                                                      <literalParameter name=""val2"">CB</literalParameter>
                                                    </parameters>
                                                  </function>
                                                </decision>
                                                <decision name=""MLT ACRG"" visibleText=""MLT ACRG GT 4 and LT 8"">
                                                  <and>
                                                    <not>
                                                      <function name=""Greater ThanUS"" visibleText=""MLT ACRG GT 4"">
                                                        <genericArguments />
                                                        <parameters>
                                                          <literalParameter name=""value1US"">
                                                            <variable name=""MLT ACRG"" visibleText=""MLT ACRG"" />
                                                          </literalParameter>
                                                          <literalParameter name=""value2US"">4</literalParameter>
                                                        </parameters>
                                                      </function>
                                                    </not>
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
                                                </decision>
                                              </and>
                                            </decisions>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("and", result.FirstChildElementName);
            Assert.Equal(2, result.DecisionElements.Count);
            Assert.Equal(XmlDataConstants.DECISIONSELEMENT, result.DecisionsElement.Name);
        }

        [Fact]
        public void DecisionsDataParserThrowsForInvalidElement()
        {
            //arrange
            IDecisionsDataParser helper = serviceProvider.GetRequiredService<IDecisionsDataParser>();
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
