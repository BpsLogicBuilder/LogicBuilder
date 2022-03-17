using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlValidation.DataValidation
{
    public class MetaObjectElementValidatorTest
    {
        public MetaObjectElementValidatorTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanMetaObjectElementValidator()
        {
            //arrange
            IMetaObjectElementValidator xmlValidator = serviceProvider.GetRequiredService<IMetaObjectElementValidator>();

            //assert
            Assert.NotNull(xmlValidator);
        }
    }
}
