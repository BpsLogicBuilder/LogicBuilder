using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.DataParsers
{
    public class FunctionsDataParserTest
    {
        public FunctionsDataParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void FunctionsDataParserWorks()
        {
            //arrange
            IFunctionsDataParser helper = serviceProvider.GetRequiredService<IFunctionsDataParser>();
            XmlElement xml = GetXmlElement(@"<functions>
                                              <function name=""equals"" visibleText=""visibleText"">
                                                <genericArguments>
                                                    <objectParameter genericArgumentName=""To"">
                                                        <objectType>System.Object</objectType>
                                                        <useForEquality>false</useForEquality>
                                                        <useForHashCode>false</useForHashCode>
                                                        <useForToString>true</useForToString>
                                                    </objectParameter>
                                                </genericArguments>
                                                <parameters>
                                                  <literalParameter name=""val1"">
                                                    <variable name=""desc"" visibleText=""visibleText"" />
                                                  </literalParameter>
                                                  <literalParameter name=""val2"">PHYI</literalParameter>
                                                </parameters>
                                              </function>
                                              <assertFunction name=""set condition"" visibleText=""visibleText"">
                                                <variable name=""My Object List Var"" visibleText=""My Object List Var"" />
                                                <variableValue>
                                                  <objectListVariable>
                                                    <objectList objectType=""String"" listType=""GenericList"" visibleText=""visibleText"">
                                                    </objectList>
                                                  </objectListVariable>
                                                </variableValue>
                                              </assertFunction>
                                              <retractFunction name=""remove condition"" visibleText=""visibleText"">
                                                <variable name=""ltn1"" visibleText=""visibleText"" />
                                              </retractFunction>
                                            </functions>");

            //act
            var result = helper.Parse(xml);

            //assert
            Assert.Equal(3, result.FunctionElements.Count);
            Assert.Equal(XmlDataConstants.FUNCTIONSELEMENT, result.FunctionsElement.Name);
        }

        [Fact]
        public void FunctionsDataParserThrowsForInvalidElement()
        {
            //arrange
            IFunctionsDataParser helper = serviceProvider.GetRequiredService<IFunctionsDataParser>();
            XmlElement xml = GetXmlElement(@"<assert name=""Set Variable"" visibleText=""visibleText"">
                                              </assert>");


            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => helper.Parse(xml));
        }

        [Fact]
        public void FunctionsDataParserThrowsForInvalidFunctionElementName()
        {
            //arrange
            IFunctionsDataParser helper = serviceProvider.GetRequiredService<IFunctionsDataParser>();
            XmlElement xml = GetXmlElement(@"<functions>
                                              <setFunction name=""remove condition"" visibleText=""visibleText"">
                                                <variable name=""ltn1"" visibleText=""visibleText"" />
                                              </setFunction>
                                            </functions>");


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
