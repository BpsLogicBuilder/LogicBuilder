using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Editing.FindAndReplace
{
    public class SearchFunctionsTest
    {
        public SearchFunctionsTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanCreateSearchFunctions()
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData("FFF", true, true, 1)]
        [InlineData("fff", true, true, 0)]
        [InlineData("FF", true, true, 0)]
        [InlineData("FF", true, false, 1)]
        [InlineData("fff", false, true, 1)]
        [InlineData("Fully.Qualified.Type.Name", true, true, 1)]
        public void FindTextMatchesWorks(string searchString, bool matchCase, bool matchWholeWord, int resultCount)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>FFF</text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.FindTextMatches(xmlString, searchString, matchCase, matchWholeWord);

            //assert
            Assert.Equal(resultCount, results.Count);
        }

        [Theory]
        [InlineData("FFF", "GGG", true, true, "//text", "GGG")]
        [InlineData("fff", "GGG", true, true, "//text", "FFF")]
        [InlineData("FF", "GGG", true, true, "//text", "FFF")]
        [InlineData("FF", "GGG", true, false, "//text", "GGGF")]
        [InlineData("fff", "GGG", false, true, "//text", "GGG")]
        [InlineData("Fully.Qualified.Type.Name", "ReplacedFullyQualifiedTypeName", true, true, "//metaObject/@objectType", "Fully.Qualified.Type.Name")]
        public void ReplaceTextMatchesWorks(string searchString, string replacementString, bool matchCase, bool matchWholeWord, string xPath, string finalValue)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>FFF</text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.ReplaceTextMatches(xmlString, searchString, replacementString, matchCase, matchWholeWord);
            XmlDocument resultDocument = GetXmlDocument(results);

            //assert
            Assert.Equal(finalValue, resultDocument.SelectSingleNode(xPath)!.InnerText);
        }

        [Theory]
        [InlineData("FFF", true, true, 1)]
        [InlineData("fff", true, true, 0)]
        [InlineData("FF", true, true, 0)]
        [InlineData("FF", true, false, 1)]
        [InlineData("fff", false, true, 1)]
        [InlineData("Fully.Qualified.Type.Name", true, true, 0)]
        public void FindConstructorMatchesWorks(string searchString, bool matchCase, bool matchWholeWord, int resultCount)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>AAA</text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""FFF"" visibleText=""FFF"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.FindConstructorMatches(xmlString, searchString, matchCase, matchWholeWord);

            //assert
            Assert.Equal(resultCount, results.Count);
        }

        [Theory]
        [InlineData("FFF", "GGG", true, true, "//constructor/@name", "GGG")]
        [InlineData("fff", "GGG", true, true, "//constructor/@name", "FFF")]
        [InlineData("FF", "GGG", true, true, "//constructor/@name", "FFF")]
        [InlineData("FF", "GGG", true, false, "//constructor/@name", "GGGF")]
        [InlineData("fff", "GGG", false, true, "//constructor/@name", "GGG")]
        [InlineData("Fully.Qualified.Type.Name", "ReplacedFullyQualifiedTypeName", true, true, "//constructor/@name", "FFF")]
        public void ReplaceTConstructoratchesWorks(string searchString, string replacementString, bool matchCase, bool matchWholeWord, string xPath, string finalValue)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>AAA</text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""FFF"" visibleText=""FFF"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.ReplaceConstructorMatches(xmlString, searchString, replacementString, matchCase, matchWholeWord);
            XmlDocument resultDocument = GetXmlDocument(results);

            //assert
            Assert.Equal(finalValue, resultDocument.SelectSingleNode(xPath)!.InnerText);
        }

        [Theory]
        [InlineData("FFF", true, true, 1)]
        [InlineData("fff", true, true, 0)]
        [InlineData("FF", true, true, 0)]
        [InlineData("FF", true, false, 1)]
        [InlineData("fff", false, true, 1)]
        [InlineData("Fully.Qualified.Type.Name", true, true, 0)]
        public void FindFunctionMatchesWorks(string searchString, bool matchCase, bool matchWholeWord, int resultCount)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>
                                        AAA
                                        <function name=""FFF"" visibleText=""FFF"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""value"">
                                                <function name=""table"" visibleText=""visibleText"">
                                                <genericArguments />
                                                <parameters>
                                                    <literalParameter name=""value"">tmq</literalParameter>
                                                    <literalParameter name=""key"">
                                                    <variable name=""tmqkey"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                    <literalParameter name=""field"">MSGID</literalParameter>
                                                </parameters>
                                                </function>
                                            </literalParameter>
                                            </parameters>
                                        </function>
                                    </text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.FindFunctionMatches(xmlString, searchString, matchCase, matchWholeWord);

            //assert
            Assert.Equal(resultCount, results.Count);
        }

        [Theory]
        [InlineData("FFF", "GGG", true, true, "//function/@name", "GGG")]
        [InlineData("fff", "GGG", true, true, "//function/@name", "FFF")]
        [InlineData("FF", "GGG", true, true, "//function/@name", "FFF")]
        [InlineData("FF", "GGG", true, false, "//function/@name", "GGGF")]
        [InlineData("fff", "GGG", false, true, "//function/@name", "GGG")]
        [InlineData("Fully.Qualified.Type.Name", "ReplacedFullyQualifiedTypeName", true, true, "//function/@name", "FFF")]
        public void ReplaceFunctionMatchesWorks(string searchString, string replacementString, bool matchCase, bool matchWholeWord, string xPath, string finalValue)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>
                                        AAA
                                        <function name=""FFF"" visibleText=""FFF"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""value"">
                                                <function name=""table"" visibleText=""visibleText"">
                                                <genericArguments />
                                                <parameters>
                                                    <literalParameter name=""value"">tmq</literalParameter>
                                                    <literalParameter name=""key"">
                                                    <variable name=""tmqkey"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                    <literalParameter name=""field"">MSGID</literalParameter>
                                                </parameters>
                                                </function>
                                            </literalParameter>
                                            </parameters>
                                        </function>
                                    </text>
                                </connector>";

            //act
            var results = helper.ReplaceFunctionMatches(xmlString, searchString, replacementString, matchCase, matchWholeWord);
            XmlDocument resultDocument = GetXmlDocument(results);

            //assert
            Assert.Equal(finalValue, resultDocument.SelectSingleNode(xPath)!.InnerText);
        }

        [Theory]
        [InlineData("FFF", true, true, 1)]
        [InlineData("fff", true, true, 0)]
        [InlineData("FF", true, true, 0)]
        [InlineData("FF", true, false, 1)]
        [InlineData("fff", false, true, 1)]
        [InlineData("Fully.Qualified.Type.Name", true, true, 0)]
        public void FindVariableMatchesWorks(string searchString, bool matchCase, bool matchWholeWord, int resultCount)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>
                                        AAA
                                        <variable name=""FFF"" visibleText=""FFF"" />
                                    </text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.FindVariableMatches(xmlString, searchString, matchCase, matchWholeWord);

            //assert
            Assert.Equal(resultCount, results.Count);
        }

        [Theory]
        [InlineData("FFF", "GGG", true, true, "//variable/@name", "GGG")]
        [InlineData("fff", "GGG", true, true, "//variable/@name", "FFF")]
        [InlineData("FF", "GGG", true, true, "//variable/@name", "FFF")]
        [InlineData("FF", "GGG", true, false, "//variable/@name", "GGGF")]
        [InlineData("fff", "GGG", false, true, "//variable/@name", "GGG")]
        [InlineData("Fully.Qualified.Type.Name", "ReplacedFullyQualifiedTypeName", true, true, "//variable/@name", "FFF")]
        public void ReplaceVariableMatchesWorks(string searchString, string replacementString, bool matchCase, bool matchWholeWord, string xPath, string finalValue)
        {
            //arrange
            ISearchFunctions helper = serviceProvider.GetRequiredService<ISearchFunctions>();
            string xmlString = @"<connector name=""1"" connectorCategory=""1"">
                                    <text>
                                        AAA
                                        <variable name=""FFF"" visibleText=""FFF"" />
                                    </text>
                                    <metaObject objectType=""Fully.Qualified.Type.Name"">
                                        <constructor name=""StringQuestionDataParameters"" visibleText=""StringQuestionDataParameters"">
                                            <genericArguments />
                                            <parameters>
                                            <literalParameter name=""val1"">
                                                <variable name=""ZBU"" visibleText=""visibleText"" />
                                            </literalParameter>
                                            <literalParameter name=""val2"">CS</literalParameter>
                                            </parameters>
                                        </constructor>
                                    </metaObject>
                                </connector>";

            //act
            var results = helper.ReplaceVariableMatches(xmlString, searchString, replacementString, matchCase, matchWholeWord);
            XmlDocument resultDocument = GetXmlDocument(results);

            //assert
            Assert.Equal(finalValue, resultDocument.SelectSingleNode(xPath)!.InnerText);
        }

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields
    }
}
