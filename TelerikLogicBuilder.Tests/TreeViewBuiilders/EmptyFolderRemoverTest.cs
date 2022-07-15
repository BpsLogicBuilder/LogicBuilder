using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class EmptyFolderRemoverTest
    {
        public EmptyFolderRemoverTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateEmptyFolderRemover()
        {
            //arrange
            IEmptyFolderRemover service = serviceProvider.GetRequiredService<IEmptyFolderRemover>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void RemovesEmptyChildFolders()
        {
            //arrange
            IEmptyFolderRemover service = serviceProvider.GetRequiredService<IEmptyFolderRemover>();
            RadTreeNode treeNode = new            
            (
                "Root",
                new RadTreeNode[]
                {
                    new RadTreeNode("folderChild01", Array.Empty<RadTreeNode>())
                    {
                        ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    },
                    new RadTreeNode
                    (
                        "folderChild02",
                        new RadTreeNode[]
                        {
                            new RadTreeNode("grandChild0201", Array.Empty<RadTreeNode>())
                            {
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

            service.RemoveEmptyFolders(treeNode);

            //assert
            Assert.Null(treeNode.Find(n => n.Text == "folderChild01"));
            Assert.Null(treeNode.Find(n => n.Text == "folderGrandChild0201"));
            Assert.NotNull(treeNode.Find(n => n.Text == "folderChild02"));
            Assert.NotNull(treeNode.Find(n => n.Text == "grandChild0201"));
        }
    }
}
