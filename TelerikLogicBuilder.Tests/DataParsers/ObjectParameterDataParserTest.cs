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
    public class ObjectParameterDataParserTest
    {
        public ObjectParameterDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ObjectParameterDataParserWorks()
        {
            //arrange
            IObjectParameterDataParser helper = serviceProvider.GetRequiredService<IObjectParameterDataParser>();
            XmlElement xml = GetXmlElement(@"<objectParameter name=""dept1"">
                                              <variable name=""vname"" visibleText =""visibleText"" />
                                            </objectParameter>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal("dept1", result.ParameterName);
            Assert.Equal(ObjectCategory.Variable, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.OBJECTPARAMETERELEMENT, result.ObjectParameterElement.Name);
        }

        [Fact]
        public void ObjectParameterDataParserThrowsForInvalidElement()
        {
            //arrange
            IObjectParameterDataParser helper = serviceProvider.GetRequiredService<IObjectParameterDataParser>();
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
