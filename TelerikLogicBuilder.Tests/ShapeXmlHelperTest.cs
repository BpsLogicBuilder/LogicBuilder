using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Xunit;


namespace TelerikLogicBuilder.Tests
{
    public class ShapeXmlHelperTest : IClassFixture<ShapeXmlHelperFixture>
    {
        private readonly ShapeXmlHelperFixture _fixture;

        public ShapeXmlHelperTest(ShapeXmlHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateShapeXmlHelper()
        {
            //arrange
            IShapeXmlHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData(UniversalMasterName.ACTION, XmlDataConstants.FUNCTIONSELEMENT)]
        [InlineData(UniversalMasterName.DIALOG, XmlDataConstants.FUNCTIONSELEMENT)]
        [InlineData(UniversalMasterName.CONDITIONOBJECT, XmlDataConstants.CONDITIONSELEMENT)]
        [InlineData(UniversalMasterName.DECISIONOBJECT, XmlDataConstants.DECISIONSELEMENT)]
        [InlineData(UniversalMasterName.JUMPOBJECT, XmlDataConstants.SHAPEDATAELEMENT)]
        [InlineData(UniversalMasterName.CONNECTOBJECT, XmlDataConstants.CONNECTORELEMENT)]
        public void GetXmlStringWorksForAllShapes(string masterName, string rootElementName)
        {
            //arrange
            IShapeXmlHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();
            Shape shape = GetOnlyShape(masterName);

            //act
            XmlElement xmlElement = GetXmlElement(helper.GetXmlString(shape));

            //assert
            Assert.Equal
            (
                rootElementName,
                xmlElement.Name
            );
        }

        [Fact]
        public void GetXmlStringThrowsForInvalidMaster()
        {
            //arrange
            IShapeXmlHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();
            Shape shape = GetOnlyShape("Process");

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetXmlString(shape));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{C8295A80-33B0-495C-B268-228FA7C0A221}"),
                exception.Message
            );
        }

