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
    public class DiagramErrorSourceDataParserTest
    {
        public DiagramErrorSourceDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void DiagramErrorSourceDataParserWorks()
        {
            //arrange
            IDiagramErrorSourceDataParser helper = serviceProvider.GetRequiredService<IDiagramErrorSourceDataParser>();
            XmlElement xml = GetXmlElement(@"<diagramErrorSource fileFullName=""C:\folder\file.vsd"" pageIndex=""1"" shapeIndex=""25"" pageId=""0"" shapeId=""14""/>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(@"C:\folder\file.vsd", result.FileFullName);
            Assert.Equal(1, result.PageIndex);
            Assert.Equal(25, result.ShapeIndex);
            Assert.Equal(0, result.PageId);
            Assert.Equal(14, result.ShapeId);
        }

        [Fact]
        public void DiagramErrorSourceDataParserThrowsForInvalidElement()
        {
            //arrange
            IDiagramErrorSourceDataParser helper = serviceProvider.GetRequiredService<IDiagramErrorSourceDataParser>();
            XmlElement xml = GetXmlElement(@"<assert name=""Set Variable"" visibleText=""visibleText"">
                                              </assert>");


            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.Parse(xml));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.invalidArgumentTextFormat, "{A5804289-F2BE-4B8E-9A2E-288593E9289E}"),
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
