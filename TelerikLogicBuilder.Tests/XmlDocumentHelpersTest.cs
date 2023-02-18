using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class XmlDocumentHelpersTest
    {
        public XmlDocumentHelpersTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateFormattedXmlWriterReturnsExpectedSettings()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var writer = helper.CreateFormattedXmlWriter(new StringBuilder());

            //assert
            Assert.True(writer.Settings!.Indent);
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
            Assert.True(writer.Settings!.Indent);
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
            Assert.False(writer.Settings!.Indent);
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
        public void GetChildElementsWithFilterWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<connector name=""1"" connectorCategory=""1"">
                                                      <text>
                                                        FFF
                                                        <function name=""get message"" visibleText=""visibleText"">
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
                                                    </connector>");

            //act
            var result = helper.GetChildElements
            (
                xmlElement, 
                element => element.Name == XmlDataConstants.METAOBJECTELEMENT
                        && element.ParentNode is XmlElement parentElement
                        && parentElement.Name == XmlDataConstants.CONNECTORELEMENT
                        && parentElement.GetAttribute(XmlDataConstants.CONNECTORCATEGORYATTRIBUTE) == ((int)ConnectorCategory.Dialog).ToString(CultureInfo.InvariantCulture)
            );

            //assert
            Assert.Single(result);
        }

        [Fact]
        public void SelectingMetaObjectElementFromConnectorElementWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<connector name=""1"" connectorCategory=""1"">
                                                          <text>
                                                            FFF
                                                            <function name=""get message"" visibleText=""visibleText"">
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
                                                        </connector>");

            //act
            var result = helper
                .SelectElements(xmlDocument, $"//{XmlDataConstants.CONNECTORELEMENT}[@{XmlDataConstants.CONNECTORCATEGORYATTRIBUTE}={((int)ConnectorCategory.Dialog).ToString(CultureInfo.InvariantCulture)}]")
                .Select(element => helper.GetSingleChildElement(element, e => e.Name == XmlDataConstants.METAOBJECTELEMENT))
                .Select(element => element.Attributes[XmlDataConstants.OBJECTTYPEATTRIBUTE])
                .Where
                (
                    attributeValue => true
                )
                .ToList();


            //assert
            Assert.Single(result);
        }

        [Fact]
        public void GetSingleChildElementWorks()
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
        public void GetSingleOrDefaultChildElementWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //act
            var result = helper.GetSingleOrDefaultChildElement(xmlElement);

            //assert
            Assert.Equal("genericArgumentName", result?.Name);
        }

        [Fact]
        public void GetSingleOrDefaultChildElementReturnsNullIfNoChildElements()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<genericParameter name=""Refresh"">
				</genericParameter>");

            //act
            var result = helper.GetSingleOrDefaultChildElement(xmlElement);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public void GetDocumentElementsWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //act
            var result = helper.GetDocumentElement(xmlDocument);

            //assert
            Assert.Equal("genericParameter", result.Name);
        }

        [Fact]
        public void GetDocumentElementsThrowsCriticalExceptionIfDocumentElementIsNull()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = new();

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => helper.GetDocumentElement(xmlDocument));
        }

        [Fact]
        public void GetGenericArgumentsWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<constructor name=""Grid.Pageable"">
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
			                                                <genericArguments>
						                                        <item>A</item>
						                                        <item>B</item>
                                                            </genericArguments>
			                                                <summary></summary>
		                                                </constructor>");

            //act
            var result = helper.GetGenericArguments(xmlDocument, "/constructor/genericArguments");

            //assert
            Assert.Equal(2, result.Length);
            Assert.Equal("A", result[0]);
            Assert.Equal("B", result[1]);
        }

        [Fact]
        public void GetParameterElementsWorks()
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

            //act
            var result = helper.GetParameterElements(xmlConstructorElement);

            //assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Refresh", result[0].GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("Page Sizes", result[1].GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("Button Count", result[2].GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
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
                    && e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE) == "Refresh"
            );

            //act
            var result = helper.GetSiblingParameterElements(refreshElement);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Page Sizes", result.First().GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("Button Count", result.Last().GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
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

        [Fact]
        public void GetUnformattedStringWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //act
            var result = helper.GetUnformattedXmlString(xmlDocument);

            //assert
            Assert.Single(result.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }


        [Fact]
        public void GetXmlStringWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //act
            var result = helper.GetXmlString(xmlDocument);

            //assert
            Assert.Equal(3, result.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length);
        }

        public static IList<object[]> MixedXml_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { @"<literalParameter name=""p1""><variable name=""myvariable"" visibleText=""myvariable"" /></literalParameter>", "<myvariable>" },
                    new object[] { @"<text name=""p1""><function name=""functionName"" visibleText=""functionVisibleText""></function></text>", "functionVisibleText" },
                    new object[] { @"<text name=""p1""><constructor name=""constructorName"" visibleText=""constructorVisibleText""></constructor></text>", "constructorVisibleText" },
                    new object[] { @"<text name=""p1"">SomeText</text>", "SomeText" },
                    new object[] { @"<text name=""p1"">Text Mixed <variable name=""myvariable"" visibleText=""myvariable"" /> With Element</text>", @"Text Mixed <myvariable> With Element" }
                };
            }
        }

        [Theory]
        [MemberData(nameof(MixedXml_Data))]
        public void GetVisibleTextWorks(string xml, string visibleText)
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(xml);

            //act
            var result = helper.GetVisibleText(xmlElement);

            //assert
            Assert.Equal(visibleText, result);
        }

        [Fact]
        public void GetVisibleTextThrowsForInvalidChildElement()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<text name=""p1"">Text Mixed <garbage name=""garbage"" visibleText=""garbage"" /> With Element</text>");

            //act
            var result = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetVisibleText(xmlElement));

            //assert
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.InvariantCulture,
                    Strings.invalidArgumentTextFormat, 
                    "{51C90B4E-2ABE-4381-817E-87EA1D72F684}"
                ), 
                result.Message
            );
        }

        [Fact]
        public void SelectElementsWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<constructor name=""Grid.Pageable"">
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

            //act
            var result = helper.SelectElements(xmlDocument, "//literalParameter");

            //assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void SelectElementsReturnsEmptyListWhenDocumentElementIsNull()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = new();

            //act
            var result = helper.SelectElements(xmlDocument, "//literalParameter");

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public void SelectSingleElementWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = GetXmlDocument(@"<constructor name=""Grid.Pageable"">
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

            //act
            var result = helper.SelectSingleElement(xmlDocument, "//literalParameter");

            //assert
            Assert.True(result is not null);
        }

        [Fact]
        public void SelectSingleElementThrowsIfElementNotFound()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument xmlDocument = new();

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => helper.SelectSingleElement(xmlDocument, "//literalParameter"));
        }

        [Fact]
        public void ToXmlDocumentWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement xmlElement = GetXmlElement(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //act
            var result = helper.ToXmlDocument(xmlElement);

            //assert
            Assert.Equal("genericParameter", result.DocumentElement!.Name);
        }

        [Fact]
        public void ToXmlDocumentFromStringWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var result = helper.ToXmlDocument(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //assert
            Assert.Equal("genericParameter", result.DocumentElement!.Name);
        }

        [Fact]
        public void ToXmlElementFromStringWorks()
        {
            //arrange
            IXmlDocumentHelpers helper = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            var result = helper.ToXmlElement(@"<genericParameter name=""Refresh"">
					<genericArgumentName>T</genericArgumentName>
				</genericParameter>");

            //assert
            Assert.Equal("genericParameter", result.Name);
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
