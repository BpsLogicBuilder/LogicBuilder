using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectParameterElementValidatorTest
    {
        public ObjectParameterElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanObjectParameterElementValidator()
        {
            //arrange
            IObjectParameterElementValidator xmlValidator = serviceProvider.GetRequiredService<IObjectParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
