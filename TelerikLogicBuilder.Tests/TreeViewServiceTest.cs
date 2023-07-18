using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
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

        [Fact]
        public void CanClearImageLists()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            IImageListService imageListService = serviceProvider.GetRequiredService<IImageListService>();
            RadTreeView radTreeView = new()
            {
                RadContextMenu = new RadContextMenu()
                {
                    ImageList = imageListService.ImageList,
                    Items =
                    {
                        new RadMenuSeparatorItem(),
                        new RadMenuItem ("Open"),
                        new RadMenuSeparatorItem()
                    }
                },
                ImageList = imageListService.ImageList
            };

            //act assert
            Assert.NotNull(radTreeView.ImageList);
            Assert.NotNull(radTreeView.RadContextMenu.ImageList);
            service.ClearImageLists(radTreeView);
            Assert.Null(radTreeView.ImageList);
            Assert.Null(radTreeView.RadContextMenu.ImageList);
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

        class TreeNodeComparer : IComparer<RadTreeNode>
        {
            readonly IServiceProvider serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();

            public int Compare(RadTreeNode? treeNodeA, RadTreeNode? treeNodeB)
            {
                if (treeNodeA == null || treeNodeB == null)
                    throw new InvalidOperationException("{58A0B075-2FA3-4935-84CC-F1AFDBFF69DC}");

                ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

                if ((service.IsApplicationNode(treeNodeA) && service.IsApplicationNode(treeNodeB))
                    || (service.IsFolderNode(treeNodeA) && service.IsFolderNode(treeNodeB)))
                    return string.Compare(treeNodeA.Text, treeNodeB.Text);
                else
                    return service.IsApplicationNode(treeNodeA) ? -1 : 1;
            }
        }

        private static readonly RadTreeNode[] nodeList = new RadTreeNode[]
        {
            new RadTreeNode
            {
                Text = "AFolder",
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            },
            new RadTreeNode
            {
                Text = "EFile",
                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
            },
            new RadTreeNode
            {
                Text = "AFile",
                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
            },
            new RadTreeNode
            {
                Text = "CFile",
                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
            },
            new RadTreeNode
            {
                Text = "EFolder",
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            }
        };

        [Fact]
        public void GetInsertPositionreturnsExpectedResultsForNewApplicationNode()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

            RadTreeNode radTreeNode = new()
            {
                Text = "BFile",
                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
            };

            //act
            int position = service.GetInsertPosition(nodeList, radTreeNode, new TreeNodeComparer());

            //
            Assert.Equal(1, position);
        }

        [Fact]
        public void GetInsertPositionreturnsExpectedResultsForNewFolderNode()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

            RadTreeNode radTreeNode = new()
            {
                Text = "BFolder",
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };

            //act
            int position = service.GetInsertPosition(nodeList, radTreeNode, new TreeNodeComparer());

            //
            Assert.Equal(4, position);
        }

        [Fact]
        public void GetInsertPositionreturnsExpectedResultsForExistingApplicationNode()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

            RadTreeNode radTreeNode = new()
            {
                Text = "CFile",
                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
            };

            //act
            int position = service.GetInsertPosition(nodeList, radTreeNode, new TreeNodeComparer());

            //
            Assert.Equal(1, position);
        }

        [Fact]
        public void GetInsertPositionreturnsExpectedResultsForExistingFolderNode()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();

            RadTreeNode radTreeNode = new()
            {
                Text = "AFolder",
                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
            };
            //act
            int position = service.GetInsertPosition(nodeList, radTreeNode, new TreeNodeComparer());

            //
            Assert.Equal(3, position);
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

        [Theory]
        [InlineData("AFile", "CFile")]
        [InlineData("CFile", "EFolder")]
        [InlineData("EFolder", "CFile")]
        public void GetClosestNodeForSelectionAfterDeleteReturnsExpectedSibling(string nodeToDelete, string expectedSelection)
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new()
            {
                Nodes =
                {
                    new RadTreeNode()
                    {
                        Text = "Parent",
                        ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
                        Nodes =
                        {
                            new RadTreeNode
                            {
                                Text = "AFile",
                                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                            },
                            new RadTreeNode
                            {
                                Text = "CFile",
                                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                            },
                            new RadTreeNode
                            {
                                Text = "EFolder",
                                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
                            }
                        }
                    }
                }
            };

            RadTreeNode radTreeNode = radTreeView.Find(n => n.Text == nodeToDelete);

            //act
            RadTreeNode closestNode = service.GetClosestNodeForSelectionAfterDelete(radTreeNode);

            //assert
            Assert.Equal(expectedSelection, closestNode.Text);
        }

        [Fact]
        public void GetClosestNodeForSelectionAfterDeleteReturnsParentIfThereAreNoSiblings()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new()
            {
                Nodes =
                {
                    new RadTreeNode()
                    {
                        Text = "Parent",
                        ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
                        Nodes =
                        {
                            new RadTreeNode
                            {
                                Text = "AFile",
                                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                            }
                        }
                    }
                }
            };

            RadTreeNode radTreeNode = radTreeView.Find(n => n.Text == "AFile");

            //act
            RadTreeNode closestNode = service.GetClosestNodeForSelectionAfterDelete(radTreeNode);

            //assert
            Assert.Equal("Parent", closestNode.Text);
        }

        [Fact]
        public void GetClosestNodeForSelectionAfterDeleteThrowsForRootNode()
        {
            //arrange
            ITreeViewService service = serviceProvider.GetRequiredService<ITreeViewService>();
            RadTreeView radTreeView = new()
            {
                Nodes =
                {
                    new RadTreeNode()
                    {
                        Text = "Parent",
                        ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX,
                        Nodes =
                        {
                            new RadTreeNode
                            {
                                Text = "AFile",
                                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                            },
                            new RadTreeNode
                            {
                                Text = "CFile",
                                ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                            },
                            new RadTreeNode
                            {
                                Text = "EFolder",
                                ImageIndex = ImageIndexes.OPENEDFOLDERIMAGEINDEX
                            }
                        }
                    }
                }
            };

            RadTreeNode radTreeNode = radTreeView.Find(n => n.Text == "Parent");

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => service.GetClosestNodeForSelectionAfterDelete(radTreeNode));
        }
    }
}
