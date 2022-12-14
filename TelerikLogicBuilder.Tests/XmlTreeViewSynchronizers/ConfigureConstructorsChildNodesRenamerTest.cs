using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
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
    public class ConfigureConstructorsChildNodesRenamerTest
    {
        public ConfigureConstructorsChildNodesRenamerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureConstructorsChildNodesRenamerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureConstructorsChildNodesRenamer>());
        }

        [Fact]
        public void ConfigureConstructorsChildNodesRenamerWorks()
        {
            //arrange
            IConfigurationFormChildNodesRenamerFactory factory = serviceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = new ConfigureConstructorsFormMock();
            configureConstructorsForm.ExpandedNodes.Add("folderChild01", "folderChild01");
            configureConstructorsForm.ExpandedNodes.Add("constructor02", "constructor02");
            RadTreeNode radTreeNode = GetInitialTreeNode();
            radTreeNode.Find(n => n.Name == "folderChild01").Expanded = true;
            radTreeNode.Find(n => n.Name == "constructor02").Expanded = true;
            radTreeNode.Name = "/folder";
            IConfigureConstructorsChildNodesRenamer service = factory.GetConfigureConstructorsChildNodesRenamer(configureConstructorsForm);
            
            //act
            service.RenameChildNodes(radTreeNode);

            //assert
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]", radTreeNode.Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/constructor[@name=\"constructor0101\"]", radTreeNode.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/constructor[@name=\"constructor0101\"]/parameters/literalParameter[@name=\"parameter010101\"]", radTreeNode.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/constructor[@name=\"constructor0101\"]/parameters/literalParameter[@name=\"parameter010102\"]", radTreeNode.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("/folder/constructor[@name=\"constructor02\"]", radTreeNode.Nodes[1].Name);
            Assert.Equal("/folder/constructor[@name=\"constructor02\"]/parameters/literalParameter[@name=\"parameter0201\"]", radTreeNode.Nodes[1].Nodes[0].Name);
            Assert.Equal("/folder/constructor[@name=\"constructor02\"]/parameters/literalParameter[@name=\"parameter0202\"]", radTreeNode.Nodes[1].Nodes[1].Name);
            Assert.Equal("/folder/constructor[@name=\"constructor03\"]", radTreeNode.Nodes[2].Name);
            Assert.Equal("/folder/constructor[@name=\"constructor03\"]/parameters/literalParameter[@name=\"parameter0301\"]", radTreeNode.Nodes[2].Nodes[0].Name);
            Assert.Equal("/folder/constructor[@name=\"constructor03\"]/parameters/literalParameter[@name=\"parameter0302\"]", radTreeNode.Nodes[2].Nodes[1].Name);
            Assert.True(configureConstructorsForm.ExpandedNodes.ContainsKey("/folder/folder[@name=\"folderChild01\"]"));
            Assert.True(configureConstructorsForm.ExpandedNodes.ContainsKey("/folder/constructor[@name=\"constructor02\"]"));
            Assert.False(configureConstructorsForm.ExpandedNodes.ContainsKey("/folder/constructor[@name=\"constructor03\"]"));
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
                            new RadTreeNode
                            (
                                "constructor0101",
                                new RadTreeNode[]
                                {
                                    new RadTreeNode("parameter010101", Array.Empty<RadTreeNode>())
                                    {
                                        Name = "parameter010101",
                                        ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                                    },
                                    new RadTreeNode("parameter010102", Array.Empty<RadTreeNode>())
                                    {
                                        Name = "parameter010102",
                                        ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                                    }
                                }
                            )
                            {
                                Name = "constructor0101",
                                ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX
                            }
                        }
                    )
                    {
                        Name = "folderChild01",
                        ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    },
                    new RadTreeNode
                    (
                        "constructor02",
                        new RadTreeNode[]
                        {
                            new RadTreeNode("parameter0201", Array.Empty<RadTreeNode>())
                            {
                                Name = "parameter0201",
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                            },
                            new RadTreeNode("parameter0202", Array.Empty<RadTreeNode>())
                            {
                                Name = "parameter0202",
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                            }
                        }
                    )
                    {
                        Name = "constructor02",
                        ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX
                    },
                    new RadTreeNode
                    (
                        "constructor03",
                        new RadTreeNode[]
                        {
                            new RadTreeNode("parameter0301", Array.Empty<RadTreeNode>())
                            {
                                Name = "parameter0301",
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                            },
                            new RadTreeNode("parameter0302", Array.Empty<RadTreeNode>())
                            {
                                Name = "parameter0302",
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                            }
                        }
                    )
                    {
                        Name = "constructor02",
                        ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX
                    }
                }
            )
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
            };
        }
    }
}
