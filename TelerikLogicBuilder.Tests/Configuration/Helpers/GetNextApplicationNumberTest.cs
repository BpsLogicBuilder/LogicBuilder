using ABIS.LogicBuilder.FlowBuilder.Configuration.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Helpers
{
    public class GetNextApplicationNumberTest
    {
        public GetNextApplicationNumberTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateGetNextApplicationNumber()
        {
            //arrange
            IGetNextApplicationNumber service = serviceProvider.GetRequiredService<IGetNextApplicationNumber>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetNextApplicationNumberReturnsTheExpectedValueForCompleteSequence()
        {
            //arrange
            IGetNextApplicationNumber service = serviceProvider.GetRequiredService<IGetNextApplicationNumber>();
            RadTreeNode parent = new()
            {
                ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Nodes =
                {
                    new RadTreeNode
                    {
                        Text = "App01",
                        ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                    },
                    new RadTreeNode
                    {
                        Text = "App02",
                        ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                    },
                    new RadTreeNode
                    {
                        Text = "App03",
                        ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    }
                }
            };

            //act
            int number = service.Get(parent);

            //assert
            Assert.Equal(4, number);
        }

        [Fact]
        public void GetNextApplicationNumberReturnsTheExpectedValueWithGapInSequence()
        {
            //arrange
            IGetNextApplicationNumber service = serviceProvider.GetRequiredService<IGetNextApplicationNumber>();
            RadTreeNode parent = new()
            {
                ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Nodes =
                {
                    new RadTreeNode
                    {
                        Text = "App01",
                        ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                    },
                    new RadTreeNode
                    {
                        Text = "App02",
                        ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                    },
                    new RadTreeNode
                    {
                        Text = "App04",
                        ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    }
                }
            };

            //act
            int number = service.Get(parent);

            //assert
            Assert.Equal(3, number);
        }

        [Fact]
        public void GetNextApplicationNumberThrowsForInvalidParentNode()
        {
            //arrange
            IGetNextApplicationNumber service = serviceProvider.GetRequiredService<IGetNextApplicationNumber>();
            RadTreeNode parent = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
                Nodes =
                {
                    new RadTreeNode
                    {
                        Text = "App01",
                        ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                    },
                    new RadTreeNode
                    {
                        Text = "App02",
                        ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                    },
                    new RadTreeNode
                    {
                        Text = "App03",
                        ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
                    }
                }
            };

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => service.Get(parent));
        }
    }
}
