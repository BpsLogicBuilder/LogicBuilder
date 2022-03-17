using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class RetractFunctionElementValidatorTest
    {
        public RetractFunctionElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanRetractFunctionElementValidator()
        {
            //arrange
            IRetractFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IRetractFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
