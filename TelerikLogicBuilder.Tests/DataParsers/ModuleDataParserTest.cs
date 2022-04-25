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
    public class ModuleDataParserTest
    {
        public ModuleDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(UniversalMasterName.MODULE, "About", "About")]
        [InlineData("XYZ", "About", "")]
        public void ModuleDataParserReturnsExpectedResults(string nameAttributeValue, string valueText, string expectedResult)
        {
            //arrange
            IModuleDataParser parser = serviceProvider.GetRequiredService<IModuleDataParser>();
            XmlElement xml = GetXmlElement(@$"<shapeData name=""{nameAttributeValue}"">
                                              <value>{valueText}</value>
                                            </shapeData>");

            //act
            var result = parser.Parse(xml);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ModuleDataParserThrowsForInvalidXmlRootElement()
        {
            //arrange
            IModuleDataParser parser = serviceProvider.GetRequiredService<IModuleDataParser>();
            XmlElement xml = GetXmlElement(@$"<invalidRootShapeData name=""{UniversalMasterName.MODULE}"">
                                              <value>About</value>
                                            </invalidRootShapeData>");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => parser.Parse(xml));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{F3FF3F75-45F3-4816-8333-DE0EA2C9C9CF}"),
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
