using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class FunctionsElementValidatorTest
    {
        public FunctionsElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanFunctionsElementValidator()
        {
            //arrange
            IFunctionsElementValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionsElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
