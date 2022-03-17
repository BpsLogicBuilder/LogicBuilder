using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class DecisionsElementValidatorTest
    {
        public DecisionsElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanDecisionsElementValidator()
        {
            //arrange
            IDecisionsElementValidator xmlValidator = serviceProvider.GetRequiredService<IDecisionsElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
