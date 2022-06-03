using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
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
