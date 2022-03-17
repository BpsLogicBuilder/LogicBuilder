using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class FunctionParametersDataValidatorTest
    {
        public FunctionParametersDataValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanFunctionParametersDataValidator()
        {
            //arrange
            IFunctionParametersDataValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionParametersDataValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