        public static List<object[]> ShapeXml_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        UniversalMasterName.ACTION,
                        $@"<functions>
                            <assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                <variableValue>
                                    <literalVariable>CB</literalVariable>
                                </variableValue>
                            </assertFunction>
                            <retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                            </retractFunction>
                            <function name=""StaticVoidMethod"" visibleText=""visibleText"">
                                <genericArguments />
                                <parameters>
                                    <literalParameter name=""arg1"">
                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                    </literalParameter>
                                    <literalParameter name=""arg2"">SomeString</literalParameter>
                                </parameters>
                            </function>
                            <function name=""WriteToLog"" visibleText=""visibleText"">
                                <genericArguments />
                                <parameters>
                                    <literalParameter name=""message"">
                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                    </literalParameter>
                                </parameters>
                            </function>
                        </functions>",
                        "Some Functions"
                    },
                    new object[]
                    {
                        UniversalMasterName.DIALOG,
                        $@"<functions>
                            <function name=""StaticVoidMethod"" visibleText=""visibleText"">
                                <genericArguments />
                                <parameters>
                                    <literalParameter name=""arg1"">
                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                    </literalParameter>
                                    <literalParameter name=""arg2"">SomeString</literalParameter>
                                </parameters>
                            </function>
                        </functions>",
                        "One Dialog Function"
                    },
                    new object[]
                    {
                        UniversalMasterName.CONDITIONOBJECT,
                        $@"<conditions>
                            <and>
                                <not>
                                    <function name=""Greater Than"" visibleText=""visibleText"">
                                    <genericArguments />
                                    <parameters>
                                        <literalParameter name=""val1"">
                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Integer)}Item"" visibleText=""visibleText"" />
                                        </literalParameter>
                                        <literalParameter name=""val2"">0</literalParameter>
                                    </parameters>
                                    </function>
                                </not>
                                <function name=""Equals"" visibleText=""visibleText"">
                                    <genericArguments />
                                    <parameters>
                                        <literalParameter name=""val1"">
                                            <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                        </literalParameter>
                                        <literalParameter name=""val2"">SomeString</literalParameter>
                                    </parameters>
                                </function>
                            </and>
                        </conditions>",
                        "Comditions"
                    },
                    new object[]
                    {
                        UniversalMasterName.DECISIONOBJECT,
                        $@"<decisions>
                                <and>
                                    <decision name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" >
                                        <function name=""Equals"" visibleText=""visibleText"">
                                            <genericArguments />
                                            <parameters>
                                                <literalParameter name=""val1"">
                                                    <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.Double)}Item"" visibleText=""visibleText"" />
                                                </literalParameter>
                                                <literalParameter name=""val2"">0.11</literalParameter>
                                            </parameters>
                                        </function>
                                    </decision>
                                    <decision name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" >
                                        <not>
                                            <function name=""Equals"" visibleText=""visibleText"">
                                                <genericArguments />
                                                <parameters>
                                                    <literalParameter name=""val1"">
                                                        <variable name=""{Enum.GetName(typeof(LiteralVariableType), LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                                    </literalParameter>
                                                    <literalParameter name=""val2"">zzz</literalParameter>
                                                </parameters>
                                            </function>
                                        </not>
                                    </decision>
                                </and>
                            </decisions>",
                        "Decisions"
                    },
                    new object[]
                    {
                        UniversalMasterName.JUMPOBJECT,
                        $@"<shapeData name=""module"">
                                <value>Some Value</value>
                            </shapeData>",
                        "Some Value"
                    },
                    new object[]
                    {
                        UniversalMasterName.CONNECTOBJECT,
                        $@"<connector name=""1"" connectorCategory=""1"">
                              <text>
                                FFF
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
                            </connector>",
                        "1: Some Text"
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(ShapeXml_Data))]
        public void CanSaveXmlToAllShapes(string masterName, string xmlToSave, string visibleText)
        {
            //arrange
            IShapeXmlHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();
            Shape shape = GetOnlyShape(masterName);

            //act
            helper.SetXmlString(shape, xmlToSave, visibleText);
            string savedString = helper.GetXmlString(shape);
            _fixture.VisioDocument.Saved = true;

            ////assert
            Assert.Equal
            (
                xmlToSave,
                savedString
            );
            Assert.Equal
            (
                visibleText,
                shape.Text
            );
        }

        [Fact]
        public void UpdateFailsForInvalidSchema()
        {
            //arrange
            IShapeXmlHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION);
            string initialString = helper.GetXmlString(shape);
            string initialText = shape.Text;

            //act
            helper.SetXmlString(shape, "<text>FFF</text>", "visibleText");
            string savedString = helper.GetXmlString(shape);

            ////assert
            Assert.Equal
            (
                initialString,
                savedString
            );
            Assert.Equal
            (
                initialText,
                shape.Text
            );
        }

        [Fact]
        public void UpdateFailsForInvalidXml()
        {
            //arrange
            IShapeXmlHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeXmlHelper>();
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION);
            string initialString = helper.GetXmlString(shape);
            string initialText = shape.Text;

            //act
            helper.SetXmlString(shape, "<someElement>FFF</mismatchedClosingTag>", "visibleText");
            string savedString = helper.GetXmlString(shape);

            ////assert
            Assert.Equal
            (
                initialString,
                savedString
            );
            Assert.Equal
            (
                initialText,
                shape.Text
            );
        }

        private Shape GetOnlyShape(string masterName)
        {
            return _fixture.VisioDocument.Pages
                .OfType<Page>()
                .Single()
                .Shapes
                .OfType<Shape>()
                .Single(s => s.Master.NameU == masterName);
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

    public class ShapeXmlHelperFixture : IDisposable
    {
        internal InvisibleApp VisioApplication;
        internal Document VisioDocument;
        internal IServiceProvider ServiceProvider;

        public ShapeXmlHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            VisioApplication = new InvisibleApp();
            VisioDocument = VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Diagrams\ShapeXmlHelperTest.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            VisioDocument.Close();
            VisioApplication.Quit();
        }
    }
}
