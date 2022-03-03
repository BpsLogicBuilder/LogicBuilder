using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class ExceptionHelperTest
    {
        public ExceptionHelperTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanCreateExceptionHelper()
        {
            //arrange
            IExceptionHelper helper = serviceProvider.GetRequiredService<IExceptionHelper>();

            //assert
            Assert.NotNull(helper);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

    }
}
