﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TelerikLogicBuilder.Tests.AttributeSamples;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class ParameterAttributeReaderTest
    {
        public ParameterAttributeReaderTest()
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
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            var dictionary = attributeReader.GetNameValueTable(constructorInfo.GetParameters().Single(p => p.Name == "field"));

            //assert
            Assert.Equal(2, dictionary.Count);
            Assert.Equal("field", dictionary[AttributeNames.DEFAULTVALUE.ToLower()]);
            Assert.Equal("modelType", dictionary[AttributeNames.PROPERTYSOURCEPARAMETER.ToLower()]);
        }

        [Fact]
        public void GetNameValueTableReturnsEmptyDictionaryForPropertyWithoutnameValueAttribute()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            var dictionary = attributeReader.GetNameValueTable(constructorInfo.GetParameters().Single(p => p.Name == "type"));

            //assert
            Assert.Empty(dictionary);
        }

        [Fact]
        public void GetLiteralInputStyleReturnsExpectedParameterControlType()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            LiteralParameterInputStyle literalInputStyle = attributeReader.GetLiteralInputStyle(constructorInfo.GetParameters().Single(p => p.Name == "field"));

            //assert
            Assert.Equal(LiteralParameterInputStyle.ParameterSourcedPropertyInput, literalInputStyle);
        }

        [Fact]
        public void GetLiteralInputStyleReturnsDefaultParameterControlTypeWithNoAttributeAttached()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            LiteralParameterInputStyle literalInputStyle = attributeReader.GetLiteralInputStyle(constructorInfo.GetParameters().Single(p => p.Name == "placeHolder"));

            //assert
            Assert.Equal(LiteralParameterInputStyle.SingleLineTextBox, literalInputStyle);
        }

        [Fact]
        public void GetObjectInputStyleReturnsExpectedParameterControlType()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            ObjectParameterInputStyle objectVariableInputStyle = attributeReader.GetObjectInputStyle(constructorInfo.GetParameters().Single(p => p.Name == "someObject"));

            //assert
            Assert.Equal(ObjectParameterInputStyle.Form, objectVariableInputStyle);
        }

        [Fact]
        public void GetObjectInputStyleReturnsDefaultParameterControlTypeWithNoAttributeAttached()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            ObjectParameterInputStyle objectVariableInputStyle = attributeReader.GetObjectInputStyle(constructorInfo.GetParameters().Single(p => p.Name == "placeHolder"));

            //assert
            Assert.Equal(ObjectParameterInputStyle.Form, objectVariableInputStyle);
        }

        [Fact]
        public void GetListInputStyleReturnsExpectedParameterControlType()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            ListParameterInputStyle listInputStyle = attributeReader.GetListInputStyle(constructorInfo.GetParameters().Single(p => p.Name == "someList"));

            //assert
            Assert.Equal(ListParameterInputStyle.ListForm, listInputStyle);
        }

        [Fact]
        public void GetListInputStyleReturnsDefaultParameterControlTypeWithNoAttributeAttached()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            ListParameterInputStyle listInputStyle = attributeReader.GetListInputStyle(constructorInfo.GetParameters().Single(p => p.Name == "placeHolder"));

            //assert
            Assert.Equal(ListParameterInputStyle.HashSetForm, listInputStyle);
        }

        [Fact]
        public void GetParameterCommentsReturnsExpectedComments()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            string comments = attributeReader.GetComments(constructorInfo.GetParameters().Single(p => p.Name == "title"));

            //assert
            Assert.Equal("Title", comments);
        }

        [Fact]
        public void GetParameterCommentsReturnsEmptyStringWithNoAttributeAttached()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            string comments = attributeReader.GetComments(constructorInfo.GetParameters().Single(p => p.Name == "someObject"));

            //assert
            Assert.Equal(string.Empty, comments);
        }

        [Fact]
        public void GetDomainReturnsExpectedList()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();
            IStringHelper stringHelper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            List<string> domain = attributeReader.GetDomain(constructorInfo.GetParameters().Single(p => p.Name == "type"));

            //assert
            Assert.True(domain.SequenceEqual(new List<string> { "text", "numeric", "boolean", "date", "email" }));
        }

        [Fact]
        public void GetDomainReturnsEmptyListWithNoAttributeAttached()
        {
            //arrange
            IParameterAttributeReader attributeReader = serviceProvider.GetRequiredService<IParameterAttributeReader>();
            ConstructorInfo constructorInfo = typeof(FormControlSettingsParameters).GetConstructors().Single();

            //act
            List<string> domain = attributeReader.GetDomain(constructorInfo.GetParameters().Single(p => p.Name == "title"));

            //assert
            Assert.Empty(domain);
        }
    }
}
