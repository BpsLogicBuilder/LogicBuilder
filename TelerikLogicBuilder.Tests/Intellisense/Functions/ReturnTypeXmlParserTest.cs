using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class ReturnTypeXmlParserTest
    {
        public ReturnTypeXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void GetLiteralReturnTypeFromXml()
        {
            //arrange
            IReturnTypeXmlParser ReturnTypeXmlParser = serviceProvider.GetRequiredService<IReturnTypeXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literal>
                                                      <literalType>Void</literalType>
                                                    </literal>");

            //act
            LiteralReturnType result = (LiteralReturnType)ReturnTypeXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal(LiteralFunctionReturnType.Void, result.ReturnType);
            Assert.Equal(ReturnTypeCategory.Literal, result.ReturnTypeCategory);
        }

        [Fact]
        public void GetObjectReturnTypeFromXml()
        {
            //arrange
            IReturnTypeXmlParser ReturnTypeXmlParser = serviceProvider.GetRequiredService<IReturnTypeXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<object>
					                                    <objectType>System.Object</objectType>
				                                    </object>");

            //act
            ObjectReturnType result = (ObjectReturnType)ReturnTypeXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("System.Object", result.ObjectType);
            Assert.Equal(ReturnTypeCategory.Object, result.ReturnTypeCategory);
        }

        [Fact]
        public void GetGenericReturnTypeFromXml()
        {
            //arrange
            IReturnTypeXmlParser ReturnTypeXmlParser = serviceProvider.GetRequiredService<IReturnTypeXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<generic>
					                                    <genericArgumentName>T</genericArgumentName>
				                                    </generic>");

            //act
            GenericReturnType result = (GenericReturnType)ReturnTypeXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("T", result.GenericArgumentName);
            Assert.Equal(ReturnTypeCategory.Generic, result.ReturnTypeCategory);
        }

        [Fact]
        public void GetLiteralListReturnTypeFromXml()
        {
            //arrange
            IReturnTypeXmlParser ReturnTypeXmlParser = serviceProvider.GetRequiredService<IReturnTypeXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalList>
					                                    <literalType>String</literalType>
					                                    <listType>GenericList</listType>
				                                    </literalList>");

            //act
            ListOfLiteralsReturnType result = (ListOfLiteralsReturnType)ReturnTypeXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal(LiteralFunctionReturnType.String, result.UnderlyingLiteralType);
            Assert.Equal(ListType.GenericList, result.ListType);
            Assert.Equal(ReturnTypeCategory.LiteralList, result.ReturnTypeCategory);
        }

        [Fact]
        public void GetObjectListReturnTypeFromXml()
        {
            //arrange
            IReturnTypeXmlParser ReturnTypeXmlParser = serviceProvider.GetRequiredService<IReturnTypeXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectList>
					                                    <objectType>System.Object</objectType>
					                                    <listType>Array</listType>
				                                    </objectList>");

            //act
            ListOfObjectsReturnType result = (ListOfObjectsReturnType)ReturnTypeXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("System.Object", result.ObjectType);
            Assert.Equal(ListType.Array, result.ListType);
            Assert.Equal(ReturnTypeCategory.ObjectList, result.ReturnTypeCategory);
        }

        [Fact]
        public void GetGenericListReturnTypeFromXml()
        {
            //arrange
            IReturnTypeXmlParser ReturnTypeXmlParser = serviceProvider.GetRequiredService<IReturnTypeXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<genericList>
					                                    <genericArgumentName>T</genericArgumentName>
					                                    <listType>GenericList</listType>
				                                    </genericList>");

            //act
            ListOfGenericsReturnType result = (ListOfGenericsReturnType)ReturnTypeXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("T", result.GenericArgumentName);
            Assert.Equal(ListType.GenericList, result.ListType);
            Assert.Equal(ReturnTypeCategory.GenericList, result.ReturnTypeCategory);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        private static XmlElement GetXmlElement(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument.DocumentElement;
        }
    }
}
