using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlValidation.Configuration
{
    public class FunctionsXmlValidatorTest
    {
        public FunctionsXmlValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        private const string FUNCTIONNAME = "functionName";
        #endregion Fields

        [Fact]
        public void ValidateValidXmlWorks()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();

            //act
            var result = xmlValidator.Validate(GetXmlString());

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidateThrowsXmlExceptionForInvalidXml()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();

            //act
            Assert.Throws<XmlException>(() => xmlValidator.Validate(@"<folder1 name=""variables"">
                                                                    </folder>"));
        }

        [Fact]
        public void ValidateReturnsFailureResponseForInvalidStructure()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();

            //act
            var result = xmlValidator.Validate(@"<folder name=""variables"">
                                                    <undefinedVariable>
		                                            </undefinedVariable>
                                                </folder>");
            Assert.False(result.Success);
        }

        [Fact]
        public void InvalidReferenceDefinitionReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCENAMEELEMENT] = "foo",
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "InvalidReferenceDefinitionName",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = "InstanceReference"
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.functionInvalidIndirectDefinitionFormat,
                    FUNCTIONNAME,
                    "InvalidReferenceDefinitionName",
                    Environment.NewLine,
                    enumHelper.GetValidIndirectReferencesList()
                ),
                result.Errors.First()
            );
        }

        [Fact]
        public void InvalidIndirectReferenceNameReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCENAMEELEMENT] = "foo",
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "IntegerKeyIndexer",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = "InstanceReference"
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
        }

        [Fact]
        public void InvalidParameterOrderReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
                    Strings.invalidParameterOrder,
                    FUNCTIONNAME
                ),
                result.Errors.First()
            );
        }

        [Fact]
        public void InvalidMemberNameReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.MEMBERNAMEELEMENT] = "1foo"
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.memberNameIsInvalidFormat,
                    FUNCTIONNAME
                ),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("Assign")]
        [InlineData("UndefinedOperator")]
        public void InvalidBinaryOperatorReturnsFailureResponse(string operatorName)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.MEMBERNAMEELEMENT] = operatorName,
                [XmlDataConstants.FUNCTIONCATEGORYELEMENT] = "BinaryOperator",
                [XmlDataConstants.REFERENCENAMEELEMENT] = "",
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = "None"
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.binaryOperatorCodeNameInvalidFormat,
                    operatorName,
                    FUNCTIONNAME,
                    string.Join
                    (
                        Strings.itemsCommaSeparator,
                        enumHelper.ConvertEnumListToStringList
                        (
                            new CodeBinaryOperatorType[] { CodeBinaryOperatorType.Assign }
                        )
                    )
                ),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("This")]
        [InlineData("Type")]
        [InlineData("None")]
        public void ReferenceNamePopulatedAndMustBeEmptyReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCENAMEELEMENT] = "foo",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = referenceCategory
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionReferenceMustBeEmptyFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("This")]
        [InlineData("Type")]
        [InlineData("None")]
        public void ReferenceDefinitionPopulatedAndMustBeEmptyReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "Property",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = referenceCategory
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionNameAndDefinitionFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("InstanceReference")]
        [InlineData("StaticReference")]
        public void ReferenceNameEmptyAndMustBePopulatedReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCENAMEELEMENT] = "",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = referenceCategory
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionReferenceInfoMustBePopulatedFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("InstanceReference")]
        [InlineData("StaticReference")]
        public void ReferenceDefinitionEmptyAndMustBePopulatedReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = referenceCategory
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionReferenceInfoMustBePopulatedFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.InstanceReference, "Foo.Bar", "Property")]
        [InlineData(ReferenceCategories.StaticReference, "Foo.Bar", "Property")]
        internal void ReferenceDefinitionLengthsDoesNotMatchReferenceNameLengthReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionNameAndDefinitionFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Fact]
        public void CastReferenceAsLengthsDoesNotMatchReferenceNameLengthReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "Property",
                [XmlDataConstants.REFERENCENAMEELEMENT] = "Foo",
                [XmlDataConstants.CASTREFERENCEASELEMENT] = "A.V",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.InstanceReference)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionNameAndCastAsFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.Type, "", "", "2Pac")]
        [InlineData(ReferenceCategories.StaticReference, "Foo", "Property", "2Pac")]
        internal void InvalidTypeNameReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition, string typeName)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.TYPENAMEELEMENT] = typeName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.InstanceReference, "Foo", "Property", "Pac")]
        [InlineData(ReferenceCategories.This, "", "", "Pac")]
        [InlineData(ReferenceCategories.None, "", "", "Pac")]
        internal void TypeNamePopulatedAndShouldBeEmptyReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition, string typeName)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.TYPENAMEELEMENT] = typeName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.functionTypeNameMustBeEmptyFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Fact]
        public void SequentialParametersLayoutForBinaryOperatorReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "",
                [XmlDataConstants.REFERENCENAMEELEMENT] = "",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.None)!,
                [XmlDataConstants.MEMBERNAMEELEMENT] = "Add",
                [XmlDataConstants.FUNCTIONCATEGORYELEMENT] = "BinaryOperator",
                [XmlDataConstants.PARAMETERSLAYOUTELEMENT] = Enum.GetName(typeof(ParametersLayout), ParametersLayout.Sequential)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.parametersLayoutMustBeBinaryFormat, FUNCTIONNAME),
                result.Errors.First()
            );
        }

        [Fact]
        public void InvalidReferenceCategoryForBinaryOperatorReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "",
                [XmlDataConstants.REFERENCENAMEELEMENT] = "",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.This)!,
                [XmlDataConstants.MEMBERNAMEELEMENT] = "Add",
                [XmlDataConstants.FUNCTIONCATEGORYELEMENT] = "BinaryOperator",
                [XmlDataConstants.PARAMETERSLAYOUTELEMENT] = Enum.GetName(typeof(ParametersLayout), ParametersLayout.Sequential)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.True
            (
                result.Errors.Contains
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.referenceCategoryMustBeNoneFormat, FUNCTIONNAME)
                )
            );
        }

        [Theory]
        [InlineData(FunctionCategories.Standard)]
        [InlineData(FunctionCategories.DialogForm)]
        internal void InvalidReferenceCategoryForFunctionCategoriesReturnsFailureResponse(FunctionCategories functionCategory)
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "",
                [XmlDataConstants.REFERENCENAMEELEMENT] = "",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.None)!,
                [XmlDataConstants.MEMBERNAMEELEMENT] = "Add",
                [XmlDataConstants.FUNCTIONCATEGORYELEMENT] = Enum.GetName(typeof(FunctionCategories), functionCategory)!,
                [XmlDataConstants.PARAMETERSLAYOUTELEMENT] = Enum.GetName(typeof(ParametersLayout), ParametersLayout.Sequential)!
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.True
            (
                result.Errors.Contains
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.referenceCategoryCannotBeNoneFormat, FUNCTIONNAME)
                )
            );
        }

        [Fact]
        public void MissingParameterSourcedPropertyForLiteralReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
                        Strings.funcCannotLoadPropertySourceParameterFormat,
                        "value3",
                        "value1",
                        FUNCTIONNAME,
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
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
                        Strings.funcCannotLoadPropertySourceParameterFormat,
                        "value3",
                        "value1",
                        FUNCTIONNAME,
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
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            XmlDocument document = new();
            document.LoadXml(GetXmlString(null));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
        public void ValidGenericParameterAndReturnTypesReturnsSuccess()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.Type)!,
                [XmlDataConstants.TYPENAMEELEMENT] = "SomeType"
            };
            XmlDocument document = new();
            document.LoadXml(GetXmlString(fieldsToSet));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
            XmlNode genericArgumentsNode = document.SelectSingleNode($"//{XmlDataConstants.GENERICARGUMENTSELEMENT}")!;
            genericArgumentsNode.InnerXml = @"<item>A</item>";
            XmlNode returnTypeNode = document.SelectSingleNode($"//{XmlDataConstants.RETURNTYPEELEMENT}")!;
            returnTypeNode.InnerXml = @"<generic>
						                    <genericArgumentName>A</genericArgumentName>
					                    </generic>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidGenericListParameterAndReturnTypesReturnsSuccess()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.Type)!,
                [XmlDataConstants.TYPENAMEELEMENT] = "SomeType"
            };
            XmlDocument document = new();
            document.LoadXml(GetXmlString(fieldsToSet));
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
            XmlNode genericArgumentsNode = document.SelectSingleNode($"//{XmlDataConstants.GENERICARGUMENTSELEMENT}")!;
            genericArgumentsNode.InnerXml = @"<item>A</item>";
            XmlNode returnTypeNode = document.SelectSingleNode($"//{XmlDataConstants.RETURNTYPEELEMENT}")!;
            returnTypeNode.InnerXml = @"<generic>
						                    <genericArgumentName>A</genericArgumentName>
					                    </generic>";

            //act
            var result = xmlValidator.Validate(document.DocumentElement!.OuterXml);

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void InvalidGenericArgumentReturnsFailure()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.Type)!,
                [XmlDataConstants.TYPENAMEELEMENT] = "SomeType"
            };
            XmlDocument document = new();
            document.LoadXml(GetXmlString(fieldsToSet));
            XmlNode genericArgumentsNode = document.SelectSingleNode($"//{XmlDataConstants.GENERICARGUMENTSELEMENT}")!;
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
                        Strings.funcGenericArgNameInvalidFormat,
                        "?A",
                        FUNCTIONNAME
                    )
                )
            );
        }

        [Fact]
        public void InvalidReferenceCategoryWithGenericTypesReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.This)!
            };

            XmlDocument document = new();
            document.LoadXml(GetXmlString(fieldsToSet));
            XmlNode genericArgumentsNode = document.SelectSingleNode($"//{XmlDataConstants.GENERICARGUMENTSELEMENT}")!;
            genericArgumentsNode.InnerXml = @"<item>A</item><item>B</item>";

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
                        Strings.funcGenericArgNotValidForAllReferenceCatFormat,
                        FUNCTIONNAME
                    )
                )
            );
        }

        [Fact]
        public void UnknownGenericArgumentNameForReturnTypesReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.Type)!,
                [XmlDataConstants.TYPENAMEELEMENT] = "SomeType"
            };
            XmlDocument document = new();
            document.LoadXml(GetXmlString(fieldsToSet));
            XmlNode genericArgumentsNode = document.SelectSingleNode($"//{XmlDataConstants.GENERICARGUMENTSELEMENT}")!;
            genericArgumentsNode.InnerXml = @"<item>A</item>";
            XmlNode returnTypeNode = document.SelectSingleNode($"//{XmlDataConstants.RETURNTYPEELEMENT}")!;
            returnTypeNode.InnerXml = @"<generic>
						                    <genericArgumentName>B</genericArgumentName>
					                    </generic>";
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
                    Strings.funcGenericReturnTypeArgNameNotFoundFormat,
                    "B",
                    FUNCTIONNAME,
                    string.Join(", ", genericArguments)
                ),
                result.Errors.First()
            );
        }

        [Fact]
        public void UnknownGenericArgumentNameForParametersReturnsFailureResponse()
        {
            //arrange
            IFunctionsXmlValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.Type)!,
                [XmlDataConstants.TYPENAMEELEMENT] = "SomeType"
            };
            XmlDocument document = new();
            document.LoadXml(GetXmlString(fieldsToSet));
            XmlNode genericArgumentsNode = document.SelectSingleNode($"//{XmlDataConstants.GENERICARGUMENTSELEMENT}")!;
            genericArgumentsNode.InnerXml = @"<item>A</item>";
            XmlNode parametersNode = document.SelectSingleNode($"//{XmlDataConstants.PARAMETERSELEMENT}")!;
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
                    Strings.funcGenericParameterArgNameNotFoundFormat,
                    "B",
                    "value1",
                    FUNCTIONNAME,
                    string.Join(", ", genericArguments)
                ),
                result.Errors.First()
            );
        }

        private static string GetXmlString(IDictionary<string, string>? valuesToSet = null)
        {
            string xmlString = $@"<forms>
                                    <form name=""FUNCTIONS"">
                                    <folder name=""Functions"">
                                        <function name=""{FUNCTIONNAME}"" >
                                        <memberName>AccessAfter</memberName>
                                        <functionCategory>Standard</functionCategory>
                                        <typeName />
                                        <referenceName></referenceName>
                                        <referenceDefinition></referenceDefinition>
                                        <castReferenceAs />
                                        <referenceCategory>This</referenceCategory>
                                        <parametersLayout>Sequential</parametersLayout>
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
                                        <returnType>
                                            <literal>
                                            <literalType>Void</literalType>
                                            </literal>
                                        </returnType>
                                        <summary>Updates the access the access after field.</summary>
                                        </function>
                                    </folder>
                                    </form>
                                    <form name=""BUILT IN FUNCTIONS"">
                                    <folder name=""Built In Functions"">
                                    </folder>
                                    </form>
                                </forms>";

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
