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
    public class LiteralListDataParserTest
    {
        public LiteralListDataParserTest()
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
            ILiteralListDataParser helper = serviceProvider.GetRequiredService<ILiteralListDataParser>();
            XmlElement xml = GetXmlElement(@"<literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                <literal>Ay</literal>
                                                <literal>Bee</literal>
                                                <literal>See</literal>
                                                <literal>Dee</literal>
                                                <literal>E</literal>
                                              </literalList>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(LiteralListElementType.String, result.LiteralType);
            Assert.Equal(ListType.GenericList, result.ListType);
            Assert.Equal("www", result.VisibleText);
            Assert.Equal(5, result.ChildElements.Count);
            Assert.Equal(XmlDataConstants.LITERALLISTELEMENT, result.LiteralListElement.Name);
        }

        [Fact]
        public void LiteralListDataParserThrowsForInvalidElement()
        {
            //arrange
            ILiteralListDataParser helper = serviceProvider.GetRequiredService<ILiteralListDataParser>();
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
