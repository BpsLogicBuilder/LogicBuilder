using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;
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

        [Fact]
        public void CanSetFormDefaults()
        {
            //arrange
            IFormInitializer helper = serviceProvider.GetRequiredService<IFormInitializer>();
            using Form form = new();
            form.Size = new Size(100, 100);

            //act
            helper.SetFormDefaults(form, 200);

            //assert
            Assert.Equal(200, form.MinimumSize.Height);
            Assert.False(form.MaximizeBox);
            Assert.False(form.MinimizeBox);
        }

        [Fact]
        public void CanSetProgressFormDefaults()
        {
            //arrange
            IFormInitializer helper = serviceProvider.GetRequiredService<IFormInitializer>();
            using Form form = new();
            form.Size = new Size(100, 100);

            //act
            helper.SetProgressFormDefaults(form, 200);

            //assert
            Assert.Equal(200, form.MinimumSize.Height);
            Assert.False(form.MaximizeBox);
            Assert.False(form.MinimizeBox);
            Assert.False(form.ControlBox);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields
    }
}
