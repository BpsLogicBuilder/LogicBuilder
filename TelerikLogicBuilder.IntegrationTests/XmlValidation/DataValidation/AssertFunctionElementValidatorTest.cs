using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class AssertFunctionElementValidatorTest
    {
        public AssertFunctionElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateXmlValidator()
        {
            //arrange
            IAssertFunctionElementValidator xmlValidator = serviceProvider.GetRequiredService<IAssertFunctionElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
