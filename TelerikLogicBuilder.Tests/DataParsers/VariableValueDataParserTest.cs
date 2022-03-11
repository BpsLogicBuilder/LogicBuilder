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
    public class VariableValueDataParserTest
    {
        public VariableValueDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void VariableValueDataParserWorks()
        {
            //arrange
            IVariableValueDataParser helper = serviceProvider.GetRequiredService<IVariableValueDataParser>();
            XmlElement xml = GetXmlElement(@"<variableValue>
                                              <objectVariable>
                                                <constructor name=""CC"" visibleText=""CC"">
                                                  <genericArguments></genericArguments>
                                                  <parameters />
                                                </constructor>
                                              </objectVariable>
                                            </variableValue>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(VariableTypeCategory.Object, result.ChildElementCategory);
            Assert.Equal(XmlDataConstants.VARIABLEVALUEELEMENT, result.VariableValueElement.Name);
        }

        [Fact]
        public void VariableValueDataParserThrowsForInvalidElement()
        {
            //arrange
            IVariableValueDataParser helper = serviceProvider.GetRequiredService<IVariableValueDataParser>();
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
