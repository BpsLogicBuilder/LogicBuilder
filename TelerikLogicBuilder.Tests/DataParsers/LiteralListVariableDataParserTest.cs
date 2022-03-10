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
    public class LiteralListVariableDataParserTest
    {
        public LiteralListVariableDataParserTest()
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
            ILiteralListVariableDataParser helper = serviceProvider.GetRequiredService<ILiteralListVariableDataParser>();
            XmlElement xml = GetXmlElement(@"<literalListVariable>
                                                <literalList literalType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                                </literalList>
                                              </literalListVariable>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(ObjectCategory.LiteralList, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.LITERALLISTVARIABLEELEMENT, result.LiteralListVariableElement.Name);
        }

        [Fact]
        public void FunctionsDataParserThrowsForInvalidElement()
        {
            //arrange
            ILiteralListVariableDataParser helper = serviceProvider.GetRequiredService<ILiteralListVariableDataParser>();
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
