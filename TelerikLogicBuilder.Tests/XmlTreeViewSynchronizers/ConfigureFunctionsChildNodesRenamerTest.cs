using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
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
    public class ConfigureFunctionsChildNodesRenamerTest
    {
        public ConfigureFunctionsChildNodesRenamerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureFunctionsChildNodesRenamerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureFunctionsChildNodesRenamer>());
        }

        [Fact]
        public void ConfigureFunctionsChildNodesRenamerWorks()
        {
            //arrange
            IConfigurationFormChildNodesRenamerFactory factory = serviceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = new ConfigureFunctionsFormMock();
            configureFunctionsForm.ExpandedNodes.Add("folderChild01", "folderChild01");
            configureFunctionsForm.ExpandedNodes.Add("function02", "function02");
            RadTreeNode radTreeNode = GetInitialTreeNode();
            radTreeNode.Find(n => n.Name == "folderChild01").Expanded = true;
            radTreeNode.Find(n => n.Name == "function02").Expanded = true;
            radTreeNode.Name = "/folder";
            IConfigureFunctionsChildNodesRenamer service = factory.GetConfigureFunctionsChildNodesRenamer(configureFunctionsForm);

            //act
            service.RenameChildNodes(radTreeNode);

            //assert
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]", radTreeNode.Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/function[@name=\"function0101\"]", radTreeNode.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/function[@name=\"function0101\"]/parameters/literalParameter[@name=\"parameter010101\"]", radTreeNode.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"folderChild01\"]/function[@name=\"function0101\"]/parameters/literalParameter[@name=\"parameter010102\"]", radTreeNode.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("/folder/function[@name=\"function02\"]", radTreeNode.Nodes[1].Name);
            Assert.Equal("/folder/function[@name=\"function02\"]/parameters/literalParameter[@name=\"parameter0201\"]", radTreeNode.Nodes[1].Nodes[0].Name);
            Assert.Equal("/folder/function[@name=\"function02\"]/parameters/literalParameter[@name=\"parameter0202\"]", radTreeNode.Nodes[1].Nodes[1].Name);
            Assert.Equal("/folder/function[@name=\"function03\"]", radTreeNode.Nodes[2].Name);
            Assert.Equal("/folder/function[@name=\"function03\"]/parameters/literalParameter[@name=\"parameter0301\"]", radTreeNode.Nodes[2].Nodes[0].Name);
            Assert.Equal("/folder/function[@name=\"function03\"]/parameters/literalParameter[@name=\"parameter0302\"]", radTreeNode.Nodes[2].Nodes[1].Name);
            Assert.True(configureFunctionsForm.ExpandedNodes.ContainsKey("/folder/folder[@name=\"folderChild01\"]"));
            Assert.True(configureFunctionsForm.ExpandedNodes.ContainsKey("/folder/function[@name=\"function02\"]"));
            Assert.False(configureFunctionsForm.ExpandedNodes.ContainsKey("/folder/function[@name=\"function03\"]"));
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
                                "function0101",
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
                                Name = "function0101",
                                ImageIndex = ImageIndexes.METHODIMAGEINDEX
                            }
                        }
                    )
                    {
                        Name = "folderChild01",
                        ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    },
                    new RadTreeNode
                    (
                        "function02",
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
                        Name = "function02",
                        ImageIndex = ImageIndexes.METHODIMAGEINDEX
                    },
                    new RadTreeNode
                    (
                        "function03",
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
                        Name = "function02",
                        ImageIndex = ImageIndexes.METHODIMAGEINDEX
                    }
                }
            )
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX
            };
        }
    }
}
