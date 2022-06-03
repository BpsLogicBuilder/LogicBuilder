using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class SaveDiagramResourcesTest
    {
        public SaveDiagramResourcesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateSaveDiagramResources()
        {
            //arrange
            ISaveDiagramResources helper = serviceProvider.GetRequiredService<ISaveDiagramResources>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
