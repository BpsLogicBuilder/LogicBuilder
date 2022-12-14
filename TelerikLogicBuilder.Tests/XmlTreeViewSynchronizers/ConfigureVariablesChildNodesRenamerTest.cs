using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
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
    public class ConfigureVariablesChildNodesRenamerTest
    {
        public ConfigureVariablesChildNodesRenamerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureVariablesChildNodesRenamerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureVariablesChildNodesRenamer>());
        }

        [Fact]
        public void ConfigureVariablesChildNodesRenamerWorks()
        {
            //arrange
            IConfigurationFormChildNodesRenamerFactory factory = serviceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>();
            IConfigureVariablesForm configureVariablesForm = new ConfigureVariablesFormMock();
            configureVariablesForm.ExpandedNodes.Add("folderChild01", "folderChild01");
            RadTreeNode radTreeNode = GetInitialTreeNode();
            radTreeNode.Find(n => n.Name == "folderChild01").Expanded = true;
            radTreeNode.Name = "/folder";

            IConfigureVariablesChildNodesRenamer service = factory.GetConfigureVariablesChildNodesRenamer(configureVariablesForm);
            
            //act
            service.RenameChildNodes(radTreeNode);

            //assert
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]", radTreeNode.Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/literalVariable[@name=\"grandchild0101\"]", radTreeNode.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/literalVariable[@name=\"child02\"]", radTreeNode.Nodes[1].Name);
            Assert.Equal("/folder/literalVariable[@name=\"child03\"]", radTreeNode.Nodes[2].Name);
            Assert.True(configureVariablesForm.ExpandedNodes.ContainsKey("/folder/folder[@name=\"folderChild01\"]"));
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
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
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
                        ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                    },
                    new RadTreeNode("child03", Array.Empty<RadTreeNode>())
                    {
                        Name = "child03",
                        ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                    }
                }
            )
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
            };
        }
    }
}
