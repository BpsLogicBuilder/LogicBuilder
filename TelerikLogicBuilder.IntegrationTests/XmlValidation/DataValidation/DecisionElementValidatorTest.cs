using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class DecisionElementValidatorTest
    {
        public DecisionElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanDecisionElementValidator()
        {
            //arrange
            IDecisionElementValidator xmlValidator = serviceProvider.GetRequiredService<IDecisionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
