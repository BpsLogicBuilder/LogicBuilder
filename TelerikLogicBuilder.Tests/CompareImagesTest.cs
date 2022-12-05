using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class CompareImagesTest
    {
        public CompareImagesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateCompareImages()
        {
            //arrange
            ICompareImages service = serviceProvider.GetRequiredService<ICompareImages>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void AreEqualReturnsTrueForMatchingImages()
        {
            //arrange
            ICompareImages service = serviceProvider.GetRequiredService<ICompareImages>();

            //act
            var result = service.AreEqual(ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark);

            //assert
            Assert.True(result);
        }

        [Fact]
        public void AreEqualReturnsFalseForDifferentImages()
        {
            //arrange
            ICompareImages service = serviceProvider.GetRequiredService<ICompareImages>();

            //act
            var result = service.AreEqual(ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error);

            //assert
            Assert.False(result);
        }
    }
}
