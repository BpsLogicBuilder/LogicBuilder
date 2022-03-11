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
    public class ObjectListParameterDataParserTest
    {
        public ObjectListParameterDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ObjectListParameterDataParserWorks()
        {
            //arrange
            IObjectListParameterDataParser helper = serviceProvider.GetRequiredService<IObjectListParameterDataParser>();
            XmlElement xml = GetXmlElement(@"<objectListParameter name=""myParamName"">
                                                <objectList objectType=""constructorType"" listType=""GenericList"" visibleText=""visibleText"">
                                                  <object>
                                                    <constructor name=""constructorName"" visibleText=""visibleText"">
                                                      <genericArguments />
                                                      <parameters>
                                                        <literalParameter name=""ff"">XX</literalParameter>
                                                      </parameters>
                                                    </constructor>
                                                  </object>
                                                  <object>
                                                    <constructor name=""constructorName"" visibleText=""visibleText"">
                                                      <genericArguments />
                                                      <parameters>
                                                        <literalParameter name=""ff"">XX</literalParameter>
                                                      </parameters>
                                                    </constructor>
                                                  </object>
                                                </objectList>
                                              </objectListParameter>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("myParamName", result.ParameterName);
            Assert.Equal(ObjectCategory.ObjectList, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.OBJECTLISTPARAMETERELEMENT, result.ObjectListParameterElement.Name);
        }

        [Fact]
        public void ObjectListParameterDataParserThrowsForInvalidElement()
        {
            //arrange
            IObjectListParameterDataParser helper = serviceProvider.GetRequiredService<IObjectListParameterDataParser>();
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
