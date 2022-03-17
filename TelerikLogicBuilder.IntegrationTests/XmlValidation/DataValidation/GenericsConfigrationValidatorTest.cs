using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
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
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            List<GenericConfigBase> configuredData = new List<GenericConfigBase>
            {
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
                ),
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
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
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            List<GenericConfigBase> configuredData = new List<GenericConfigBase>
            {
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
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
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            List<string> configuredArgumentNames = new List<string> { "A", "B" };
            List<GenericConfigBase> configuredData = new List<GenericConfigBase>
            {
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
                ),
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
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
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            List<string> configuredArgumentNames = new List<string> { "A", "C" };
            List<GenericConfigBase> configuredData = new List<GenericConfigBase>
            {
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
                ),
                new LiteralGenericConfig
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
                    new List<string>(),
                    contextProvider
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
