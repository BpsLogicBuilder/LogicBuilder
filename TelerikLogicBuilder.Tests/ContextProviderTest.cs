using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ContextProviderTest
    {
        public ContextProviderTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateContextProvider()
        {
            //arrange
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();

            //assert
            Assert.NotNull(contextProvider);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
