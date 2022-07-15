using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class GetAllCheckedNodesTest
    {
        public GetAllCheckedNodesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateGetAllCheckedNodes()
        {
            //arrange
            IGetAllCheckedNodes service = serviceProvider.GetRequiredService<IGetAllCheckedNodes>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetsAllCheckedNodeNames()
        {
            //arrange
            IGetAllCheckedNodes service = serviceProvider.GetRequiredService<IGetAllCheckedNodes>();
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
                            new RadTreeNode("grandChild0101", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0101",
                                CheckState = ToggleState.On,
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0101", Array.Empty<RadTreeNode>())
                            {
                                ImageIndex =ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                            },
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
                            new RadTreeNode("grandChild0201", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0201",
                                CheckState = ToggleState.Off,
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("grandChild0202", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0202",
                                CheckState = ToggleState.On,
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0201", Array.Empty<RadTreeNode>())
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


            var result = service.GetNames(treeNode);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Contains("grandChild0101", result);
            Assert.Contains("grandChild0202", result);
            Assert.DoesNotContain("grandChild0201", result);
        }

        [Fact]
        public void GetsAllCheckedNodes()
        {
            //arrange
            IGetAllCheckedNodes service = serviceProvider.GetRequiredService<IGetAllCheckedNodes>();
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
                            new RadTreeNode("grandChild0101", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0101",
                                CheckState = ToggleState.On,
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0101", Array.Empty<RadTreeNode>())
                            {
                                ImageIndex =ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                            },
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
                            new RadTreeNode("grandChild0201", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0201",
                                CheckState = ToggleState.Off,
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("grandChild0202", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0202",
                                CheckState = ToggleState.On,
                                ImageIndex = ImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0201", Array.Empty<RadTreeNode>())
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


            var result = service.GetNodes(treeNode);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Contains("grandChild0101", result.Select(n => n.Name));
            Assert.Contains("grandChild0202", result.Select(n => n.Name));
            Assert.DoesNotContain("grandChild0201", result.Select(n => n.Name));
        }
    }
}
