using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class UpdateProjectPropertiesTest
    {
        public UpdateProjectPropertiesTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CanCreateUpdateProjectProperties()
        {
            //arrange
            IUpdateProjectProperties updateProjectProperties = serviceProvider.GetRequiredService<IUpdateProjectProperties>();

            //assert
            Assert.NotNull(updateProjectProperties);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
