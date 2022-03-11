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
    public class ObjectListDataParserTest
    {
        public ObjectListDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ObjectListDataParserWorks()
        {
            //arrange
            IObjectListDataParser helper = serviceProvider.GetRequiredService<IObjectListDataParser>();
            XmlElement xml = GetXmlElement(@"<objectList objectType=""KeyValuePair`2[String,String]"" listType=""GenericList"" visibleText=""vvv"">
                                                <object>
                                                  <constructor name=""KeyValuePair`2[String,String]"" visibleText=""KeyValuePair`2[String,String]"">
                                                    <genericArguments />
                                                    <parameters>
                                                      <literalParameter name=""key"">type</literalParameter>
                                                      <literalParameter name=""value"">number</literalParameter>
                                                    </parameters>
                                                  </constructor>
                                                </object>
                                                <object>
                                                  <constructor name=""ddd"" visibleText=""vvv"">
                                                    <genericArguments />
                                                    <parameters>
                                                      <literalParameter name=""key"">disbabled</literalParameter>
                                                      <literalParameter name=""value"">disabled</literalParameter>
                                                    </parameters>
                                                  </constructor>
                                                </object>
                                              </objectList>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("KeyValuePair`2[String,String]", result.ObjectType);
            Assert.Equal(ListType.GenericList, result.ListType);
            Assert.Equal("vvv", result.VisibleText);
            Assert.Equal(2, result.ChildElements.Count);
            Assert.Equal(XmlDataConstants.OBJECTLISTELEMENT, result.ObjectListElement.Name);
        }

        [Fact]
        public void ObjectListDataParserThrowsForInvalidElement()
        {
            //arrange
            IObjectListDataParser helper = serviceProvider.GetRequiredService<IObjectListDataParser>();
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
