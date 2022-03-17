using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class XmlElementValidatorTest
    {
        public XmlElementValidatorTest()
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
            IXmlElementValidator xmlValidator = serviceProvider.GetRequiredService<IXmlElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
