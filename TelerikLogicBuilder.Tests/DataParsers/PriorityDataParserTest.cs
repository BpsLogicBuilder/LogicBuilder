using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class PriorityDataParserTest
    {
        public PriorityDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(TableColumnName.PRIORITYCOLUMN, "3", 3)]
        [InlineData(TableColumnName.PRIORITYCOLUMN, "4", 4)]
        [InlineData(TableColumnName.PRIORITYCOLUMN, "four", null)]
        [InlineData("XYZ", "3", null)]
        public void PriorityDataParserReturnsExpectedResults(string nameAttributeValue, string valueText, int? expectedResult)
        {
            //arrange
            IPriorityDataParser parser = serviceProvider.GetRequiredService<IPriorityDataParser>();
            XmlElement xml = GetXmlElement(@$"<shapeData name=""{nameAttributeValue}"">
                                              <value>{valueText}</value>
                                            </shapeData>");

            //act
            var result = parser.Parse(xml);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void PriorityDataParserThrowsForInvalidXmlRootElement()
        {
            //arrange
            IPriorityDataParser parser = serviceProvider.GetRequiredService<IPriorityDataParser>();
            XmlElement xml = GetXmlElement(@$"<invalidRootShapeData name=""{TableColumnName.PRIORITYCOLUMN}"">
                                              <value>2</value>
                                            </invalidRootShapeData>");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => parser.Parse(xml));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{33F56766-7ACF-4CA6-A85F-C30B3E3B1072}"),
                exception.Message
            );
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
