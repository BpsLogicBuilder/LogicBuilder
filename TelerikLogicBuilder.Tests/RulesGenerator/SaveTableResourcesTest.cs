using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class SaveTableResourcesTest
    {
        public SaveTableResourcesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateSaveTableResources()
        {
            //arrange
            ISaveTableResources helper = serviceProvider.GetRequiredService<ISaveTableResources>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
