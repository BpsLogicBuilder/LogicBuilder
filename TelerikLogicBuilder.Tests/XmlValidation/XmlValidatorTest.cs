using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlValidation
{
    public class XmlValidatorTest
    {
        public XmlValidatorTest()
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
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
