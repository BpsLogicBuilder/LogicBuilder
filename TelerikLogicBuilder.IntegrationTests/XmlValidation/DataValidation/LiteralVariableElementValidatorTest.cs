using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralVariableElementValidatorTest
    {
        public LiteralVariableElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLiteralVariableElementValidator()
        {
            //arrange
            ILiteralVariableElementValidator xmlValidator = serviceProvider.GetRequiredService<ILiteralVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
