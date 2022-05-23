using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator.RuleBuilders
{
    public class TableResourcesManagerTest
    {
        public TableResourcesManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateDiagramResourcesManager()
        {
            //arrange
            ITableResourcesManager helper = serviceProvider.GetRequiredService<ITableResourcesManager>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData("Start Date is required", "mymodule:tb_SDIR")]
        [InlineData("Credits must be between 0 and 5 inclusive.", "mymodule:tb_CMBB0A")]
        public void GetShortStringGivenLongStringWorks(string longString, string expectedShortString)
        {
            //arrange
            ITableResourcesManager helper = serviceProvider.GetRequiredService<ITableResourcesManager>();
            Dictionary<string, string> resourseStrings = new();
            string moduleName = "mymodule";


            //act
            var result = helper.GetShortString(longString, resourseStrings, moduleName);

            //assert
            Assert.Equal(expectedShortString, result);
        }

        [Theory]
        [InlineData("<literal>Start Date is required</literal>", "mymodule:tb_SDIR")]
        [InlineData("<literal>Credits must be between 0 and 5 inclusive.</literal>", "mymodule:tb_CMBB0A")]
        [InlineData(@"<literal>The <variable name=""StringItem"" visibleText=""StringItem"" /> brown fox jumped over the lazy dog.</literal>", "mymodule:tb_TABFJO")]
        [InlineData(@"<literal>The <constructor name=""String"" visibleText=""String"" >
                                        <genericArguments />
                                        <parameters>
                                            <literalListParameter name=""stringArray"">
                                                <literalList literalType=""String"" listType=""Array"" visibleText=""visibleText"">
                                                    <literal>A</literal>
                                                    <literal>B</literal>
                                                </literalList>
                                            </literalListParameter>
                                        </parameters>
                                    </constructor> brown <function name=""GetString"" visibleText=""GetString"">
                                        <genericArguments />
                                        <parameters />
                                    </function> jumped <variable name=""StringItem"" visibleText=""StringItem"" /> the lazy dog.</literal>", "mymodule:tb_TABAJA")]
        public void GetShortStringGivenXmlNodeWorks(string xmlString, string expectedShortString)
        {
            //arrange
            ITableResourcesManager helper = serviceProvider.GetRequiredService<ITableResourcesManager>();
            Dictionary<string, string> resourseStrings = new();
            string moduleName = "mymodule";


            //act
            var result = helper.GetShortString(GetXmlElement(xmlString), resourseStrings, moduleName);

            //assert
            Assert.Equal(expectedShortString, result);
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
