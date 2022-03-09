using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class ConditionsDataParserTest
    {
        public ConditionsDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ConditionsDataParserWorks()
        {
            //arrange
            IConditionsDataParser helper = serviceProvider.GetRequiredService<IConditionsDataParser>();
            XmlElement xml = GetXmlElement(@"<conditions>
                                              <or>
                                                <not>
                                                  <function name=""equals"" visibleText=""visibleText"">
                                                    <genericArguments />
                                                    <parameters>
                                                      <literalParameter name=""val1"">
                                                        <variable name=""desc"" visibleText=""visibleText"" />
                                                      </literalParameter>
                                                      <literalParameter name=""val2"">PHYI</literalParameter>
                                                    </parameters>
                                                  </function>
                                                </not>
                                                <function name=""contains"" visibleText=""visibleText"">
                                                  <genericArguments />
                                                  <parameters>
                                                    <literalParameter name=""val1"">
                                                      <function name=""table"" visibleText=""visibleText"">
                                                        <genericArguments />
                                                        <parameters>
                                                          <literalParameter name=""value"">regulatory</literalParameter>
                                                          <literalParameter name=""key"">DRTMQ</literalParameter>
                                                          <literalParameter name=""field"">STATES</literalParameter>
                                                        </parameters>
                                                      </function>
                                                    </literalParameter>
                                                    <literalParameter name=""val2"">
                                                      <variable name=""state"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                  </parameters>
                                                </function>
                                              </or>
                                            </conditions>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("or", result.FirstChildElementName);
            Assert.Equal(2, result.FunctionElements.Count);
            Assert.Equal(XmlDataConstants.CONDITIONSELEMENT, result.ConditionsElement.Name);
        }

        [Fact]
        public void ConditionsDataParserThrowsForInvalidElement()
        {
            //arrange
            IConditionsDataParser helper = serviceProvider.GetRequiredService<IConditionsDataParser>();
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
