using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class GetAllCheckedNodeNamesTest
    {
        public GetAllCheckedNodeNamesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateGetAllCheckedNodeNames()
        {
            //arrange
            IGetAllCheckedNodeNames service = serviceProvider.GetRequiredService<IGetAllCheckedNodeNames>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetsAllCheckedNodeNames()
        {
            //arrange
            IGetAllCheckedNodeNames service = serviceProvider.GetRequiredService<IGetAllCheckedNodeNames>();
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
                                ImageIndex = TreeNodeImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0101", Array.Empty<RadTreeNode>())
                            {
                                ImageIndex =TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX
                            },
                        }
                    )
                    {
                        Name = "folderChild01",
                        ImageIndex = TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX
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
                                ImageIndex = TreeNodeImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("grandChild0202", Array.Empty<RadTreeNode>())
                            {
                                Name = "grandChild0202",
                                CheckState = ToggleState.On,
                                ImageIndex = TreeNodeImageIndexes.VSDXFILEIMAGEINDEX
                            },
                            new RadTreeNode("folderGrandChild0201", Array.Empty<RadTreeNode>())
                            {
                                ImageIndex =TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX
                            },
                        }
                    )
                    {
                        ImageIndex = TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    }
                }
            )
            {
                ImageIndex = TreeNodeImageIndexes.PROJECTFOLDERIMAGEINDEX
            };


            var result = service.GetNames(treeNode);

            //assert
            Assert.Equal(2, result.Count);
            Assert.Contains("grandChild0101", result);
            Assert.Contains("grandChild0202", result);
            Assert.DoesNotContain("grandChild0201", result);
        }
    }
}
