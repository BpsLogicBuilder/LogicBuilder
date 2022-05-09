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
        public void CanCreateThemeManager()
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
                            ImageIndex = TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = TreeNodeImageIndexes.OPENEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = TreeNodeImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = TreeNodeImageIndexes.CUTOPENEDFOLDERIMAGEINDEX
                        },
                        true
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = TreeNodeImageIndexes.VISIOFILEIMAGEINDEX
                        },
                        false
                    },
                    new object[]
                    {
                        new RadTreeNode()
                        {
                            ImageIndex = TreeNodeImageIndexes.VSDXFILEIMAGEINDEX
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
                ImageIndex = TreeNodeImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode = new()
            {
                ImageIndex = TreeNodeImageIndexes.VISIOFILEIMAGEINDEX
            };
            parentNode.Nodes.Add(childNode);

            //act assert
            Assert.True(service.IsRootNode(parentNode));
            Assert.False(service.IsRootNode(childNode));
        }
    }
}
