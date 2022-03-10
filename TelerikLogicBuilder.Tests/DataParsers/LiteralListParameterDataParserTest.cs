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
    public class LiteralListParameterDataParserTest
    {
        public LiteralListParameterDataParserTest()
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
            ILiteralListParameterDataParser helper = serviceProvider.GetRequiredService<ILiteralListParameterDataParser>();
            XmlElement xml = GetXmlElement(@"<literalListParameter name=""Includes"">
                                                <literalList literalType=""String"" listType=""GenericList"" visibleText=""www"">
                                                  <literal>Field1</literal>
                                                  <literal>Field2</literal>
                                                </literalList>
                                              </literalListParameter>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("Includes", result.ParameterName);
            Assert.Equal(ObjectCategory.LiteralList, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.LITERALLISTPARAMETERELEMENT, result.LiteralListParameterElement.Name);
        }

        [Fact]
        public void FunctionsDataParserThrowsForInvalidElement()
        {
            //arrange
            ILiteralListParameterDataParser helper = serviceProvider.GetRequiredService<ILiteralListParameterDataParser>();
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
