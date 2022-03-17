using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ConditionsElementValidatorTest
    {
        public ConditionsElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanConditionsElementValidator()
        {
            //arrange
            IConditionsElementValidator xmlValidator = serviceProvider.GetRequiredService<IConditionsElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
