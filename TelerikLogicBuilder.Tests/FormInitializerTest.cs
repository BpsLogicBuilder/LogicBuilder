using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class FormInitializerTest
    {
        public FormInitializerTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanSetCenterScreen()
        {
            //arrange
            IFormInitializer helper = serviceProvider.GetRequiredService<IFormInitializer>();
            using Form form = new();
            form.Size = new Size(100, 100);
            form.Location = new Point(0, 0);
            Point initialLocation = form.Location;

            //act
            helper.SetCenterScreen(form);

            //assert
            Assert.NotEqual(initialLocation.X, form.Location.X);
            Assert.NotEqual(initialLocation.Y, form.Location.X);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields
    }
}
