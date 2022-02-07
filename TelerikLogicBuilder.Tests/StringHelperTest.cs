using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class StringHelperTest
    {
        public StringHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void EnsureUniqueNameReturmsTheInputValueWhenNotExisting()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.EnsureUniqueName("A", new HashSet<string> { });

            //assert
            Assert.StartsWith("A", result);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void EnsureUniqueNameReturmsNewValueWhenExisting()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.EnsureUniqueName("A", new HashSet<string> { "A" });

            //assert
            Assert.StartsWith("A0", result);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void SplitWithQuoteQualifierReturnsExpectedvalueUsingQuotes()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.SplitWithQuoteQualifier("A;\"A;A;A\"", ";");

            //assert
            Assert.Equal(2, result.Length);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void SplitWithQuoteQualifierReturnsExpectedvalueWithoutQuotes()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.SplitWithQuoteQualifier("A;B;C;A", ";");

            //assert
            Assert.Equal(4, result.Length);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void ToCamelCaseReturnsExpectedvalue()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.ToCamelCase("AndSoItGoes.EscapeToVictory");

            //assert
            Assert.Equal("andSoItGoes.escapeToVictory", result);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void ToCamelCaseReturnsExpectedvalueWithQuotes()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.ToCamelCase("AndSoItGoes.\"And.So.It.Goes\"");

            //assert
            Assert.Equal("andSoItGoes.and.So.It.Goes", result);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void ToShortNameReturnsExpectedvalue()
        {
            //arrange
            IStringHelper helper = serviceProvider.GetRequiredService<IStringHelper>();

            //act
            var result = helper.ToShortName("TelerikLogicBuilder.Tests.AttributeSamples.InstructorModel");

            //assert
            Assert.StartsWith("InstructorModel", result);
        }

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
