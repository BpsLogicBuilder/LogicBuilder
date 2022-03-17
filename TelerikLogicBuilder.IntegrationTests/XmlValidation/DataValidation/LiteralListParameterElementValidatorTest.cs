using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralListParameterElementValidatorTest
    {
        public LiteralListParameterElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLiteralListParameterElementValidator()
        {
            //arrange
            ILiteralListParameterElementValidator xmlValidator = serviceProvider.GetRequiredService<ILiteralListParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
