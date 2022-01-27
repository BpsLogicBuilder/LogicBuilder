using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

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
            serviceProvider = new ServiceCollection()
                .AddSingleton<IStringHelper, StringHelper>()
                .BuildServiceProvider();
        }
    }
}
