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
    public class MetaObjectDataParserTest
    {
        public MetaObjectDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void LiteralListDataParserWorks()
        {
            //arrange
            IMetaObjectDataParser helper = serviceProvider.GetRequiredService<IMetaObjectDataParser>();
            XmlElement xml = GetXmlElement(@"<metaObject objectType=""Fully.Qualified.Type.Name"">
                                                <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                                  <genericArguments />
                                                  <parameters>
                                                    <literalParameter name=""val1"">
                                                      <variable name=""ZBU"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                    <literalParameter name=""val2"">CS</literalParameter>
                                                  </parameters>
                                                </constructor>
                                              </metaObject>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("Fully.Qualified.Type.Name", result.ObjectType);
            Assert.Equal(ObjectCategory.Constructor, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.METAOBJECTELEMENT, result.MetaObjectElement.Name);
        }

        [Fact]
        public void FunctionsDataParserThrowsForInvalidElement()
        {
            //arrange
            IMetaObjectDataParser helper = serviceProvider.GetRequiredService<IMetaObjectDataParser>();
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
