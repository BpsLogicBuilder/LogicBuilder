using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class TableErrorSourceDataParserTest
    {
        public TableErrorSourceDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void TableErrorSourceDataParserWorks()
        {
            //arrange
            ITableErrorSourceDataParser helper = serviceProvider.GetRequiredService<ITableErrorSourceDataParser>();
            XmlElement xml = GetXmlElement(@"<tableErrorSource fileFullName=""C:\folder\file.vsd"" rowIndex=""1"" columnIndex=""3"" />");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(@"C:\folder\file.vsd", result.FileFullName);
            Assert.Equal(1, result.RowIndex);
            Assert.Equal(3, result.ColumnIndex);
        }

        [Fact]
        public void TableErrorSourceDataParserThrowsForInvalidElement()
        {
            //arrange
            ITableErrorSourceDataParser helper = serviceProvider.GetRequiredService<ITableErrorSourceDataParser>();
            XmlElement xml = GetXmlElement(@"<assert name=""Set Variable"" visibleText=""visibleText"">
                                              </assert>");


            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.Parse(xml));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.invalidArgumentTextFormat, "{5D15167F-D61D-4B7F-9B8B-F2C542DCC156}"),
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
