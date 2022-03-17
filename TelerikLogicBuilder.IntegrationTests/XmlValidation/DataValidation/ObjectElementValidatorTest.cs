using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectElementValidatorTest
    {
        public ObjectElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanObjectElementValidator()
        {
            //arrange
            IObjectElementValidator xmlValidator = serviceProvider.GetRequiredService<IObjectElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
