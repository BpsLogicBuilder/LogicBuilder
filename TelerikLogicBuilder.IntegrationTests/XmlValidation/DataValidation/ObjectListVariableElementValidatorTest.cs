using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class ObjectListVariableElementValidatorTest
    {
        public ObjectListVariableElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanObjectListVariableElementValidator()
        {
            //arrange
            IObjectListVariableElementValidator xmlValidator = serviceProvider.GetRequiredService<IObjectListVariableElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
