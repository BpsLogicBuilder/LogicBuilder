using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class LiteralListVariableElementValidatorTest
    {
        public LiteralListVariableElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLiteralListVariableElementValidator()
        {
            //arrange
            ILiteralListVariableElementValidator xmlValidator = serviceProvider.GetRequiredService<ILiteralListVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
