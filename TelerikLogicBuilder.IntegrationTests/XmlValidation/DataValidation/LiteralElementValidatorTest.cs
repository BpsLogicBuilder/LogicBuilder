using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralElementValidatorTest
    {
        public LiteralElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLiteralElementValidator()
        {
            //arrange
            ILiteralElementValidator xmlValidator = serviceProvider.GetRequiredService<ILiteralElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
