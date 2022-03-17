using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralParameterElementValidatorTest
    {
        public LiteralParameterElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLiteralParameterElementValidator()
        {
            //arrange
            ILiteralParameterElementValidator xmlValidator = serviceProvider.GetRequiredService<ILiteralParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
