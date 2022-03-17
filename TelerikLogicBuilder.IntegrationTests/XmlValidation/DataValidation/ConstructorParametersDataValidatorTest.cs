using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ConstructorParametersDataValidatorTest
    {
        public ConstructorParametersDataValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanConstructorParametersDataValidator()
        {
            //arrange
            IConstructorParametersDataValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorParametersDataValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
