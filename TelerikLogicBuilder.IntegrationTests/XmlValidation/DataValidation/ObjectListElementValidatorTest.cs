using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectListElementValidatorTest
    {
        public ObjectListElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanObjectListElementValidator()
        {
            //arrange
            IObjectListElementValidator xmlValidator = serviceProvider.GetRequiredService<IObjectListElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
