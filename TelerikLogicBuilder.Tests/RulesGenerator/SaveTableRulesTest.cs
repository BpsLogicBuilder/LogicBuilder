using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class SaveTableRulesTest
    {
        public SaveTableRulesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateSaveTableRules()
        {
            //arrange
            ISaveTableRules helper = serviceProvider.GetRequiredService<ISaveTableRules>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
