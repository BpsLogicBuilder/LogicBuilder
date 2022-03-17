using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class VariableElementValidatorTest
    {
        public VariableElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanVariableElementValidator()
        {
            //arrange
            IVariableElementValidator xmlValidator = serviceProvider.GetRequiredService<IVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
