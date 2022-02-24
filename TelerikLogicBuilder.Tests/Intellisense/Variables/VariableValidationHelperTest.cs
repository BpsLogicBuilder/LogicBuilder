using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Variables
{
    public class VariableValidationHelperTest
    {
        public VariableValidationHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(ValidIndirectReference.BooleanKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.ByteKeyIndexer, "1")]
        [InlineData(ValidIndirectReference.CharKeyIndexer, "f")]
        [InlineData(ValidIndirectReference.DateKeyIndexer, "2020-10-10")]
        [InlineData(ValidIndirectReference.DateTimeKeyIndexer, "2020-10-10 13:13:13")]
        [InlineData(ValidIndirectReference.DateTimeOffsetKeyIndexer, "2020-10-10 13:13:13 +01:00")]
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
        [InlineData(ValidIndirectReference.UIntegerKeyIndexer, "10")]
        [InlineData(ValidIndirectReference.ULongKeyIndexer, "11")]
        [InlineData(ValidIndirectReference.UShortKeyIndexer, "12")]
        [InlineData(ValidIndirectReference.Field, "FieldName")]
        [InlineData(ValidIndirectReference.Property, "PropertyName")]
        internal void ValidNonVariableIndirectReferenceIndexersWork(ValidIndirectReference validIndirectReference, string referenceName)
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(validIndirectReference, referenceName, String.Empty, errors, null);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(ValidIndirectReference.BooleanKeyIndexer, "NotFalse")]
        [InlineData(ValidIndirectReference.ByteKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.CharKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateTimeKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.DateTimeOffsetKeyIndexer, "false")]
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
        [InlineData(ValidIndirectReference.UIntegerKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.ULongKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.UShortKeyIndexer, "false")]
        [InlineData(ValidIndirectReference.Field, "3")]
        [InlineData(ValidIndirectReference.Property, "4")]
        internal void InvalidNonVariableIndirectReferenceIndexersFail(ValidIndirectReference validIndirectReference, string referenceName)
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(validIndirectReference, referenceName, String.Empty, errors, null);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void VariableReferenceDefinitionIndexerWorks()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "Name";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();
            PropertyInfo indexKeyVariablePropertyInfo = typeof(TestVariableClass).GetProperty(indexKeyVariableName);
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyDictionary");
            PropertyInfo indexPropertyInfo = dictionaryPropertyInfo.PropertyType.GetProperty("Item");
            PropertyInfo variablePropertyInfo = indexPropertyInfo.PropertyType.GetProperty("MyProperty");

            variables.Add
            (
                indexKeyVariablePropertyInfo.Name, //variable to use in the index
                variablesManager.GetVariable
                (
                    indexKeyVariablePropertyInfo.Name,
                    indexKeyVariablePropertyInfo.Name,
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ReferenceCategories.This,
                    indexKeyVariablePropertyInfo,
                    indexKeyVariablePropertyInfo.PropertyType
                )
            );

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyDictionary.{indexKeyVariableName}",
                    "Property.VariableKeyIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableKeyIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void VariableIndexerFailsIfIndexKeyVariableNameIsNotInTheVariablesDicionary()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            string indexKeyVariableName = "Name";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();

            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyDictionary");
            PropertyInfo indexPropertyInfo = dictionaryPropertyInfo.PropertyType.GetProperty("Item");
            PropertyInfo variablePropertyInfo = indexPropertyInfo.PropertyType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyDictionary.{indexKeyVariableName}",
                    "Property.VariableKeyIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableKeyIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableKeyReferenceIsInvalidFormat2, enumHelper.GetVisibleEnumText(ValidIndirectReference.VariableKeyIndexer), indexKeyVariableName, variableName),
                errors[0]
            );
        }

        [Fact]
        public void VariableReferenceIndexerFailsIfIndexKeyVariableNameIsTheSameAsTheIndexerVariable()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "VariableKeyIndexerVariable";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();

            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyDictionary");
            PropertyInfo indexPropertyInfo = dictionaryPropertyInfo.PropertyType.GetProperty("Item");
            PropertyInfo variablePropertyInfo = indexPropertyInfo.PropertyType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyDictionary.{indexKeyVariableName}",
                    "Property.VariableKeyIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableKeyIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.referenceCannotEqualVariable, variableName, variableName),
                errors[0]
            );
        }

        [Fact]
        public void ArrayVariableIndexerWorks()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "MyInt";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();
            PropertyInfo indexKeyVariablePropertyInfo = typeof(TestVariableClass).GetProperty(indexKeyVariableName);
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                indexKeyVariablePropertyInfo.Name, //variable to use in the index
                variablesManager.GetVariable
                (
                    indexKeyVariablePropertyInfo.Name,
                    indexKeyVariablePropertyInfo.Name,
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ReferenceCategories.This,
                    indexKeyVariablePropertyInfo,
                    indexKeyVariablePropertyInfo.PropertyType
                )
            );

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyValueClassArray.{indexKeyVariableName}",
                    "Property.VariableArrayIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void VariableArrayIndexerReferenceDefinitionFailsIfIndexKeyVariableNameIsNotInTheVariablesDicionary()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            string indexKeyVariableName = "MyInt";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();

            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyValueClassArray.{indexKeyVariableName}",
                    "Property.VariableArrayIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableArrayKeyReferenceIsInvalidFormat2, enumHelper.GetVisibleEnumText(ValidIndirectReference.VariableArrayIndexer), indexKeyVariableName, variableName),
                errors[0]
            );
        }

        [Fact]
        public void ArrayVariableIndexerFailsIfIndexKeyVariableNameIsTheSameAsTheArrayIndexVariable()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "VariableKeyIndexerVariable";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();

            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyValueClassArray.{indexKeyVariableName}",
                    "Property.VariableArrayIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.referenceCannotEqualVariable, indexKeyVariableName, variableName),
                errors[0]
            );
        }

        [Fact]
        public void ArrayVariableIndexerFailsIfTheInexKeyVariableCannotBeAnInteger()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            string indexKeyVariableName = "Name";
            string variableName = "VariableKeyIndexerVariable";
            Dictionary<string, VariableBase> variables = new();
            PropertyInfo indexKeyVariablePropertyInfo = typeof(TestVariableClass).GetProperty(indexKeyVariableName);
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                indexKeyVariablePropertyInfo.Name, //variable to use in the index
                variablesManager.GetVariable
                (
                    indexKeyVariablePropertyInfo.Name,
                    indexKeyVariablePropertyInfo.Name,
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ReferenceCategories.This,
                    indexKeyVariablePropertyInfo,
                    indexKeyVariablePropertyInfo.PropertyType
                )
            );

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    "MyProperty",
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    $"MyValueClassArray.{indexKeyVariableName}",
                    "Property.VariableArrayIndexer",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateVariableIndirectReferenceName(ValidIndirectReference.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableArrayKeyReferenceIsInvalidFormat2, enumHelper.GetVisibleEnumText(ValidIndirectReference.VariableArrayIndexer), indexKeyVariableName, variableName),
                errors[0]
            );
        }

        [Theory]
        [InlineData(VariableCategory.BooleanKeyIndexer, "false")]
        [InlineData(VariableCategory.ByteKeyIndexer, "1")]
        [InlineData(VariableCategory.CharKeyIndexer, "f")]
        [InlineData(VariableCategory.DateKeyIndexer, "2020-10-10")]
        [InlineData(VariableCategory.DateTimeKeyIndexer, "2020-10-10 13:13:13")]
        [InlineData(VariableCategory.DateTimeOffsetKeyIndexer, "2020-10-10 13:13:13 +01:00")]
        [InlineData(VariableCategory.DecimalKeyIndexer, "3")]
        [InlineData(VariableCategory.DoubleKeyIndexer, "4")]
        [InlineData(VariableCategory.FloatKeyIndexer, "5")]
        [InlineData(VariableCategory.GuidKeyIndexer, "{2D64191A-C055-4E41-BF86-3781D775FA97}")]
        [InlineData(VariableCategory.IntegerKeyIndexer, "6")]
        [InlineData(VariableCategory.LongKeyIndexer, "7")]
        [InlineData(VariableCategory.SByteKeyIndexer, "9")]
        [InlineData(VariableCategory.ShortKeyIndexer, "9")]
        [InlineData(VariableCategory.StringKeyIndexer, "false")]
        [InlineData(VariableCategory.TimeOfDayKeyIndexer, "13:13:13")]
        [InlineData(VariableCategory.TimeSpanKeyIndexer, "13:13:13")]
        [InlineData(VariableCategory.UIntegerKeyIndexer, "10")]
        [InlineData(VariableCategory.ULongKeyIndexer, "11")]
        [InlineData(VariableCategory.UShortKeyIndexer, "12")]
        [InlineData(VariableCategory.Field, "FieldName")]
        [InlineData(VariableCategory.Property, "PropertyName")]
        internal void ValidNonVariableVariableCategoryIndexersWork(VariableCategory variableCategory, string memberName)
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            List<string> errors = new();

            //act
            helper.ValidateMemberName(variableCategory, memberName, String.Empty, errors, null);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(VariableCategory.BooleanKeyIndexer, "NotFalse")]
        [InlineData(VariableCategory.ByteKeyIndexer, "false")]
        [InlineData(VariableCategory.CharKeyIndexer, "false")]
        [InlineData(VariableCategory.DateKeyIndexer, "false")]
        [InlineData(VariableCategory.DateTimeKeyIndexer, "false")]
        [InlineData(VariableCategory.DateTimeOffsetKeyIndexer, "false")]
        [InlineData(VariableCategory.DecimalKeyIndexer, "false")]
        [InlineData(VariableCategory.DoubleKeyIndexer, "false")]
        [InlineData(VariableCategory.FloatKeyIndexer, "false")]
        [InlineData(VariableCategory.GuidKeyIndexer, "false")]
        [InlineData(VariableCategory.IntegerKeyIndexer, "false")]
        [InlineData(VariableCategory.LongKeyIndexer, "false")]
        [InlineData(VariableCategory.SByteKeyIndexer, "false")]
        [InlineData(VariableCategory.ShortKeyIndexer, "false")]
        [InlineData(VariableCategory.TimeOfDayKeyIndexer, "false")]
        [InlineData(VariableCategory.TimeSpanKeyIndexer, "false")]
        [InlineData(VariableCategory.UIntegerKeyIndexer, "false")]
        [InlineData(VariableCategory.ULongKeyIndexer, "false")]
        [InlineData(VariableCategory.UShortKeyIndexer, "false")]
        [InlineData(VariableCategory.Field, "3")]
        [InlineData(VariableCategory.Property, "4")]
        internal void InvalidNonVariableVariableCategoryIndexersFail(VariableCategory variableCategory, string memberName)
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            List<string> errors = new();

            //act
            helper.ValidateMemberName(variableCategory, memberName, String.Empty, errors, null);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
        }

        [Fact]
        public void VariableVariableCategoryIndexerWorks()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "Name";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();
            PropertyInfo indexKeyVariablePropertyInfo = typeof(TestVariableClass).GetProperty(indexKeyVariableName);
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyDictionary");
            PropertyInfo indexPropertyInfo = dictionaryPropertyInfo.PropertyType.GetProperty("Item");
            PropertyInfo variablePropertyInfo = indexPropertyInfo.PropertyType.GetProperty("MyProperty");

            variables.Add
            (
                indexKeyVariablePropertyInfo.Name, //variable to use in the index
                variablesManager.GetVariable
                (
                    indexKeyVariablePropertyInfo.Name,
                    indexKeyVariablePropertyInfo.Name,
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ReferenceCategories.This,
                    indexKeyVariablePropertyInfo,
                    indexKeyVariablePropertyInfo.PropertyType
                )
            );

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableKeyIndexer,
                    string.Empty,
                    string.Empty,
                    "MyDictionary",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableKeyIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void VariableVariableCategoryIndexerFailsIfIndexVariableIsNotInTheDictionary()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "Name";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();

            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyDictionary");
            PropertyInfo indexPropertyInfo = dictionaryPropertyInfo.PropertyType.GetProperty("Item");
            PropertyInfo variablePropertyInfo = indexPropertyInfo.PropertyType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableKeyIndexer,
                    string.Empty,
                    string.Empty,
                    "MyDictionary",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableKeyIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableKeyIndexIsInvalidFormat, variableName),
                errors[0]
            );
        }

        [Fact]
        public void VariableVariableCategoryIndexerFailsIfIndexKeyVariableNameIsTheSameAsTheIndexerVariable()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "VariableKeyIndexerVariable";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();

            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyDictionary");
            PropertyInfo indexPropertyInfo = dictionaryPropertyInfo.PropertyType.GetProperty("Item");
            PropertyInfo variablePropertyInfo = indexPropertyInfo.PropertyType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableKeyIndexer,
                    string.Empty,
                    string.Empty,
                    "MyDictionary",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableKeyIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableIndexCannotBeSelfFormat, variableName),
                errors[0]
            );
        }

        [Fact]
        public void VariableArrayIndexerVariableCategoryWorks()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "MyInt";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();
            PropertyInfo indexKeyVariablePropertyInfo = typeof(TestVariableClass).GetProperty(indexKeyVariableName);
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                indexKeyVariablePropertyInfo.Name, //variable to use in the index
                variablesManager.GetVariable
                (
                    indexKeyVariablePropertyInfo.Name,
                    indexKeyVariablePropertyInfo.Name,
                    VariableCategory.Property,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    ReferenceCategories.This,
                    indexKeyVariablePropertyInfo,
                    indexKeyVariablePropertyInfo.PropertyType
                )
            );

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableArrayIndexer,
                    string.Empty,
                    string.Empty,
                    "MyValueClassArray",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void VariableArrayIndexerVariableCategoryFailsIfIndexKeyVariableNameIsNotInTheVariablesDicionary()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "MyInt";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableArrayIndexer,
                    string.Empty,
                    string.Empty,
                    "MyValueClassArray",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableArrayIndexIsInvalidFormat, variableName),
                errors[0]
            );
        }

        [Fact]
        public void VariableArrayIndexerVariableCategoryFailsIfIndexKeyVariableNameIsTheSameAsTheArrayIndexVariable()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "VariableKeyIndexerVariable";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableArrayIndexer,
                    string.Empty,
                    string.Empty,
                    "MyValueClassArray",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableIndexCannotBeSelfFormat, variableName),
                errors[0]
            );
        }


        [Fact]
        public void VariableArrayIndexerVariableCategoryFailsIfTheInexKeyVariableCannotBeAnInteger()
        {
            //arrange
            IVariableValidationHelper helper = serviceProvider.GetRequiredService<IVariableValidationHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            string indexKeyVariableName = "Name";
            string variableName = "VariableKeyIndexerVariable";

            Dictionary<string, VariableBase> variables = new();
            PropertyInfo dictionaryPropertyInfo = typeof(TestVariableClass).GetProperty("MyValueClassArray");
            MethodInfo getMethodInfo = dictionaryPropertyInfo.PropertyType.GetMethod("Get");
            PropertyInfo variablePropertyInfo = getMethodInfo.ReturnType.GetProperty("MyProperty");

            variables.Add
            (
                variableName,
                variablesManager.GetVariable
                (
                    variableName,
                    indexKeyVariableName,
                    VariableCategory.VariableArrayIndexer,
                    string.Empty,
                    string.Empty,
                    "MyValueClassArray",
                    "Property",
                    string.Empty,
                    ReferenceCategories.InstanceReference,
                    variablePropertyInfo,
                    variablePropertyInfo.PropertyType
                )
            );

            List<string> errors = new();

            //act
            helper.ValidateMemberName(VariableCategory.VariableArrayIndexer, indexKeyVariableName, variableName, errors, variables);
            var result = errors.Count == 0;

            //assert
            Assert.False(result);
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.variableArrayIndexIsInvalidFormat, variableName),
                errors[0]
            );
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        class TestVariableClass
        {
            public string Name { get; set; }
            public int MyInt { get; set; }
            public int? MyNullableInt { get; set; }
            public short MyShort { get; set; }
            public byte MyByte { get; set; }
            public Dictionary<string, MyValueClass> MyDictionary { get; set; }
            public MyValueClass[] MyValueClassArray { get; set; }
        }

        class MyValueClass
        {
            public string MyProperty { get; set; }
        }
    }
}
