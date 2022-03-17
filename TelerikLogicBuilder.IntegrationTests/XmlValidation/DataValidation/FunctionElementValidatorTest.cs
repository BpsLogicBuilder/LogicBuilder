using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class FunctionElementValidatorTest
    {
        public FunctionElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanFunctionElementValidator()
        {
            //arrange
            IFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
