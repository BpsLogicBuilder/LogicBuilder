using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Parameters
{
    public class ParametersXmlParserTest
    {
        public ParametersXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetLiteralParameterFromXml()
        {
            //arrange
            IParametersXmlParser parametersXmlParser = serviceProvider.GetRequiredService<IParametersXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalParameter name=""Refresh"">
					<literalType>Boolean</literalType>
					<control>SingleLineTextBox</control>
					<optional>false</optional>
					<useForEquality>true</useForEquality>
					<useForHashCode>true</useForHashCode>
					<useForToString>true</useForToString>
					<propertySource />
					<propertySourceParameter />
					<defaultValue>true</defaultValue>
					<domain>
						<item>true</item>
						<item>false</item>
					</domain>
					<comments></comments>
				</literalParameter>");

            //act
            LiteralParameter result = (LiteralParameter)parametersXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Refresh", result.Name);
            Assert.Equal(2, result.Domain.Count);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetObjectParameterFromXml()
        {
            //arrange
            IParametersXmlParser parametersXmlParser = serviceProvider.GetRequiredService<IParametersXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectParameter name=""Refresh"">
					<objectType>System.Object</objectType>
					<optional>false</optional>
					<useForEquality>true</useForEquality>
					<useForHashCode>true</useForHashCode>
					<useForToString>true</useForToString>
					<comments>Comment</comments>
				</objectParameter>");

            //act
            ObjectParameter result = (ObjectParameter)parametersXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Refresh", result.Name);
            Assert.Equal("System.Object", result.ObjectType);
            Assert.True(result.UseForEquality);
            Assert.Equal("Comment", result.Comments);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetGenericParameterFromXml()
        {
            //arrange
            IParametersXmlParser parametersXmlParser = serviceProvider.GetRequiredService<IParametersXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
					<optional>true</optional>
					<comments>Comment</comments>
				</genericParameter>");

            //act
            GenericParameter result = (GenericParameter)parametersXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Refresh", result.Name);
            Assert.Equal("T", result.GenericArgumentName);
            Assert.True(result.IsOptional);
            Assert.Equal("Comment", result.Comments);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetLiteralListParameterFromXml()
        {
            //arrange
            IParametersXmlParser parametersXmlParser = serviceProvider.GetRequiredService<IParametersXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<literalListParameter name=""Page Sizes"">
					<literalType>String</literalType>
					<listType>GenericList</listType>
					<control>HashSetForm</control>
					<elementControl>SingleLineTextBox</elementControl>
					<optional>false</optional>
					<propertySource />
					<propertySourceParameter />
					<defaultValue>
						<item>hi</item>
						<item>medium</item>
					</defaultValue>
					<domain>
						<item>hi</item>
						<item>medium</item>
						<item>lo</item>
					</domain>
					<comments></comments>
				</literalListParameter>");

            //act
            ListOfLiteralsParameter result = (ListOfLiteralsParameter)parametersXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Page Sizes", result.Name);
            Assert.Equal(LiteralParameterInputStyle.SingleLineTextBox, result.ElementControl);
            Assert.Equal(ListParameterInputStyle.HashSetForm, result.Control);
            Assert.Equal(3, result.Domain.Count);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetObjectListParameterFromXml()
        {
            //arrange
            IParametersXmlParser parametersXmlParser = serviceProvider.GetRequiredService<IParametersXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<objectListParameter name=""Page Sizes"">
					<objectType>System.Object</objectType>
					<listType>GenericList</listType>
					<control>HashSetForm</control>
					<optional>false</optional>
					<comments>A Comment</comments>
				</objectListParameter>");

            //act
            ListOfObjectsParameter result = (ListOfObjectsParameter)parametersXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Page Sizes", result.Name);
            Assert.Equal("System.Object", result.ObjectType);
            Assert.Equal(ListParameterInputStyle.HashSetForm, result.Control);
            Assert.Equal("A Comment", result.Comments);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetGenericListParameterFromXml()
        {
            //arrange
            IParametersXmlParser parametersXmlParser = serviceProvider.GetRequiredService<IParametersXmlParser>();
            XmlElement xmlElement = GetXmlElement(@"<genericListParameter name=""Page Sizes"">
					<genericArgumentName>T</genericArgumentName>
					<listType>GenericList</listType>
					<control>HashSetForm</control>
					<optional>false</optional>
					<comments>A Comment</comments>
				</genericListParameter>");

            //act
            ListOfGenericsParameter result = (ListOfGenericsParameter)parametersXmlParser.Parse(xmlElement);

            //assert
            Assert.Equal("Page Sizes", result.Name);
            Assert.Equal("T", result.GenericArgumentName);
            Assert.Equal(ListParameterInputStyle.HashSetForm, result.Control);
            Assert.Equal("A Comment", result.Comments);
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
