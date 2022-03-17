using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralListElementValidatorTest
    {
        public LiteralListElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLiteralListElementValidator()
        {
            //arrange
            ILiteralListElementValidator xmlValidator = serviceProvider.GetRequiredService<ILiteralListElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
