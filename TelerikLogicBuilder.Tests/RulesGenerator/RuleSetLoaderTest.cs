using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class RuleSetLoaderTest
    {
        public RuleSetLoaderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateRuleSetLoader()
        {
            //arrange
            IRuleSetLoader helper = serviceProvider.GetRequiredService<IRuleSetLoader>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
