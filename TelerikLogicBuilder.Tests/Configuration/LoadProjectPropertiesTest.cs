using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class LoadProjectPropertiesTest
    {
        public LoadProjectPropertiesTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CanCreateLoadProjectProperties()
        {
            //arrange
            ILoadProjectProperties loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();

            //assert
            Assert.NotNull(loadProjectProperties);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
