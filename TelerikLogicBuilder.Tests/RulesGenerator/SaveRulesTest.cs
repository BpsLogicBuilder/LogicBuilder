using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class SaveRulesTest
    {
        public SaveRulesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateSaveRules()
        {
            //arrange
            ISaveRules helper = serviceProvider.GetRequiredService<ISaveRules>();

            //assert
            Assert.NotNull(helper);
        }
    }
}
