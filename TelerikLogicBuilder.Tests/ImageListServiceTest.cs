using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ImageListServiceTest
    {
        public ImageListServiceTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateTreeViewService()
        {
            //arrange
            IImageListService service = serviceProvider.GetRequiredService<IImageListService>();

            //assert
            Assert.NotNull(service);
        }
    }
}
