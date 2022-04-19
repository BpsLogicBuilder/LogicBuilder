using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class ConnectorDataParserTest
    {
        public ConnectorDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ConnectorDataParserWorks()
        {
            //arrange
            IConnectorDataParser helper = serviceProvider.GetRequiredService<IConnectorDataParser>();
            XmlElement xml = GetXmlElement(@"<connector name=""1"" connectorCategory=""0"">
                                              <text>
                                                FFF
                                                <function name=""get message"" visibleText=""visibleText"">
                                                  <genericArguments />
                                                  <parameters>
                                                    <literalParameter name=""value"">
                                                      <function name=""table"" visibleText=""visibleText"">
                                                        <genericArguments />
                                                        <parameters>
                                                          <literalParameter name=""value"">tmq</literalParameter>
                                                          <literalParameter name=""key"">
                                                            <variable name=""tmqkey"" visibleText=""visibleText"" />
                                                          </literalParameter>
                                                          <literalParameter name=""field"">MSGID</literalParameter>
                                                        </parameters>
                                                      </function>
                                                    </literalParameter>
                                                  </parameters>
                                                </function>
                                              </text>
                                              <metaObject objectType=""Fully.Qualified.Type.Name"">
                                                <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                                  <genericArguments />
                                                  <parameters>
                                                    <literalParameter name=""val1"">
                                                      <variable name=""ZBU"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                    <literalParameter name=""val2"">CS</literalParameter>
                                                  </parameters>
                                                </constructor>
                                              </metaObject>
                                            </connector>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(1, result.Index);
            Assert.Equal(ConnectorCategory.Decision, result.ConnectorCategory);
            Assert.Equal(XmlDataConstants.TEXTELEMENT, result.TextXmlNode.Name);
            Assert.Equal(XmlDataConstants.METAOBJECTELEMENT, result.MetaObjectDataXmlNode!.Name);
            Assert.Equal(XmlDataConstants.CONNECTORELEMENT, result.ConnectorElement.Name);
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
