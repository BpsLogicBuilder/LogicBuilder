using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
    public class VariablesXmlValidatorTest
    {
        public VariablesXmlValidatorTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        private const string VARIABLENAME = "VariableName";
        #endregion Fields

        [Fact]
        public void CanCreateVariablesXmlValidator()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        [Fact]
        public void ValidateValidXmlWorks()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();

            //act
            var result = xmlValidator.Validate(GetXmlString());

            //assert
            Assert.True(result.Success);
        }

        [Fact]
        public void ValidateThrowsXmlExceptionForInvalidXml()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();

            //act
            Assert.Throws<XmlException>(() => xmlValidator.Validate(@"<folder1 name=""variables"">
                                                                    </folder>"));
        }

        [Fact]
        public void ValidateReturnsFailureResponseForInvalidStructure()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();

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
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
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
                    Strings.variableInvalidIndirectDefinitionFormat,
                    VARIABLENAME,
                    "InvalidReferenceDefinitionName",
                    Environment.NewLine,
                    enumHelper.GetValidIndirectReferencesList()
                ),
                result.Errors.First()
            );
        }

        [Fact]
        public void InvalidmemberNameReturnsFailureResponse()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.MEMBERNAMEELEMENT] = "2MemberName"
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableNameIsInvalidFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("This")]
        [InlineData("Type")]
        public void ReferenceNamePopulatedAndMustBeEmptyReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
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
                string.Format(CultureInfo.CurrentCulture, Strings.variableReferenceMustBeEmptyFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("This")]
        [InlineData("Type")]
        public void ReferenceDefinitionPopulatedAndMustBeEmptyReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
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
                string.Format(CultureInfo.CurrentCulture, Strings.variableReferenceDefinitionMustBeEmptyFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData("This")]
        [InlineData("Type")]
        public void CastReferenceAsPopulatedAndMustBeEmptyReturnsFailureResponse(string referenceCategory)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.CASTREFERENCEASELEMENT] = "Foo",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = referenceCategory
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.castReferenceAsMustBeEmptyFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.InstanceReference, "", "Property")]
        [InlineData(ReferenceCategories.StaticReference, "", "Property")]
        [InlineData(ReferenceCategories.InstanceReference, "Foo", "")]
        [InlineData(ReferenceCategories.StaticReference, "Foo", "")]
        internal void ReferenceNameOrDefinitionEmptyMustBePopulatedReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableReferenceInfoMustBePopulatedFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.InstanceReference, "Foo.Bar", "Property")]
        [InlineData(ReferenceCategories.StaticReference, "Foo.Bar", "Property")]
        internal void ReferenceDefinitionLengthsDoesNotMatchReferenceNameLengthReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableNameAndDefinitionFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Fact]
        public void CastReferenceAsLengthsDoesNotMatchReferenceNameLengthReturnsFailureResponse()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = "Property",
                [XmlDataConstants.REFERENCENAMEELEMENT] = "Foo",
                [XmlDataConstants.CASTREFERENCEASELEMENT] = "A.V",
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), ReferenceCategories.InstanceReference)
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.referenceNameAndCastReferenceAFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.Type, "", "", "2Pac")]
        [InlineData(ReferenceCategories.StaticReference, "Foo", "Property", "2Pac")]
        internal void InvalidTypeNameReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition, string typeName)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.TYPENAMEELEMENT] = typeName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.typeReferenceNameIsInvalidFormatVariable, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Theory]
        [InlineData(ReferenceCategories.This, "", "", "Pac")]
        [InlineData(ReferenceCategories.InstanceReference, "Foo", "Property", "Pac")]
        internal void TypeNameNotEmptyReturnsFailureResponse(ReferenceCategories referenceCategory, string referenceName, string referenceDefinition, string typeName)
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
            Dictionary<string, string> fieldsToSet = new()
            {
                [XmlDataConstants.REFERENCEDEFINITIONELEMENT] = referenceDefinition,
                [XmlDataConstants.REFERENCENAMEELEMENT] = referenceName,
                [XmlDataConstants.TYPENAMEELEMENT] = typeName,
                [XmlDataConstants.REFERENCECATEGORYELEMENT] = Enum.GetName(typeof(ReferenceCategories), referenceCategory)
            };

            //act
            var result = xmlValidator.Validate(GetXmlString(fieldsToSet));

            //assert
            Assert.False(result.Success);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableTypeNameMustBeEmptyFormat, VARIABLENAME),
                result.Errors.First()
            );
        }

        [Fact]
        public void InvalidIndirectReferenceNameReturnsFailureResponse()
        {
            //arrange
            IVariablesXmlValidator xmlValidator = serviceProvider.GetRequiredService<IVariablesXmlValidator>();
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

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        private static string GetXmlString(IDictionary<string, string> valuesToSet = null)
        {
            string xmlString = $@"<folder name=""variables"">
                                    <objectVariable name=""{VARIABLENAME}"">
			                            <memberName>acrg</memberName>
			                            <variableCategory>Property</variableCategory>
			                            <castVariableAs />
			                            <typeName />
			                            <referenceName />
			                            <referenceDefinition />
			                            <castReferenceAs />
			                            <referenceCategory>This</referenceCategory>
			                            <evaluation>Implemented</evaluation>
			                            <comments />
			                            <metadata />
			                            <objectType>Decimal</objectType>
		                            </objectVariable>
                                </folder>";

            if (valuesToSet == null)
                return xmlString;

            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);

            foreach (KeyValuePair<string, string> kvp in valuesToSet)
            {
                XmlNode xmlNode = xmlDocument.SelectSingleNode($"//{kvp.Key}"); 
                xmlNode.InnerText = kvp.Value;
            };

            return xmlDocument.DocumentElement.OuterXml;
        }
    }
}
