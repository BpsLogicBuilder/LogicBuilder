using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

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
            Assert.Equal(xmlElement.Name, XmlDataConstants.FUNCTIONSELEMENT);
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
            Assert.Equal(xmlElement.Name, XmlDataConstants.FUNCTIONSELEMENT);
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

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(Directory.GetCurrentDirectory(), @$"Tables\{nameof(CellXmlHelperTest)}\{fileNameNoExtension}.tbl");
    }
}
