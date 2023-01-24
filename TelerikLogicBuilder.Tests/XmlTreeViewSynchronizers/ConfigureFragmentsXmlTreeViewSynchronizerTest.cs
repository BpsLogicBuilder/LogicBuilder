using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlTreeViewSynchronizers
{
    public class ConfigureFragmentsXmlTreeViewSynchronizerTest
    {
        public ConfigureFragmentsXmlTreeViewSynchronizerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureFragmentsXmlTreeViewSynchronizerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureFragmentsXmlTreeViewSynchronizer>());
        }

        [Fact]
        public void AddFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);

            //act
            synchronizer.AddFolder(configureFragmentsForm.TreeView.Nodes[0], "AFolder");
            synchronizer.AddFolder(configureFragmentsForm.TreeView.Nodes[0], "XFolder");

            //assert
            Assert.Equal("AFolder", configureFragmentsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("XFolder", configureFragmentsForm.TreeView.Nodes[0].Nodes[4].Text);
            Assert.Equal("/folder/folder[@name=\"AFolder\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("/folder/folder[@name=\"XFolder\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[4].Name);
            Assert.Equal("AFolder", ((XmlElement)configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"AFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("XFolder", ((XmlElement)configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"XFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddFragmentWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement newFragmentElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFragmentsForm.XmlDocument,
                GetXmlElement(@"<fragment name=""IntegerItem"">
                                  <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                    <genericArguments />
                                    <parameters>
                                      <literalParameter name=""parameterName"">$it</literalParameter>
                                    </parameters>
                                  </constructor>
                                </fragment>")
            );

            //act
            RadTreeNode newTreeNode = synchronizer.AddFragmentNode(configureFragmentsForm.TreeView.Nodes[0], newFragmentElement);
            //assert
            Assert.Equal(newTreeNode, configureFragmentsForm.TreeView.SelectedNode);
            Assert.Equal("IntegerItem", configureFragmentsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("/folder/fragment[@name=\"IntegerItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("IntegerItem", ((XmlElement)configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"IntegerItem\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddFragmentNodesWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            XmlElement firstFragmentElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFragmentsForm.XmlDocument,
                GetXmlElement(@"<fragment name=""IntegerItem"">
                                  <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                    <genericArguments />
                                    <parameters>
                                      <literalParameter name=""parameterName"">$it</literalParameter>
                                    </parameters>
                                  </constructor>
                                </fragment>")
            );

            XmlElement secondFragmentElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFragmentsForm.XmlDocument,
                GetXmlElement(@"<fragment name=""BooleanItem"">
                                  <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                    <genericArguments />
                                    <parameters>
                                      <literalParameter name=""parameterName"">$it</literalParameter>
                                    </parameters>
                                  </constructor>
                                </fragment>")
            );

            //act
            synchronizer.AddFragmentNodes
            (
                configureFragmentsForm.TreeView.Nodes[0],
                new XmlElement[] { firstFragmentElement, secondFragmentElement }
            );

            //assert
            Assert.Equal("BooleanItem", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("IntegerItem", configureFragmentsForm.TreeView.Nodes[0].Nodes[2].Text);
            Assert.Equal("/folder/fragment[@name=\"BooleanItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/fragment[@name=\"IntegerItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[2].Name);
            Assert.Equal("BooleanItem", ((XmlElement)configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"BooleanItem\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("IntegerItem", ((XmlElement)configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"IntegerItem\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void DeleteFragmentWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);

            //act
            synchronizer.DeleteNode(configureFragmentsForm.TreeView.Nodes[0].Nodes[0]);

            //assert
            Assert.Equal(2, configureFragmentsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"DecimalItem\"]"));
        }

        [Fact]
        public void DeleteFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);

            //act
            synchronizer.DeleteNode(configureFragmentsForm.TreeView.Nodes[0].Nodes[1]);

            //assert
            Assert.Equal(2, configureFragmentsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]"));
        }

        [Fact]
        public void MoveFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);
            RadTreeNode destinationFolder = configureFragmentsForm.TreeView.Nodes[0].Nodes[2];
            RadTreeNode movingFolder = configureFragmentsForm.TreeView.Nodes[0].Nodes[1];

            //act
            synchronizer.MoveFolderNode(destinationFolder, movingFolder);

            //assert
            Assert.Equal(2, configureFragmentsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]"));
            Assert.NotNull(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]"));
            Assert.Equal("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Name);
        }

        [Fact]
        public void MoveFoldersAndFragments()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);
            RadTreeNode destinationFolder = configureFragmentsForm.TreeView.Nodes[0].Nodes[2];
            IList<RadTreeNode> movingTreeNodes = new RadTreeNode[]
            {
                configureFragmentsForm.TreeView.Nodes[0].Nodes[0],
                configureFragmentsForm.TreeView.Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersAndFragments            (
                destinationFolder,
                movingTreeNodes
            );

            //assert
            Assert.Single(configureFragmentsForm.TreeView.Nodes[0].Nodes);
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"DecimalItem\"]"));
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]"));
            Assert.Equal("Objects", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("/folder/folder[@name=\"Objects\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/fragment[@name=\"DecimalItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/fragment[@name=\"Object\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.FILEIMAGEINDEX, configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].ImageIndex);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]/fragment[@name=\"StringItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].Name);
            Assert.Equal(3, configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveFragmentWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);
            RadTreeNode destinationFolder = configureFragmentsForm.TreeView.Nodes[0].Nodes[1];
            RadTreeNode movingFragmentNode = configureFragmentsForm.TreeView.Nodes[0].Nodes[0];

            //act
            synchronizer.MoveFragmentNode(destinationFolder, movingFragmentNode);

            //assert
            Assert.Equal(2, configureFragmentsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"DecimalItem\"]"));
            Assert.NotNull(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]/fragment[@name=\"DecimalItem\"]"));
            Assert.Equal("/folder/folder[@name=\"Literals\"]/fragment[@name=\"DecimalItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
        }

        [Fact]
        public void ReplaceFragmentWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFragmentsForm configureFragmentsForm = GetConfigureFragmentsForm();
            IConfigureFragmentsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFragmentsXmlTreeViewSynchronizer(configureFragmentsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            RadTreeNode existingFragmentNode = configureFragmentsForm.TreeView.Nodes[0].Nodes[0];
            XmlElement replacementFragmentElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFragmentsForm.XmlDocument,
                GetXmlElement(@"<fragment name=""IntegerItem"">
                                  <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                    <genericArguments />
                                    <parameters>
                                      <literalParameter name=""parameterName"">$it</literalParameter>
                                    </parameters>
                                  </constructor>
                                </fragment>")
            );

            //act
            synchronizer.ReplaceFragmentNode(existingFragmentNode, replacementFragmentElement);

            //assert
            Assert.Equal(3, configureFragmentsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"DecimalItem\"]"));
            Assert.NotNull(configureFragmentsForm.XmlDocument.SelectSingleNode("/folder/fragment[@name=\"IntegerItem\"]"));
            Assert.Equal("/folder/fragment[@name=\"IntegerItem\"]", configureFragmentsForm.TreeView.Nodes[0].Nodes[0].Name);
        }

        private IConfigureFragmentsForm GetConfigureFragmentsForm()
        {
            ITreeViewBuilderFactory treeViewBuilderFactory = serviceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.FragmentsSchema
            );
            RadTreeView radTreeView = new()
            {
                MultiSelect = true
            };
            treeViewXmlDocumentHelper.LoadXmlDocument(initialFragmentsXml);
            IConfigureFragmentsForm configureFragmentsForm = new ConfigureFragmentsFormMock
            (
                treeViewXmlDocumentHelper,
                radTreeView,
                treeViewXmlDocumentHelper.XmlTreeDocument,
                serviceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>()
            );
            treeViewBuilderFactory.GetConfigureFragmentsTreeViewBuilder(configureFragmentsForm).Build(radTreeView, treeViewXmlDocumentHelper.XmlTreeDocument);
            return configureFragmentsForm;
        }

        private static readonly string initialFragmentsXml = @"<folder name=""Decisions"">
                                                                    <fragment name=""DecimalItem"">
                                                                        <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                                                        <genericArguments />
                                                                        <parameters>
                                                                            <literalParameter name=""parameterName"">$it</literalParameter>
                                                                        </parameters>
                                                                        </constructor>
                                                                    </fragment>
                                                                    <folder name=""Literals"">
                                                                        <fragment name=""StringItem"">
                                                                          <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                                                            <genericArguments />
                                                                            <parameters>
                                                                              <literalParameter name=""parameterName"">$it</literalParameter>
                                                                            </parameters>
                                                                          </constructor>
                                                                        </fragment>
											                        </folder>
                                                                    <folder name=""Objects"">
                                                                        <fragment name=""Object"">
                                                                            <constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=County;title=County;placeholder=County (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;DropDownTemplateParameters: dropDownTemplate;fieldTypeSource=Enrollment.Domain.Entities.PersonalModel"">
                                                                                <genericArguments />
                                                                                <parameters>
                                                                                <literalParameter name=""field"">County</literalParameter>
                                                                                <literalParameter name=""title"">County</literalParameter>
                                                                                <literalParameter name=""placeholder"">County (required)</literalParameter>
                                                                                <literalParameter name=""stringFormat"">{0}</literalParameter>
                                                                                <literalParameter name=""fieldTypeSource"">Enrollment.Domain.Entities.PersonalModel</literalParameter>
                                                                                </parameters>
                                                                            </constructor>
                                                                        </fragment>
											                        </folder>
                                                                </folder>";

        private static XmlElement GetXmlElement(string xmlString)
        {
            return GetXmlDocument(xmlString).DocumentElement!;
        }

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }
}
