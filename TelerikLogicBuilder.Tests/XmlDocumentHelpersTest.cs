using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class XmlDocumentHelpersTest
    {
        public XmlDocumentHelpersTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CreateUnformattedXmlWritertReturnsExpectedSettings()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var writer = helper.CreateUnformattedXmlWriter(new StringBuilder());

            //assert
            Assert.False(writer.Settings.Indent);
            Assert.True(writer.Settings.OmitXmlDeclaration);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetChildElementsWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
					<optional>true</optional>
					<comments>Comment</comments>
				</genericParameter>");

            //act
            var result = helper.GetChildElements(xmlElement);

            //assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetSingleChildElementsWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //act
            var result = helper.GetSingleChildElement(xmlElement);

            //assert
            Assert.Equal("genericArgumentName", result.Name);
        }

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        private static XmlElement GetXmlElement(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument.DocumentElement;
        }
    }
}
