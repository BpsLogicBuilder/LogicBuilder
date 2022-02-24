using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
        public void CreateFormattedXmlWriterReturnsExpectedSettings()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var writer = helper.CreateFormattedXmlWriter(new StringBuilder());

            //assert
            Assert.True(writer.Settings.Indent);
            Assert.Equal("\t", writer.Settings.IndentChars);
            Assert.True(writer.Settings.OmitXmlDeclaration);
        }

        [Fact]
        public void CreateFormattedXmlWriterWithDeclarationReturnsExpectedSettings()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var writer = helper.CreateFormattedXmlWriterWithDeclaration(new StringBuilder());

            //assert
            Assert.True(writer.Settings.Indent);
            Assert.Equal("\t", writer.Settings.IndentChars);
            Assert.False(writer.Settings.OmitXmlDeclaration);
        }

        [Fact]
        public void CreateUnformattedXmlWriterReturnsExpectedSettings()
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

        [Fact]
        public void GetSiblingParameterElementsWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlConstructorElement = GetXmlElement(@"<constructor name=""Grid.Pageable"">
			                                                <typeName>LogicBuilder.Forms.Parameters.Grid.Pageable</typeName>
			                                                <parameters>
				                                                <literalParameter name=""Refresh"" >
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
				                                                </literalParameter>
				                                                <literalListParameter name=""Page Sizes"" >
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
				                                                </literalListParameter>
				                                                <literalParameter name=""Button Count"" >
					                                                <literalType>Boolean</literalType>
					                                                <control>SingleLineTextBox</control>
					                                                <optional>false</optional>
					                                                <useForEquality>true</useForEquality>
					                                                <useForHashCode>false</useForHashCode>
					                                                <useForToString>true</useForToString>
					                                                <propertySource />
					                                                <propertySourceParameter />
					                                                <defaultValue>5</defaultValue>
					                                                <domain>
						                                                <item>true</item>
						                                                <item>false</item>
					                                                </domain>
					                                                <comments></comments>
				                                                </literalParameter>
			                                                </parameters>
			                                                <genericArguments />
			                                                <summary></summary>
		                                                </constructor>");

            XmlElement parametersElement = helper.GetSingleChildElement(xmlConstructorElement, e => e.Name == XmlDataConstants.PARAMETERSELEMENT);
            XmlElement refreshElement = helper.GetSingleChildElement
            (
                parametersElement, 
                e => e.Name == XmlDataConstants.LITERALPARAMETERELEMENT 
                    && e.Attributes[XmlDataConstants.NAMEATTRIBUTE].Value == "Refresh"
            );

            //act
            var result = helper.GetSiblingParameterElements(refreshElement, xmlConstructorElement);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Page Sizes", result.First().Attributes[XmlDataConstants.NAMEATTRIBUTE].Value);
            Assert.Equal("Button Count", result.Last().Attributes[XmlDataConstants.NAMEATTRIBUTE].Value);
        }

        [Fact]
        public void MakeAttributeWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<root name=""Refresh"">
					                                        <comments>Comment</comments>
				                                        </root>");

            //act
            var result = helper.MakeAttribute(xmlDocument, "id", "2");

            //assert
            Assert.Same(xmlDocument, result.OwnerDocument);
        }

        [Fact]
        public void MakeElementWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<root name=""Refresh"">
					                                        <comments>Comment</comments>
				                                        </root>");

            //act
            var result = helper.MakeElement(xmlDocument, "id", "2");

            //assert
            Assert.Same(xmlDocument, result.OwnerDocument);
        }

        [Fact]
        public void MakeFragmentWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<root name=""Refresh"">
					                                        <comments>Comment</comments>
				                                        </root>");

            //act
            var result = helper.MakeFragment(xmlDocument);

            //assert
            Assert.Same(xmlDocument, result.OwnerDocument);
        }

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
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
