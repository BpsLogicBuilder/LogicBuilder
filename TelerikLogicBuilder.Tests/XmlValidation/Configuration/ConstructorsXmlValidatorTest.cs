using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlValidation.Configuration
{
    public class ConstructorsXmlValidatorTest
    {
        public ConstructorsXmlValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        private const string CONSTRUCTORNAME = "constructorName";
        #endregion Fields

        [Fact]
        public void ValidateValidXmlWorks()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();

            //act
            var result = xmlValidator.Validate(GetXmlString());

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidateReturnsFailureResponseForInvalidStructure()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();

            //act
            var result = xmlValidator.Validate(@"<folder name=""variables"">
                                                    <undefinedVariable>
		                                            </undefinedVariable>
                                                </folder>");
            Assert.False(result.Success);
        }

        [Fact]
        public void ValidateThrowsXmlExceptionForInvalidXml()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();

            //act
            Assert.Throws<XmlException>(() => xmlValidator.Validate(@"<folder1 name=""variables"">
                                                                    </folder>"));
        }

        [Fact]
        public void InvalidParameterOrderReturnsFailureResponse()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<literalParameter name=""value1"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>true</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.invalidConsParameterOrder,
                    CONSTRUCTORNAME
                ),
                result.Errors.First()
            );
        }

        [Fact]
        public void InvalidTypeNameReturnsFailureResponse()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.TYPENAMEELEMENT] = "2Pac"
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidConstructorFormat, CONSTRUCTORNAME),
                result.Errors.First()
            );
        }

        [Fact]
        public void MissingParameterSourcedPropertyForLiteralReturnsFailureResponse()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<literalParameter name=""value1"" >
                                            <literalType>String</literalType>
                                            <control>ParameterSourcedPropertyInput</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter>value3</propertySourceParameter>
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";
            var siblingNames = new List<string> { "value2" };

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.False(result.Success);
            Assert.True
            (
                result.Errors.Contains
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.cannotLoadPropertySourceParameterFormat,
                        "value3",
                        "value1",
                        CONSTRUCTORNAME,
                        string.Join(", ", siblingNames),
                        Strings.enumDescriptionParameterSourcedPropertyInput
                    )
                )
            );
        }

        [Fact]
        public void ValidParameterSourcedPropertyForLiteralReturnsSuccess()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<literalParameter name=""value1"" >
                                            <literalType>String</literalType>
                                            <control>ParameterSourcedPropertyInput</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter>value2</propertySourceParameter>
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void MissingParameterSourcedPropertyForLiteralListReturnsFailureResponse()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<literalListParameter name=""value1"">
					                        <literalType>String</literalType>
					                        <listType>GenericList</listType>
					                        <control>HashSetForm</control>
					                        <elementControl>ParameterSourcedPropertyInput</elementControl>
					                        <optional>false</optional>
					                        <propertySource />
					                        <propertySourceParameter>value3</propertySourceParameter>
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
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";
            var siblingNames = new List<string> { "value2" };

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.False(result.Success);
            Assert.True
            (
                result.Errors.Contains
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.cannotLoadPropertySourceParameterFormat,
                        "value3",
                        "value1",
                        CONSTRUCTORNAME,
                        string.Join(", ", siblingNames),
                        Strings.enumDescriptionParameterSourcedPropertyInput
                    )
                )
            );
        }

        [Fact]
        public void ValidParameterSourcedPropertyForLiteralListReturnsSuccess()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<literalListParameter name=""value1"">
					                        <literalType>String</literalType>
					                        <listType>GenericList</listType>
					                        <control>HashSetForm</control>
					                        <elementControl>ParameterSourcedPropertyInput</elementControl>
					                        <optional>false</optional>
					                        <propertySource />
					                        <propertySourceParameter>value2</propertySourceParameter>
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
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidGenericParameterReturnsSuccess()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<genericParameter name=""value1"" >
                                            <genericArgumentName>A</genericArgumentName>
                                            <optional>false</optional>
                                            <comments />
                                        </genericParameter>
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";
            XmlNode genericArgumentsNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.GENERICARGUMENTSELEMENT}");
            genericArgumentsNode.InnerXml = @"<item>A</item>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidGenericListParameterReturnsSuccess()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<genericListParameter name=""value1"" >
                                            <genericArgumentName>A</genericArgumentName>
                                            <listType>GenericList</listType>
                                            <control>HashSetForm</control>
                                            <optional>false</optional>
                                            <comments />
                                        </genericListParameter>
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";
            XmlNode genericArgumentsNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.GENERICARGUMENTSELEMENT}");
            genericArgumentsNode.InnerXml = @"<item>A</item>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void InvalidGenericArgumentReturnsFailure()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode genericArgumentsNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.GENERICARGUMENTSELEMENT}");
            genericArgumentsNode.InnerXml = @"<item>?A</item>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.False(result.Success);
            Assert.True
            (
                result.Errors.Contains
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.constrGenericArgNameInvalidFormat,
                        "?A",
                        CONSTRUCTORNAME
                    )
                )
            );
        }

        [Fact]
        public void UnknownGenericArgumentNameForParametersReturnsFailureResponse()
        {
            //arrange
            IConstructorsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorsXmlValidator>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode genericArgumentsNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.GENERICARGUMENTSELEMENT}");
            genericArgumentsNode.InnerXml = @"<item>A</item>";
            XmlNode parametersNode = xmlDocumentHelpers.SelectSingleElement(document, $"//{XmlDataConstants.PARAMETERSELEMENT}");
            parametersNode.InnerXml = @"<genericParameter name=""value1"" >
                                            <genericArgumentName>B</genericArgumentName>
                                            <optional>false</optional>
                                            <comments />
                                        </genericParameter>
                                        <literalParameter name=""value2"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>";
            var genericArguments = new List<string> { "A" };

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.genericParameterArgNameNotFoundFormat,
                    "B",
                    "value1",
                    CONSTRUCTORNAME,
                    string.Join(", ", genericArguments)
                ),
                result.Errors.First()
            );
        }

        private static string GetXmlString(IDictionary<string, string>? valuesToSet = null)
        {
            string xmlString = $@"<form>
                                    <folder name=""Constructors"">
                                        <constructor name=""{CONSTRUCTORNAME}"" >
                                            <typeName>LogicBuilder.Forms.Parameters.Grid.Sort</typeName>
                                            <parameters>
                                                <literalParameter name=""value"" >
                                                    <literalType>String</literalType>
                                                    <control>SingleLineTextBox</control>
                                                    <optional>false</optional>
                                                    <useForEquality>true</useForEquality>
                                                    <useForHashCode>false</useForHashCode>
                                                    <useForToString>true</useForToString>
                                                    <propertySource />
                                                    <propertySourceParameter />
                                                    <defaultValue />
                                                    <domain />
                                                    <comments />
                                                </literalParameter>
                                            </parameters>
                                            <genericArguments />
                                            <summary>Updates the access the access after field.</summary>
                                        </constructor>
                                    </folder>
                                </form>";

            if (valuesToSet == null)
                return xmlString;

            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);

            foreach (KeyValuePair<string, string> kvp in valuesToSet)
            {
                XmlNode xmlNode = xmlDocument.SelectSingleNode($"//{kvp.Key}")!;
                xmlNode.InnerText = kvp.Value;
            };

            return xmlDocument.DocumentElement!.OuterXml;
        }
    }
}
