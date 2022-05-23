using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator.RuleBuilders
{
    public class RulesAssemblerTest
    {
        public RulesAssemblerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateRulesAssembler()
        {
            //arrange
            IRulesAssembler helper = serviceProvider.GetRequiredService<IRulesAssembler>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
