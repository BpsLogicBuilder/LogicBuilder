using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectListParameterElementValidatorTest
    {
        public ObjectListParameterElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanObjectListParameterElementValidator()
        {
            //arrange
            IObjectListParameterElementValidator xmlValidator = serviceProvider.GetRequiredService<IObjectListParameterElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
