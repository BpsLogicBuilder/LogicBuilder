using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class VariableDataParserTest
    {
        public VariableDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void VariableDataParserWorks()
        {
            //arrange
            IVariableDataParser helper = serviceProvider.GetRequiredService<IVariableDataParser>();
            XmlElement xml = GetXmlElement(@"<variable name=""MyVariable01"" visibleText=""MyVar01"" />");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("MyVariable01", result.Name);
            Assert.Equal("MyVar01", result.VisibleText);
            Assert.Equal(XmlDataConstants.VARIABLEELEMENT, result.VariableElement.Name);
        }

        [Fact]
        public void VariableDataParserThrowsForInvalidElement()
        {
            //arrange
            IVariableDataParser helper = serviceProvider.GetRequiredService<IVariableDataParser>();
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
