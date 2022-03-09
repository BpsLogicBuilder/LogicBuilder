using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class AssertFunctionDataParserTest
    {
        public AssertFunctionDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void AssertFunctionDataParserWorks()
        {
            //arrange
            IAssertFunctionDataParser helper = serviceProvider.GetRequiredService<IAssertFunctionDataParser>();
            XmlElement xml = GetXmlElement(@"<assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                                <variable name=""MyVariable01"" visibleText=""MyVar01"" />
                                                <variableValue>
                                                  <literalVariable>
                                                    <function name=""substring"" visibleText=""visibleText"">
                                                      <genericArguments />
                                                      <parameters>
                                                        <literalParameter name=""value"">
                                                          <variable name=""ltn1"" visibleText=""visibleText"" />
                                                        </literalParameter>
                                                        <literalParameter name=""start"">3</literalParameter>
                                                        <literalParameter name=""finish"">12</literalParameter>
                                                      </parameters>
                                                    </function>
                                                  </literalVariable>
                                                </variableValue>
                                              </assertFunction>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("Set Variable", result.Name);
            Assert.Equal("visibleText", result.VisibleText);
            Assert.Equal(XmlDataConstants.VARIABLEELEMENT, result.VariableElement.Name);
            Assert.Equal(XmlDataConstants.VARIABLEVALUEELEMENT, result.VariableValueElement.Name);
            Assert.Equal(XmlDataConstants.ASSERTFUNCTIONELEMENT, result.AssertFunctionElement.Name);
        }

        [Fact]
        public void AssertFunctionDataParserThrowsForInvalidElement()
        {
            //arrange
            IAssertFunctionDataParser helper = serviceProvider.GetRequiredService<IAssertFunctionDataParser>();
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
