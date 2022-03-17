using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectVariableElementValidatorTest
    {
        public ObjectVariableElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanObjectVariableElementValidator()
        {
            //arrange
            IObjectVariableElementValidator xmlValidator = serviceProvider.GetRequiredService<IObjectVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
