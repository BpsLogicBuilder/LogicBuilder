using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class RetractFunctionDataParserTest
    {
        public RetractFunctionDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void RetractFunctionDataParserWorks()
        {
            //arrange
            IRetractFunctionDataParser helper = serviceProvider.GetRequiredService<IRetractFunctionDataParser>();
            XmlElement xml = GetXmlElement(@"<retractFunction name=""SetToNull"" visibleText=""visibleText"">
                                                <variable name=""MyVariable01"" visibleText=""MyVar01"" />
                                              </retractFunction>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("SetToNull", result.Name);
            Assert.Equal("visibleText", result.VisibleText);
            Assert.Equal(XmlDataConstants.VARIABLEELEMENT, result.VariableElement.Name);
            Assert.Equal(XmlDataConstants.RETRACTFUNCTIONELEMENT, result.RetractFunctionElement.Name);
        }

        [Fact]
        public void RetractFunctionDataParserThrowsForInvalidElement()
        {
            //arrange
            IRetractFunctionDataParser helper = serviceProvider.GetRequiredService<IRetractFunctionDataParser>();
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
