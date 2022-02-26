using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class FragmentXmlParserTest
    {
        public FragmentXmlParserTest()
        {
            Initialize();
        }

        [Fact]
        public void GetFragmenFromXml()
        {
            //arrange
            IFragmentXmlParser fragmentsXmlParser = serviceProvider.GetRequiredService<IFragmentXmlParser>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<fragment name=""ParameterOperatorParameters"">
                                                        <constructor name=""ParameterOperatorParameters"" visibleText =""ParameterOperatorParameters: parameterName=$it"">
                                                        <genericArguments />
                                                        <parameters>
                                                            <literalParameter name=""parameterName"" >$it</literalParameter>
                                                        </parameters>
                                                        </constructor>
                                                    </fragment>");

            //act
            Fragment result = fragmentsXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("ParameterOperatorParameters", result.Name);
            Assert.Equal("constructor", xmlDocumentHelpers.ToXmlDocument(result.Xml).DocumentElement.Name);
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        private static XmlElement GetXmlElement(string xmlString)
            => GetXmlDocument(xmlString).DocumentElement;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }
}
