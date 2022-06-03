using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class SaveResourcesTest
    {
        public SaveResourcesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateSaveResources()
        {
            //arrange
            ISaveResources helper = serviceProvider.GetRequiredService<ISaveResources>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
