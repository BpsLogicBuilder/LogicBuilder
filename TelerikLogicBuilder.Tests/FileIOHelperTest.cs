using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class FileIOHelperTest
    {
        public FileIOHelperTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanCreateFileIOHelper()
        {
            //arrange
            IFileIOHelper helper = serviceProvider.GetRequiredService<IFileIOHelper>();

            //assert
            Assert.NotNull(helper);
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields
    }
}
