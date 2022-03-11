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
    public class ObjectListVariableDataParserTest
    {
        public ObjectListVariableDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ObjectListDataParserWorks()
        {
            //arrange
            IObjectListVariableDataParser helper = serviceProvider.GetRequiredService<IObjectListVariableDataParser>();
            XmlElement xml = GetXmlElement(@"<objectListVariable>
                                                <objectList objectType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                                </objectList>
                                              </objectListVariable>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(ObjectCategory.ObjectList, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.OBJECTLISTVARIABLEELEMENT, result.ObjectListVariableElement.Name);
        }

        [Fact]
        public void FunctionsDataParserThrowsForInvalidElement()
        {
            //arrange
            IObjectListVariableDataParser helper = serviceProvider.GetRequiredService<IObjectListVariableDataParser>();
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
