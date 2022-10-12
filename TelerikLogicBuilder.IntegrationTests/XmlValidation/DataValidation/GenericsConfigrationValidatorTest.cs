using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class GenericsConfigrationValidatorTest
    {
        public GenericsConfigrationValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanGenericsConfigrationValidator()
        {
            //arrange
            IGenericsConfigrationValidator validator = serviceProvider.GetRequiredService<IGenericsConfigrationValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void GenericArgumentCountMatchesTypeWorksForMatchingConfiguredArguments()
        {
            //arrange
            IGenericConfigFactory genericConfigFactory = serviceProvider.GetRequiredService<IGenericConfigFactory>();
            List<GenericConfigBase> configuredData = new()
            {
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "B",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                )
            };
            IGenericsConfigrationValidator validator = serviceProvider.GetRequiredService<IGenericsConfigrationValidator>();

            //act
            var result = validator.GenericArgumentCountMatchesType(typeof(GenericClass<,>), configuredData);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void GenericArgumentCountMatchesTypeFailsForIncorrectNumberOfConfiguredArguments()
        {
            //arrange
            IGenericConfigFactory genericConfigFactory = serviceProvider.GetRequiredService<IGenericConfigFactory>();
            List<GenericConfigBase> configuredData = new()
            {
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                )
            };
            IGenericsConfigrationValidator validator = serviceProvider.GetRequiredService<IGenericsConfigrationValidator>();

            //act
            var result = validator.GenericArgumentCountMatchesType(typeof(GenericClass<,>), configuredData);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void GenericArgumentNameMismatchFailsForValidArguments()
        {
            //arrange
            IGenericConfigFactory genericConfigFactory = serviceProvider.GetRequiredService<IGenericConfigFactory>();
            List<string> configuredArgumentNames = new() { "A", "B" };
            List<GenericConfigBase> configuredData = new()
            {
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "B",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                )
            };
            IGenericsConfigrationValidator validator = serviceProvider.GetRequiredService<IGenericsConfigrationValidator>();

            //act
            var result = validator.GenericArgumentNameMismatch(configuredArgumentNames, configuredData);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void GenericArgumentNameMismatchWorksForMismatchedArgumentname()
        {
            //arrange
            IGenericConfigFactory genericConfigFactory = serviceProvider.GetRequiredService<IGenericConfigFactory>();
            List<string> configuredArgumentNames = new() { "A", "C" };
            List<GenericConfigBase> configuredData = new()
            {
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "A",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                ),
                genericConfigFactory.GetLiteralGenericConfig
                (
                    "B",
                    LiteralParameterType.String,
                    LiteralParameterInputStyle.SingleLineTextBox,
                    true,
                    false,
                    false,
                    "",
                    "",
                    "",
                    new List<string>()
                )
            };
            IGenericsConfigrationValidator validator = serviceProvider.GetRequiredService<IGenericsConfigrationValidator>();

            //act
            var result = validator.GenericArgumentNameMismatch(configuredArgumentNames, configuredData);

            //assert
            Assert.True(result);
        }

        class GenericClass<A, B>
        {
            public GenericClass(A aProperty, B bProperty)
            {
                AProperty = aProperty;
                BProperty = bProperty;
            }

            public A AProperty { get; set; }
            public B BProperty { get; set; }
        }
    }
}
