using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlTreeViewSynchronizers
{
    public class ConfigureFragmentsChildNodesRenamerTest
    {
        public ConfigureFragmentsChildNodesRenamerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureFragmentsChildNodesRenamerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureFragmentsChildNodesRenamer>());
        }

        [Fact]
        public void ConfigureFragmentsChildNodesRenamerWorks()
        {
            //arrange
            IConfigurationFormChildNodesRenamerFactory factory = serviceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = new ConfigureFragmentsFormMock();

            configureFragmentsForm.ExpandedNodes.Add("folderChild01", "folderChild01");
            RadTreeNode radTreeNode = GetInitialTreeNode();
            radTreeNode.Find(n => n.Name == "folderChild01").Expanded = true;
            radTreeNode.Name = "/folder";
            IConfigureFragmentsChildNodesRenamer service = factory.GetConfigureFragmentsChildNodesRenamer(configureFragmentsForm);

            //act
            service.RenameChildNodes(radTreeNode);

            //assert
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]", radTreeNode.Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/fragment[@name=\"grandchild0101\"]", radTreeNode.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/fragment[@name=\"child02\"]", radTreeNode.Nodes[1].Name);
            Assert.Equal("/folder/fragment[@name=\"child03\"]", radTreeNode.Nodes[2].Name);
            Assert.True(configureFragmentsForm.ExpandedNodes.ContainsKey("/folder/folder[@name=\"folderChild01\"]"));
        }

        private RadTreeNode GetInitialTreeNode()
        {
            return new
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
                                ImageIndex = ImageIndexes.FILEIMAGEINDEX
                            }
                        }
                    )
                    {
                        Name = "folderChild01",
                        ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    },
                    new RadTreeNode("child02", Array.Empty<RadTreeNode>())
                    {
                        Name = "child02",
                        ImageIndex = ImageIndexes.FILEIMAGEINDEX
                    },
                    new RadTreeNode("child03", Array.Empty<RadTreeNode>())
                    {
                        Name = "child03",
                        ImageIndex = ImageIndexes.FILEIMAGEINDEX
                    }
                }
            )
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
            };
        }
    }
}
