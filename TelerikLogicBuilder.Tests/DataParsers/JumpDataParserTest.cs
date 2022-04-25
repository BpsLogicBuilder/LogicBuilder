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
    public class JumpDataParserTest
    {
        public JumpDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(UniversalMasterName.JUMPOBJECT, "HOME", "HOME")]
        [InlineData("XYZ", "HOME", "")]
        public void JumpDataParserReturnsExpectedResults(string nameAttributeValue, string valueText, string expectedResult)
        {
            //arrange
            IJumpDataParser parser = serviceProvider.GetRequiredService<IJumpDataParser>();
            XmlElement xml = GetXmlElement(@$"<shapeData name=""{nameAttributeValue}"">
                                              <value>{valueText}</value>
                                            </shapeData>");

            //act
            var result = parser.Parse(xml);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void JumpDataParserThrowsForInvalidXmlRootElement()
        {
            //arrange
            IJumpDataParser parser = serviceProvider.GetRequiredService<IJumpDataParser>();
            XmlElement xml = GetXmlElement(@$"<invalidRootShapeData name=""{UniversalMasterName.JUMPOBJECT}"">
                                              <value>HOME</value>
                                            </invalidRootShapeData>");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => parser.Parse(xml));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{61BBC8E3-D218-4775-87BA-E235797B994A}"), 
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
