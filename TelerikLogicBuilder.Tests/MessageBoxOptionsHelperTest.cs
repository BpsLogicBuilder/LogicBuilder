﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CanSetMessageBoxOptions()
        {
            //arrange
            IMessageBoxOptionsHelper helper = serviceProvider.GetRequiredService<IMessageBoxOptionsHelper>();

            //act
            helper.MessageBoxOptions = MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;

            //assert
            Assert.Equal(helper.MessageBoxOptions, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
