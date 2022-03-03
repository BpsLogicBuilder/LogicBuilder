using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikLogicBuilder.Tests.AttributeSamples;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class MemberAttributeReaderTest
    {
        public MemberAttributeReaderTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void GetNameValueTableReturnsExpectedItems()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            var dictionary = attributeReader.GetNameValueTable(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FirstName))!);

            //assert
            Assert.Equal(2, dictionary.Count);
            Assert.Equal("John", dictionary[AttributeNames.DEFAULTVALUE]);
            Assert.Equal(nameof(InstructorModel.FullName), dictionary[AttributeNames.PROPERTYSOURCE]);
        }

        [Fact]
        public void GetNameValueTableReturnsEmptyDictionaryForPropertyWithoutnameValueAttribute()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            var dictionary = attributeReader.GetNameValueTable(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Empty(dictionary);
        }

        [Fact]
        public void GetLiteralInputStyleReturnsExpectedVariableControlType()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            LiteralVariableInputStyle literalInputStyle = attributeReader.GetLiteralInputStyle(typeof(InstructorModel).GetProperty(nameof(InstructorModel.LastName))!);

            //assert
            Assert.Equal(LiteralVariableInputStyle.DomainAutoComplete, literalInputStyle);
        }

        [Fact]

        public void GetLiteralInputStyleReturnsDefaultVariableControlTypeWithNoAttributeAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            LiteralVariableInputStyle literalInputStyle = attributeReader.GetLiteralInputStyle(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Equal(LiteralVariableInputStyle.SingleLineTextBox, literalInputStyle);
        }

        [Fact]
        public void GetObjectInputStyleReturnsExpectedVariableControlType()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            ObjectVariableInputStyle objectVariableInputStyle = attributeReader.GetObjectInputStyle(typeof(InstructorModel).GetProperty(nameof(InstructorModel.OfficeAssignment))!);

            //assert
            Assert.Equal(ObjectVariableInputStyle.Form, objectVariableInputStyle);
        }

        [Fact]
        public void GetObjectInputStyleReturnsDefaultVariableControlTypeWithNoAttributeAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            ObjectVariableInputStyle objectVariableInputStyle = attributeReader.GetObjectInputStyle(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Equal(ObjectVariableInputStyle.Form, objectVariableInputStyle);
        }

        [Fact]
        public void GetListInputStyleReturnsExpectedVariableControlType()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            ListVariableInputStyle listInputStyle = attributeReader.GetListInputStyle(typeof(InstructorModel).GetProperty(nameof(InstructorModel.Courses))!);

            //assert
            Assert.Equal(ListVariableInputStyle.ListForm, listInputStyle);
        }

        [Fact]
        public void GetListInputStyleReturnsDefaultVariableControlTypeWithNoAttributeAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            ListVariableInputStyle listInputStyle = attributeReader.GetListInputStyle(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Equal(ListVariableInputStyle.HashSetForm, listInputStyle);
        }

        [Fact]
        public void GetSummaryReturnsExpectedSummaryForFunction()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string summary = attributeReader.GetSummary(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomething))!);

            //assert
            Assert.Equal("DoSomething", summary);
        }

        [Fact]
        public void GetSummaryReturnsEmptyStringWithNoAttributeAttachedForFunction()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string summary = attributeReader.GetSummary(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomethingElse))!);

            //assert
            Assert.Equal(string.Empty, summary);
        }

        [Fact]
        public void GetSummaryReturnsExpectedSummaryForConstructor()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string summary = attributeReader.GetSummary(typeof(FormControlSettingsParameters).GetConstructors().Single());

            //assert
            Assert.Equal("Form Control Settings Parameters", summary);
        }

        [Fact]
        public void GetSummaryReturnsEmptyStringWithNoAttributeAttachedForConstructor()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string summary = attributeReader.GetSummary(typeof(InstructorModel).GetConstructors().Single());

            //assert
            Assert.Equal(string.Empty, summary);
        }

        [Fact]
        public void GetVariableCommentsReturnsExpectedComments()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string comments = attributeReader.GetVariableComments(typeof(InstructorModel).GetProperty(nameof(InstructorModel.LastName))!);

            //assert
            Assert.Equal("Last Name", comments);
        }

        [Fact]
        public void GetVariableCommentsReturnsEmptyStringWithNoAttributeAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string comments = attributeReader.GetVariableComments(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Equal(string.Empty, comments);
        }

        [Fact]
        public void GetAlsoKnownAsReturnsExpectedTextForProperty()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string text = attributeReader.GetAlsoKnownAs(typeof(InstructorModel).GetProperty(nameof(InstructorModel.LastName))!);

            //assert
            Assert.Equal("Instructor_LastName", text);
        }

        [Fact]
        public void GetAlsoKnownAsReturnsEmptyStringWithNoAttributeAttachedForProperty()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string text = attributeReader.GetAlsoKnownAs(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Equal(string.Empty, text);
        }

        [Fact]
        public void GetAlsoKnownAsReturnsExpectedTextForMethod()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string text = attributeReader.GetAlsoKnownAs(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomething))!);

            //assert
            Assert.Equal("Do Something", text);
        }

        [Fact]
        public void GetAlsoKnownAsReturnsEmptyStringWithNoAttributeAttachedForMethod()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string text = attributeReader.GetAlsoKnownAs(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomethingElse))!);

            //assert
            Assert.Equal(string.Empty, text);
        }

        [Fact]
        public void GetAlsoKnownAsReturnsExpectedTextForConstructor()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string text = attributeReader.GetAlsoKnownAs(typeof(FormControlSettingsParameters).GetConstructors().Single());

            //assert
            Assert.Equal("FormControlSettingsParameters", text);
        }

        [Fact]
        public void GetAlsoKnownAsReturnsEmptyStringWithNoAttributeAttachedForConstructor()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            string text = attributeReader.GetAlsoKnownAs(typeof(InstructorModel).GetConstructors().Single());

            //assert
            Assert.Equal(string.Empty, text);
        }

        [Fact]
        public void GetFunctionCategoryReturnsExpectedCategory()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            FunctionCategories category = attributeReader.GetFunctionCategory(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomething))!);

            //assert
            Assert.Equal(FunctionCategories.DialogForm, category);
        }

        [Fact]
        public void GetFunctionCategoryReturnsUnknownCategoryithNoAttributeAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            FunctionCategories category = attributeReader.GetFunctionCategory(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomethingElse))!);

            //assert
            Assert.Equal(FunctionCategories.Unknown, category);
        }

        [Fact]
        public void GetDomainReturnsExpectedList()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            List<string> domain = attributeReader.GetDomain(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FirstName))!);

            //assert
            Assert.True(domain.SequenceEqual(new List<string> { "A", "B", "C" }));
        }

        [Fact]
        public void GetDomainReturnsEmptyListWithNoAttributeAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            List<string> domain = attributeReader.GetDomain(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.Empty(domain);
        }

        [Fact]
        public void IsVariableConfigurableFromClassHelperReturnsTrueWithLogicBuilderAttributesAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            bool isConfigurable = attributeReader.IsVariableConfigurableFromClassHelper(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FirstName))!);

            //assert
            Assert.True(isConfigurable);
        }

        [Fact]
        public void IsVariableConfigurableFromClassHelperReturnsFalseWithoutLogicBuilderAttributesAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            bool isConfigurable = attributeReader.IsVariableConfigurableFromClassHelper(typeof(InstructorModel).GetProperty(nameof(InstructorModel.FullName))!);

            //assert
            Assert.False(isConfigurable);
        }

        [Fact]
        public void IsFunctionConfigurableFromClassHelperReturnsTrueWithLogicBuilderAttributesAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            bool isConfigurable = attributeReader.IsFunctionConfigurableFromClassHelper(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomething))!);

            //assert
            Assert.True(isConfigurable);
        }

        [Fact]
        public void IsFunctionConfigurableFromClassHelperReturnsFalseWithoutLogicBuilderAttributesAttached()
        {
            //arrange
            IMemberAttributeReader attributeReader = serviceProvider.GetRequiredService<IMemberAttributeReader>();

            //act
            bool isConfigurable = attributeReader.IsFunctionConfigurableFromClassHelper(typeof(InstructorModel).GetMethod(nameof(InstructorModel.DoSomethingElse))!);

            //assert
            Assert.False(isConfigurable);
        }
    }
}
