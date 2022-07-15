using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class TreeViewServiceTest
    {
        public TreeViewServiceTest()
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
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

            //assert
            Assert.NotNull(service);
        }

        public static List<object[]> TreeNode_Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
                        },
                        false
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                        },
                        false
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(TreeNode_Data))]
        public void IsFolderNodeReturnsExpectedResult(RadTreeNode treeNode, bool isFolderNode)
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

            //act
            var result = service.IsFolderNode(treeNode);

            //assert
            Assert.Equal(isFolderNode, result);
        }

        [Fact]
        public void IsRootNodeReturnsTrueForRootNodeAndFalseForChildNode()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            parentNode.Nodes.Add(childNode);

            //act assert
            Assert.True(service.IsRootNode(parentNode));
            Assert.False(service.IsRootNode(childNode));
        }
    }
}
