using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ParameterElementValidatorTest
    {
        public ParameterElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanParameterElementValidator()
        {
            //arrange
            IParameterElementValidator xmlValidator = serviceProvider.GetRequiredService<IParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
