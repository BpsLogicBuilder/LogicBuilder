using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class CheckSelectedTreeNodesTest
    {
        public CheckSelectedTreeNodesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateCheckSelectedTreeNodes()
        {
            //arrange
            ICheckSelectedTreeNodes service = serviceProvider.GetRequiredService<ICheckSelectedTreeNodes>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetsAllCheckedNodeNames()
        {
            //arrange
            ICheckSelectedTreeNodes service = serviceProvider.GetRequiredService<ICheckSelectedTreeNodes>();
            RadTreeNode treeNode = new
            (
                "Root",
                new RadTreeNode[]
                {
                    new RadTreeNode
                    (
                        "folderChild01",
                        new RadTreeNode[]
                        {
                            new RadTreeNode("grandchild0101", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0101",
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            }
                        }
                    )
                    {
                        Name = "folderChild01",
                        ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    },
                    new RadTreeNode
                    (
                        "folderChild02",
                        new RadTreeNode[]
                        {
                            new RadTreeNode("grandchild0201", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0201",
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("grandchild0202", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0202",
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0201", Array.Empty<RadTreeNode>())
                            {
                                ImageIndex =ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                            },
                        }
                    ),
                    new RadTreeNode
                    (
                        "folderChild03",
                        new RadTreeNode[]
                        {
                            new RadTreeNode("grandchild0301", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0301",
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0301", Array.Empty<RadTreeNode>())
                            {
                                ImageIndex =ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                            },
                        }
                    )
                    {
                        ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    }
                }
            )
            {
                ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX
            };
            RadTreeView radTreeView = new()
            {
                TriStateMode = true
            };
            radTreeView.Nodes.Add(treeNode);
            HashSet<string> nodesToCheck = new()
            {
                "grandchild0101",
                "grandchild0202",
                "grandchild0301"
            };

            //act
            service.CheckListedNodes(treeNode, nodesToCheck);

            //assert
            RadTreeNode grandChild0101 = treeNode.Find(n => n.Text == "grandchild0101");
            RadTreeNode grandChild0201 = treeNode.Find(n => n.Text == "grandchild0201");
            RadTreeNode grandChild0202 = treeNode.Find(n => n.Text == "grandchild0202");
            RadTreeNode grandChild0301 = treeNode.Find(n => n.Text == "grandchild0301");
            RadTreeNode folderChild01 = treeNode.Find(n => n.Text == "folderChild01");
            RadTreeNode folderChild02 = treeNode.Find(n => n.Text == "folderChild02");
            RadTreeNode folderChild03 = treeNode.Find(n => n.Text == "folderChild03");
            RadTreeNode folderGrandChild0301 = treeNode.Find(n => n.Text == "folderGrandChild0301");

            Assert.Equal(ToggleState.On, grandChild0101.CheckState);
            Assert.Equal(ToggleState.Off, grandChild0201.CheckState);
            Assert.Equal(ToggleState.On, grandChild0202.CheckState);
            Assert.Equal(ToggleState.On, grandChild0301.CheckState);
            Assert.Equal(ToggleState.On, folderChild01.CheckState);
            Assert.Equal(ToggleState.Indeterminate, folderChild03.CheckState);
            Assert.Equal(ToggleState.Indeterminate, folderChild02.CheckState);
            Assert.Equal(ToggleState.Off, folderGrandChild0301.CheckState);
            Assert.Equal(ToggleState.Indeterminate, treeNode.CheckState);
        }
    }
}
