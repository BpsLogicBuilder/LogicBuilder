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
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CanCreateXmlValidator()
        {
            //arrange
            IXmlValidator xmlValidator = serviceProvider.GetRequiredService<IXmlValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
