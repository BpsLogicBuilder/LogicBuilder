using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class FunctionGenericsConfigrationValidatorTest
    {
        public FunctionGenericsConfigrationValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanFunctionGenericsConfigrationValidator()
        {
            //arrange
            IFunctionGenericsConfigrationValidator xmlValidator = serviceProvider.GetRequiredService<IFunctionGenericsConfigrationValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
