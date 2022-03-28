using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class FunctionValidationHelperTest
    {
        public FunctionValidationHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(ValidIndirectReference.ArrayIndexer, "100")]
        [InlineData(ValidIndirectReference.BooleanKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.ByteKeyIndexer, "1")]
        [InlineData(ValidIndirectReference.CharKeyIndexer, "f")]
        [InlineData(ValidIndirectReference.DateKeyIndexer, "2020-10-10")]
        [InlineData(ValidIndirectReference.DateTimeKeyIndexer, "2020-10-10 13:13:13")]
        [InlineData(ValidIndirectReference.DateTimeOffsetKeyIndexer, "2020-10-10 13:13:13 +01:00")]
        [InlineData(ValidIndirectReference.DateOnlyKeyIndexer, "2020-10-10")]
        [InlineData(ValidIndirectReference.DecimalKeyIndexer, "3")]
        [InlineData(ValidIndirectReference.DoubleKeyIndexer, "4")]
        [InlineData(ValidIndirectReference.FloatKeyIndexer, "5")]
        [InlineData(ValidIndirectReference.GuidKeyIndexer, "{2D64191A-C055-4E41-BF86-3781D775FA97}")]
        [InlineData(ValidIndirectReference.IntegerKeyIndexer, "6")]
        [InlineData(ValidIndirectReference.LongKeyIndexer, "7")]
        [InlineData(ValidIndirectReference.SByteKeyIndexer, "9")]
        [InlineData(ValidIndirectReference.ShortKeyIndexer, "9")]
        [InlineData(ValidIndirectReference.StringKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.TimeOfDayKeyIndexer, "13:13:13")]
        [InlineData(ValidIndirectReference.TimeSpanKeyIndexer, "13:13:13")]
        [InlineData(ValidIndirectReference.TimeOnlyKeyIndexer, "13:13:13")]
        [InlineData(ValidIndirectReference.UIntegerKeyIndexer, "10")]
        [InlineData(ValidIndirectReference.ULongKeyIndexer, "11")]
        [InlineData(ValidIndirectReference.UShortKeyIndexer, "12")]
        [InlineData(ValidIndirectReference.Field, "FieldName")]
        [InlineData(ValidIndirectReference.Property, "PropertyName")]
        internal void ValidNonVariableIndirectReferenceIndexersWork(ValidIndirectReference validIndirectReference, string referenceName)
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(validIndirectReference, referenceName, String.Empty, errors);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(ValidIndirectReference.ArrayIndexer, "false")]
        [InlineData(ValidIndirectReference.BooleanKeyIndexer, "NotFalse")]
        [InlineData(ValidIndirectReference.ByteKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.CharKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateTimeKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateTimeOffsetKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateOnlyKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DecimalKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DoubleKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.FloatKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.GuidKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.IntegerKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.LongKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.SByteKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.ShortKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.TimeOfDayKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.TimeSpanKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.TimeOnlyKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.UIntegerKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.ULongKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.UShortKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.Field, "3")]
        [InlineData(ValidIndirectReference.Property, "4")]
        internal void InvalidNonVariableIndirectReferenceIndexersFail(ValidIndirectReference validIndirectReference, string referenceName)
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(validIndirectReference, referenceName, String.Empty, errors);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void VariableReferenceDefinitionIndexerWorks()
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();

            IVariableListInitializer variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(VariableReferenceDefinitionIndexerWorks)
            );
            updateVariables.Update(GetVariablesDocument());
            configurationService.VariableList = variableListInitializer.InitializeList();

            string indexKeyVariableName = "VariableUsedAsIndexer";
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(ValidIndirectReference.VariableKeyIndexer, indexKeyVariableName, String.Empty, errors);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);

            static XmlDocument GetVariablesDocument()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<folder name=""Decisions"">
                                          <objectVariable name=""DDD"">
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
                                              <objectType>MMM</objectType>
                                            </objectVariable>
                                            <folder name=""Literals"">
		                                        <literalVariable name=""VariableUsedAsIndexer"">
			                                        <memberName>actg</memberName>
			                                        <variableCategory>Property</variableCategory>
			                                        <castVariableAs />
			                                        <typeName />
			                                        <referenceName />
			                                        <referenceDefinition></referenceDefinition>
			                                        <castReferenceAs />
			                                        <referenceCategory>This</referenceCategory>
			                                        <evaluation>Implemented</evaluation>
			                                        <comments />
			                                        <metadata />
			                                        <literalType>Decimal</literalType>
			                                        <control>SingleLineTextBox</control>
			                                        <propertySource />
			                                        <defaultValue />
			                                        <domain />
		                                        </literalVariable>
											</folder>
                                        </folder>");

                return xmlDocument;
            }
        }

        [Fact]
        public void VariableIndexerFailsIfIndexKeyVariableNameIsNotInTheVariablesDicionary()
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();

            IVariableListInitializer variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(VariableReferenceDefinitionIndexerWorks)
            );
            updateVariables.Update(GetVariablesDocument());
            configurationService.VariableList = variableListInitializer.InitializeList();

            string indexKeyVariableName = "VariableUsedAsIndexer";
            string functionName = "Test";
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(ValidIndirectReference.VariableKeyIndexer, indexKeyVariableName, functionName, errors);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableKeyReferenceIsInvalidFormat3, enumHelper.GetVisibleEnumText(ValidIndirectReference.VariableKeyIndexer), indexKeyVariableName, functionName),
                errors[0]
            );

            static XmlDocument GetVariablesDocument()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<folder name=""Decisions"">
                                          <objectVariable name=""DDD"">
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
                                              <objectType>MMM</objectType>
                                            </objectVariable>
                                            <folder name=""Literals"">
											</folder>
                                        </folder>");

                return xmlDocument;
            }
        }

        [Fact]
        public void ArrayVariableIndexerWorks()
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();

            IVariableListInitializer variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(VariableReferenceDefinitionIndexerWorks)
            );
            updateVariables.Update(GetVariablesDocument());
            configurationService.VariableList = variableListInitializer.InitializeList();

            string indexKeyVariableName = "VariableUsedAsIndexer";
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, String.Empty, errors);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);

            static XmlDocument GetVariablesDocument()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<folder name=""Decisions"">
                                          <objectVariable name=""DDD"">
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
                                              <objectType>MMM</objectType>
                                            </objectVariable>
                                            <folder name=""Literals"">
		                                        <literalVariable name=""VariableUsedAsIndexer"">
			                                        <memberName>actg</memberName>
			                                        <variableCategory>Property</variableCategory>
			                                        <castVariableAs />
			                                        <typeName />
			                                        <referenceName />
			                                        <referenceDefinition></referenceDefinition>
			                                        <castReferenceAs />
			                                        <referenceCategory>This</referenceCategory>
			                                        <evaluation>Implemented</evaluation>
			                                        <comments />
			                                        <metadata />
			                                        <literalType>Integer</literalType>
			                                        <control>SingleLineTextBox</control>
			                                        <propertySource />
			                                        <defaultValue />
			                                        <domain />
		                                        </literalVariable>
											</folder>
                                        </folder>");

                return xmlDocument;
            }
        }

        [Fact]
        public void VariableArrayIndexerReferenceDefinitionFailsIfIndexKeyVariableNameIsNotInTheVariablesDicionary()
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();

            IVariableListInitializer variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(VariableReferenceDefinitionIndexerWorks)
            );
            updateVariables.Update(GetVariablesDocument());
            configurationService.VariableList = variableListInitializer.InitializeList();

            string indexKeyVariableName = "VariableUsedAsIndexer";
            string functionName = "Test";
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, functionName, errors);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableArrayKeyReferenceIsInvalidFormat3, enumHelper.GetVisibleEnumText(ValidIndirectReference.VariableArrayIndexer), indexKeyVariableName, functionName),
                errors[0]
            );

            static XmlDocument GetVariablesDocument()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<folder name=""Decisions"">
                                          <objectVariable name=""DDD"">
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
                                              <objectType>MMM</objectType>
                                            </objectVariable>
                                            <folder name=""Literals"">
											</folder>
                                        </folder>");

                return xmlDocument;
            }
        }

        [Fact]
        public void ArrayVariableIndexerFailsIfTheInexKeyVariableCannotBeAnInteger()
        {
            //arrange
            IFunctionValidationHelper helper = serviceProvider.GetRequiredService<IFunctionValidationHelper>();

            IVariableListInitializer variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(VariableReferenceDefinitionIndexerWorks)
            );
            updateVariables.Update(GetVariablesDocument());
            configurationService.VariableList = variableListInitializer.InitializeList();

            string indexKeyVariableName = "VariableUsedAsIndexer";
            string functionName = "Test";
            List<string> errors = new();

            //act
            helper.ValidateFunctionIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, functionName, errors);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableArrayKeyReferenceIsInvalidFormat3, enumHelper.GetVisibleEnumText(ValidIndirectReference.VariableArrayIndexer), indexKeyVariableName, functionName),
                errors[0]
            );

            static XmlDocument GetVariablesDocument()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<folder name=""Decisions"">
                                          <objectVariable name=""DDD"">
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
                                              <objectType>MMM</objectType>
                                            </objectVariable>
                                            <folder name=""Literals"">
		                                        <literalVariable name=""VariableUsedAsIndexer"">
			                                        <memberName>actg</memberName>
			                                        <variableCategory>Property</variableCategory>
			                                        <castVariableAs />
			                                        <typeName />
			                                        <referenceName />
			                                        <referenceDefinition></referenceDefinition>
			                                        <castReferenceAs />
			                                        <referenceCategory>This</referenceCategory>
			                                        <evaluation>Implemented</evaluation>
			                                        <comments />
			                                        <metadata />
			                                        <literalType>TimeSpan</literalType>
			                                        <control>SingleLineTextBox</control>
			                                        <propertySource />
			                                        <defaultValue />
			                                        <domain />
		                                        </literalVariable>
											</folder>
                                        </folder>");

                return xmlDocument;
            }
        }
    }
}
