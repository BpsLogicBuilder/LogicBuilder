using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Xunit;
using static TelerikLogicBuilder.Tests.CellXmlHelperTest;

namespace TelerikLogicBuilder.Tests
{
    public class CellXmlHelperTest
    {
        public CellXmlHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateCellXmlHelper()
        {
            //arrange
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void GetXmlStringGivenColumnIndexSucceeds()
        {
            //arrange
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();
            string sourceFile = GetFullSourceFilePath(nameof(GetXmlStringGivenColumnIndexSucceeds));
            DataSet dataSet = GetDataSet(sourceFile);

            //act
            XmlElement xmlElement = GetXmlElement
            (
                helper.GetXmlString
                (
                    (string)dataSet.Tables[TableName.RULESTABLE]!.Rows[0].ItemArray.GetValue(TableColumns.ACTIONCOLUMNINDEX)!, 
                    TableColumns.ACTIONCOLUMNINDEX
                )
            );

            dataSet.Dispose();

            //assert
            Assert.Equal(XmlDataConstants.FUNCTIONSELEMENT, xmlElement.Name);
        }

        [Fact]
        public void GetXmlStringGivenSchemaNameSucceeds()
        {
            //arrange
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();
            string sourceFile = GetFullSourceFilePath(nameof(GetXmlStringGivenSchemaNameSucceeds));
            DataSet dataSet = GetDataSet(sourceFile);

            //act
            XmlElement xmlElement = GetXmlElement
            (
                helper.GetXmlString
                (
                    (string)dataSet.Tables[TableName.RULESTABLE]!.Rows[0].ItemArray.GetValue(TableColumns.ACTIONCOLUMNINDEX)!,
                    TableColumns.ACTIONCOLUMNINDEX
                )
            );

            dataSet.Dispose();

            //assert
            Assert.Equal(XmlDataConstants.FUNCTIONSELEMENT, xmlElement.Name);
        }

        public class CellXmlTheoryData(int columnIndex, string xmlToSave, string visibleText)
        {
            public int ColumnIndex { get; } = columnIndex;
            public string XmlToSave { get; } = xmlToSave;
            public string VisibleText { get; } = visibleText;
        }

        public static TheoryData<CellXmlTheoryData> CellXml_Data
        {
            get
            {
                return
                [
                    new CellXmlTheoryData
                    (
                        TableColumns.ACTIONCOLUMNINDEX,
                        $@"<functions>
                            <assertFunction name=""Set Variable"" visibleText=""visibleText"">
                                <variable name=""{Enum.GetName(LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                <variableValue>
                                    <literalVariable>CB</literalVariable>
                                </variableValue>
                            </assertFunction>
                            <retractFunction name=""Set To Null"" visibleText=""visibleText"">
                                <variable name=""{Enum.GetName(LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                            </retractFunction>
                            <function name=""StaticVoidMethod"" visibleText=""visibleText"">
                                <genericArguments />
                                <parameters>
                                    <literalParameter name=""arg1"">
                                        <variable name=""{Enum.GetName(LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                    </literalParameter>
                                    <literalParameter name=""arg2"">SomeString</literalParameter>
                                </parameters>
                            </function>
                            <function name=""WriteToLog"" visibleText=""visibleText"">
                                <genericArguments />
                                <parameters>
                                    <literalParameter name=""message"">
                                        <variable name=""{Enum.GetName(LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                    </literalParameter>
                                </parameters>
                            </function>
                        </functions>",
                        "Some Functions"
                    ),
                    new CellXmlTheoryData
                    (
                        TableColumns.CONDITIONCOLUMNINDEX,
                        $@"<conditions>
                            <and>
                                <not>
                                    <function name=""Greater Than"" visibleText=""visibleText"">
                                    <genericArguments />
                                    <parameters>
                                        <literalParameter name=""val1"">
                                        <variable name=""{Enum.GetName(LiteralVariableType.Integer)}Item"" visibleText=""visibleText"" />
                                        </literalParameter>
                                        <literalParameter name=""val2"">0</literalParameter>
                                    </parameters>
                                    </function>
                                </not>
                                <function name=""Equals"" visibleText=""visibleText"">
                                    <genericArguments />
                                    <parameters>
                                        <literalParameter name=""val1"">
                                            <variable name=""{Enum.GetName(LiteralVariableType.String)}Item"" visibleText=""visibleText"" />
                                        </literalParameter>
                                        <literalParameter name=""val2"">SomeString</literalParameter>
                                    </parameters>
                                </function>
                            </and>
                        </conditions>",
                        "Comditions"
                    ),
                    new CellXmlTheoryData
                    (
                        TableColumns.PRIORITYCOLUMNINDEX,
                        $@"<shapeData name=""Priority"">
                                <value>Some Value</value>
                            </shapeData>",
                        "10"
                    )
                ];
            }
        }

        [Theory]
        [MemberData(nameof(CellXml_Data), MemberType = typeof(CellXmlHelperTest))]
        public void CanSaveXmlToAllCells(CellXmlTheoryData cellXmlTheoryData)
        {
            //arrange
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();
            string sourceFile = GetFullSourceFilePath(nameof(CanSaveXmlToAllCells));
            DataSet dataSet = GetDataSet(sourceFile);

            //act
            helper.SetXmlString(dataSet, 0, cellXmlTheoryData.ColumnIndex, cellXmlTheoryData.XmlToSave, cellXmlTheoryData.VisibleText);
            string savedString = helper.GetXmlString
            (
                (string)dataSet.Tables[TableName.RULESTABLE]!.Rows[0].ItemArray.GetValue(cellXmlTheoryData.ColumnIndex)!,
                cellXmlTheoryData.ColumnIndex
            );

            //assert
            Assert.Equal(cellXmlTheoryData.XmlToSave, savedString);
        }

        [Fact]
        public void UpdateThrowsCriticalExceptionForInvalidXml()
        {
            //arrange
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();
            DataSet dataSet = GetEmptyDataset();

            //act assert
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.SetXmlString(dataSet, 0, TableColumns.ACTIONCOLUMNINDEX, "<someElement>FFF</mismatchedClosingTag>", "visibleText"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, ABIS.LogicBuilder.FlowBuilder.Strings.invalidArgumentTextFormat, "{21C80621-8E80-434A-B2C1-3E75E6528122}"), exception.Message);
        }

        [Fact]
        public void UpdateThrowsCriticalExceptionForInvalidColumn()
        {
            //arrange
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();
            DataSet dataSet = GetEmptyDataset();

            //act assert
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.SetXmlString(dataSet, 0, TableColumns.ACTIVECOLUMNINDEX, "<text>FFF</text>", "visibleText"));
            Assert.Equal(string.Format(CultureInfo.InvariantCulture, ABIS.LogicBuilder.FlowBuilder.Strings.invalidArgumentTextFormat, "{7599F0A3-01A7-434B-ADA0-6CA33020E5C0}"), exception.Message);
        }

        [Fact]
        public void SetXmlStringSavesXmlInUnencryptedForm()
        {
            //arrange
            string xmlToSave = $@"<shapeData name=""Priotity"">
                                <value>7</value>
                            </shapeData>";
            string visibleText = "Some Value";
            ICellXmlHelper helper = serviceProvider.GetRequiredService<ICellXmlHelper>();
            string sourceFile = GetFullSourceFilePath(nameof(SetXmlStringSavesXmlInUnencryptedForm));
            DataSet dataSet = GetDataSet(sourceFile);

            //act
            helper.SetXmlString(dataSet, 0, TableColumns.PRIORITYCOLUMNINDEX, xmlToSave, visibleText);
            string savedStringFromCell = (string)dataSet.Tables[TableName.RULESTABLE]!.Rows[0].ItemArray.GetValue(TableColumns.PRIORITYCOLUMNINDEX)!;
            string savedString = helper.GetXmlString
            (
                savedStringFromCell,
                TableColumns.PRIORITYCOLUMNINDEX
            );

            XmlElement xmlElement1 = GetXmlElement(savedString);
            XmlElement xmlElement2 = GetXmlElement(savedStringFromCell);

            //assert
            Assert.Equal("shapeData", xmlElement1.Name);
            Assert.Equal("shapeData", xmlElement2.Name);
        }

        private static XmlElement GetXmlElement(string xmlString)
            => GetXmlDocument(xmlString).DocumentElement!;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }

        private static DataSet GetDataSet(string sourceFullPath)
        {
            DataSet dataSet = new()
            {
                Locale = CultureInfo.InvariantCulture
            };

            using (StringReader stringReader = new(Schemas.GetSchema(Schemas.TableSchema)))
            {
                dataSet.ReadXmlSchema(stringReader);
                stringReader.Close();
            }

            using FileStream fileStream = new(sourceFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            dataSet.ReadXml(fileStream);
            return dataSet;
        }

        private DataSet GetEmptyDataset()
        {
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            StringBuilder stringBuilder = new();
            using XmlWriter xmlTextWriter = xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder);
            DataSet dataSet = new(XmlDataConstants.TABLESELEMENT)
            {
                Locale = CultureInfo.InvariantCulture
            };

            using (StringReader stringReader = new(Schemas.GetSchema(Schemas.TableSchema)))
            {
                dataSet.ReadXmlSchema(stringReader);
                stringReader.Close();
            }

            xmlTextWriter.WriteElementString(XmlDataConstants.TABLESELEMENT, string.Empty);

            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            XmlTextReader xmlReader = new(stringBuilder.ToString(), XmlNodeType.Element, null);
            dataSet.ReadXml(xmlReader);
            return dataSet;
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(Directory.GetCurrentDirectory(), @$"Tables\{nameof(CellXmlHelperTest)}\{fileNameNoExtension}.tbl");
    }
}
