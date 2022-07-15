using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class MessageBoxOptionsHelperTest
    {
        public MessageBoxOptionsHelperTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanSetMessageBoxOptions()
        {
            //arrange
            IMessageBoxOptionsHelper helper = serviceProvider.GetRequiredService<IMessageBoxOptionsHelper>();

            //act
            helper.RightToLeft = RightToLeft.Yes;

            //assert
            Assert.Equal(RightToLeft.Yes, helper.RightToLeft);
        }
    }
}
