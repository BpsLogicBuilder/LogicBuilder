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

        [Fact]
        public void GetSelectedNodesReturnsEmptyListForNoNodesSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };
            
            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);

            //act assert
            Assert.Null(radTreeView.SelectedNode);
            Assert.Empty(radTreeView.SelectedNodes);
            Assert.Empty(service.GetSelectedNodes(radTreeView));
        }

        [Fact]
        public void GetSelectedNodesReturnsExpectedResultsForSelectedNodesEmptyAndSelectedNodeNotNull()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);
            radTreeView.SelectedNode = childNode1;
            childNode1.Selected = false;

            //act assert
            Assert.NotNull(radTreeView.SelectedNode);
            Assert.Empty(radTreeView.SelectedNodes);
            Assert.Single(service.GetSelectedNodes(radTreeView));
        }

        [Fact]
        public void GetSelectedNodesReturnsExpectedResultsForSelectedNodesNotEmptyAndSelectedNodeNull()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);
            childNode1.Selected = true;
            childNode2.Selected = true;
            radTreeView.SelectedNode = null;

            //act assert
            Assert.Null(radTreeView.SelectedNode);
            Assert.Equal(2, radTreeView.SelectedNodes.Count);
            Assert.Equal(2, service.GetSelectedNodes(radTreeView).Count);
        }

        [Fact]
        public void GetSelectedNodesReturnsOnlyTheParentNodeIfParentAndChildAreSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode folderNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
            };
            RadTreeNode childNode3 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            parentNode.Nodes.Add(folderNode);
            folderNode.Nodes.Add(childNode3);
            radTreeView.Nodes.Add(parentNode);

            folderNode.Selected = true;
            childNode3.Selected = true;

            //act assert
            Assert.Null(radTreeView.SelectedNode);
            Assert.Equal(2, radTreeView.SelectedNodes.Count);
            Assert.Single(service.GetSelectedNodes(radTreeView));
        }

        [Fact]
        public void GetSelectedNodesReturnsOnlyTheRootNodeIfAllNodesAreSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode folderNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
            };
            RadTreeNode childNode3 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            parentNode.Nodes.Add(folderNode);
            folderNode.Nodes.Add(childNode3);
            radTreeView.Nodes.Add(parentNode);

            parentNode.Selected = true;
            childNode1.Selected = true;
            childNode2.Selected = true;
            folderNode.Selected = true;
            childNode3.Selected = true;

            //act assert
            Assert.Null(radTreeView.SelectedNode);
            Assert.Equal(5, radTreeView.SelectedNodes.Count);
            Assert.Single(service.GetSelectedNodes(radTreeView));
        }

        [Fact]
        public void CollectionIncludesNodeAndDescendantReturnsTrueIfRootAndAnotherNodeAreSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);

            parentNode.Selected = true;
            childNode2.Selected = true;

            //act assert
            Assert.Null(radTreeView.SelectedNode);
            Assert.Equal(2, radTreeView.SelectedNodes.Count);
            Assert.True(service.CollectionIncludesNodeAndDescendant(radTreeView.SelectedNodes));
        }

        [Fact]
        public void CollectionIncludesNodeAndDescendantReturnsTrueIfRootAndChildOfFolderNodeAreSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode folderNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
            };
            RadTreeNode childNode3 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            parentNode.Nodes.Add(folderNode);
            folderNode.Nodes.Add(childNode3);
            radTreeView.Nodes.Add(parentNode);

            parentNode.Selected = true;
            childNode3.Selected = true;

            //act assert
            Assert.Null(radTreeView.SelectedNode);
            Assert.Equal(2, radTreeView.SelectedNodes.Count);
            Assert.True(service.CollectionIncludesNodeAndDescendant(radTreeView.SelectedNodes));
        }

        [Fact]
        public void CollectionIncludesNodeAndDescendantReturnsFalseWithMultipleNodesSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);

            childNode1.Selected = true;
            childNode2.Selected = true;

            //act assert
            Assert.Equal(2, radTreeView.SelectedNodes.Count);
            Assert.False(service.CollectionIncludesNodeAndDescendant(radTreeView.SelectedNodes));
        }

        [Fact]
        public void CollectionIncludesNodeAndDescendantReturnsFalseWithOnlyTheRootNodeSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);

            parentNode.Selected = true;

            //act assert
            Assert.Single(radTreeView.SelectedNodes);
            Assert.False(service.CollectionIncludesNodeAndDescendant(radTreeView.SelectedNodes));
        }

        [Fact]
        public void CollectionIncludesNodeAndDescendantReturnsFalseWithOnlyOneChildNodeSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);

            childNode1.Selected = true;

            //act assert
            Assert.Single(radTreeView.SelectedNodes);
            Assert.False(service.CollectionIncludesNodeAndDescendant(radTreeView.SelectedNodes));
        }

        [Fact]
        public void CollectionIncludesNodeAndDescendantReturnsFalseWithNoNodesSelected()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new() { MultiSelect = true };

            RadTreeNode parentNode = new()
            {
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            RadTreeNode childNode1 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };
            RadTreeNode childNode2 = new()
            {
                ImageIndex = ImageIndexes.VISIOFILEIMAGEINDEX
            };

            parentNode.Nodes.Add(childNode1);
            parentNode.Nodes.Add(childNode2);
            radTreeView.Nodes.Add(parentNode);

            //act assert
            Assert.Empty(radTreeView.SelectedNodes);
            Assert.False(service.CollectionIncludesNodeAndDescendant(radTreeView.SelectedNodes));
        }
    }
}
