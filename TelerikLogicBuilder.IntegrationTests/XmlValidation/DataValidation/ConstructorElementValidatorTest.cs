using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ConstructorElementValidatorTest
    {
        public ConstructorElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanConstructorElementValidator()
        {
            //arrange
            IConstructorElementValidator xmlValidator = serviceProvider.GetRequiredService<IConstructorElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
