using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class MainWindowTest
    {
        public MainWindowTest()
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
            IMainWindow helper = serviceProvider.GetRequiredService<IMainWindow>();
            using Form form = new()
            {
                RightToLeft = RightToLeft.Yes
            };

            //act
            helper.Instance = form;

            //assert
            Assert.Equal(RightToLeft.Yes, helper.RightToLeft);
        }
    }
}
