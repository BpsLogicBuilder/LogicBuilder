using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class SaveDiagramRulesTest
    {
        public SaveDiagramRulesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateSaveDiagramRules()
        {
            //arrange
            ISaveDiagramRules helper = serviceProvider.GetRequiredService<ISaveDiagramRules>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
