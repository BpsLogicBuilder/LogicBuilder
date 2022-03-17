using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ConnectorElementValidatorTest
    {
        public ConnectorElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanConnectorElementValidator()
        {
            //arrange
            IConnectorElementValidator xmlValidator = serviceProvider.GetRequiredService<IConnectorElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
