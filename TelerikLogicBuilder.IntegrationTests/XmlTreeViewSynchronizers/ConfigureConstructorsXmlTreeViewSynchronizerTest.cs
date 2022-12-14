using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.IntegrationTests.Constants;
using TelerikLogicBuilder.IntegrationTests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.XmlTreeViewSynchronizers
{
    public class ConfigureConstructorsXmlTreeViewSynchronizerTest : IClassFixture<ConfigureConstructorsXmlTreeViewSynchronizerFixture>
    {
        private readonly ConfigureConstructorsXmlTreeViewSynchronizerFixture _fixture;

        public ConfigureConstructorsXmlTreeViewSynchronizerTest(ConfigureConstructorsXmlTreeViewSynchronizerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateConfigureConstructorsXmlTreeViewSynchronizerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<IConfigureConstructorsXmlTreeViewSynchronizer>());
        }

        [Fact]
        public void AddConstructorWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement newConstructorElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureConstructorsForm.XmlDocument,
                GetXmlElement(@"<constructor name=""Object"" >
                                    <typeName>System.Object</typeName>
                                    <parameters>
                                        <literalParameter name=""aProperty"" >
                                            <literalType>String</literalType>
                                            <control>SingleLineTextBox</control>
                                            <optional>false</optional>
                                            <useForEquality>true</useForEquality>
                                            <useForHashCode>false</useForHashCode>
                                            <useForToString>true</useForToString>
                                            <propertySource />
                                            <propertySourceParameter />
                                            <defaultValue />
                                            <domain />
                                            <comments />
                                        </literalParameter>
                                    </parameters>
                                    <genericArguments />
                                    <summary></summary>
                                </constructor>")
            );

            //act
            StateImageRadTreeNode newTreeNode = synchronizer.AddConstructorNode((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0], newConstructorElement);

            //assert
            Assert.Equal(newTreeNode, configureConstructorsForm.TreeView.SelectedNode);
            Assert.True(compareImages.AreEqual(newTreeNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("Object", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("/form/constructor[@name=\"Object\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/constructor[@name=\"Object\"]/parameters/literalParameter[@name=\"aProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal("Object", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"Object\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("aProperty", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"Object\"]/parameters/literalParameter[@name=\"aProperty\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddConstructorNodesWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement firstConstructorElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureConstructorsForm.XmlDocument,
                GetXmlElement(@"<constructor name=""ErrorResponse"" >
                                    <typeName>Contoso.Test.Business.Responses.ErrorResponse</typeName>
                                    <parameters>
                                    </parameters>
                                    <genericArguments />
                                    <summary></summary>
                                </constructor>")
            );
            XmlElement secondConstructorElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureConstructorsForm.XmlDocument,
                GetXmlElement(@"<constructor name=""Object"" >
                                    <typeName>System.Object</typeName>
                                    <parameters>
                                    </parameters>
                                    <genericArguments />
                                    <summary></summary>
                                </constructor>")
            );

            //act
            synchronizer.AddConstructorNodes
            (
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0],
                new XmlElement[] { firstConstructorElement, secondConstructorElement }
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("ErrorResponse", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("Object", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("TestResponseA", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Text);
            Assert.Equal("/form/constructor[@name=\"ErrorResponse\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/constructor[@name=\"Object\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/constructor[@name=\"TestResponseA\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Name);
            Assert.Equal("ErrorResponse", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"ErrorResponse\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("Object", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"Object\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("TestResponseA", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseA\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.AddFolder((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0], "AFolder");
            synchronizer.AddFolder((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0], "XFolder");

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("AFolder", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("XFolder", configureConstructorsForm.TreeView.Nodes[0].Nodes[4].Text);
            Assert.Equal("/form/folder[@name=\"AFolder\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/folder[@name=\"XFolder\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[4].Name);
            Assert.Equal("AFolder", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"AFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("XFolder", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"XFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement newParameterElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureConstructorsForm.XmlDocument,
                GetXmlElement(@"<literalParameter name=""anotherStringProperty"" >
                                    <literalType>String</literalType>
                                    <control>SingleLineTextBox</control>
                                    <optional>false</optional>
                                    <useForEquality>true</useForEquality>
                                    <useForHashCode>false</useForHashCode>
                                    <useForToString>true</useForToString>
                                    <propertySource />
                                    <propertySourceParameter />
                                    <defaultValue />
                                    <domain />
                                    <comments />
                                </literalParameter>")
            );

            //act
            StateImageRadTreeNode newTreeNode = synchronizer.AddParameterNode((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[0], newParameterElement);

            //assert
            Assert.Equal(newTreeNode, configureConstructorsForm.TreeView.SelectedNode);
            Assert.True(compareImages.AreEqual(newTreeNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("anotherStringProperty", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Text);
            Assert.Equal("/form/constructor[@name=\"TestResponseA\"]/parameters/literalParameter[@name=\"anotherStringProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("anotherStringProperty", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseA\"]/parameters/literalParameter[@name=\"anotherStringProperty\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void DeleteConstructorWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteNode((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Empty(configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]"));
            Assert.NotNull(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]"));
        }

        [Fact]
        public void DeleteFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteNode((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder C\"]"));
        }

        [Fact]
        public void DeleteParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteParameterNode((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Single(configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"stringProperty\"]"));
            Assert.NotNull(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"intProperty\"]"));
        }

        [Fact]
        public void MoveConstructorNodeWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2];
            StateImageRadTreeNode movingTreeNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[0];

            //act
            synchronizer.MoveConstructorNode
            (
                destinationFolder,
                movingTreeNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseA\"]"));
            Assert.Equal("Folder C", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("/form/folder[@name=\"Folder C\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseA\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Name);
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes.Count);
        }

        [Fact]
        public void MoveFolderNodeWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2];
            StateImageRadTreeNode movingTreeNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1];

            //act
            synchronizer.MoveFolderNode
            (
                destinationFolder,
                movingTreeNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]"));
            Assert.Equal("Folder C", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("/form/folder[@name=\"Folder C\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Name);
            Assert.Equal(ImageIndexes.CONSTRUCTORIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Nodes[0].ImageIndex);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Nodes[0].Name);
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes.Count);
        }

        [Fact]
        public void MoveFoldersAndConstructors()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersConstructorsAndParameters
            (
                destinationFolder,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Single(configureConstructorsForm.TreeView.Nodes[0].Nodes);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseA\"]"));
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]"));
            Assert.Equal("Folder C", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("/form/folder[@name=\"Folder C\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseA\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.CONSTRUCTORIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].ImageIndex);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].Name);
            Assert.Equal(3, configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParametersToConstructorWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationConstructorNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersConstructorsAndParameters
            (
                destinationConstructorNode,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"stringProperty\"]"));
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"intProperty\"]"));
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/objectParameter[@name=\"objectProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/literalParameter[@name=\"stringProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/literalParameter[@name=\"intProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.OBJECTPARAMETERIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].ImageIndex);
            Assert.Equal(3, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParametersBeforeParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationParameterNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersConstructorsAndParameters
            (
                destinationParameterNode,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"stringProperty\"]"));
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"intProperty\"]"));
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/literalParameter[@name=\"stringProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/literalParameter[@name=\"intProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/objectParameter[@name=\"objectProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].ImageIndex);
            Assert.Equal(ImageIndexes.OBJECTPARAMETERIMAGEINDEX, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].ImageIndex);
            Assert.Equal(3, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParameterBeforeParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationParameterNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0];
            StateImageRadTreeNode movingParameterNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1];

            //act
            synchronizer.MoveParameterBeforeParameter
            (
                destinationParameterNode,
                movingParameterNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.NotNull(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"stringProperty\"]"));
            Assert.NotNull(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"intProperty\"]"));
            Assert.Equal("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"intProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"stringProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1].Name);
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParameterToConstructorWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationConstructorNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0];
            StateImageRadTreeNode movingParameterNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1];

            //act
            synchronizer.MoveParameterToConstructor
            (
                destinationConstructorNode,
                movingParameterNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.NotNull(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/literalParameter[@name=\"intProperty\"]"));
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/folder[@name=\"Folder B\"]/constructor[@name=\"TestResponseB\"]/parameters/literalParameter[@name=\"intProperty\"]"));
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/objectParameter[@name=\"objectProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].Name);
            Assert.Equal("/form/folder[@name=\"Folder C\"]/constructor[@name=\"TestResponseC\"]/parameters/literalParameter[@name=\"intProperty\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].Name);
            Assert.Single(configureConstructorsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes);
            Assert.Equal(2, configureConstructorsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void ReplaceConstructorWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureConstructorsForm configureConstructorsForm = GetConfigureConstructorsForm();
            IConfigureConstructorsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureConstructorsXmlTreeViewSynchronizer(configureConstructorsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode existingConstructorNode = (StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0].Nodes[0];
            XmlElement replacementConstructorElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureConstructorsForm.XmlDocument,
                GetXmlElement(@"<constructor name=""Object"" >
                                    <typeName>System.Object</typeName>
                                    <parameters>
                                    </parameters>
                                    <genericArguments />
                                    <summary></summary>
                                </constructor>")
            );

            //act
            synchronizer.ReplaceConstructorNode(existingConstructorNode, replacementConstructorElement);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureConstructorsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(3, configureConstructorsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"TestResponseA\"]"));
            Assert.NotNull(configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"Object\"]"));
            Assert.Equal("/form/constructor[@name=\"Object\"]", configureConstructorsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("Object", ((XmlElement)configureConstructorsForm.XmlDocument.SelectSingleNode("/form/constructor[@name=\"Object\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        private IConfigureConstructorsForm GetConfigureConstructorsForm()
        {
            ITreeViewBuilderFactory treeViewBuilderFactory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            IServiceFactory serviceFactory = _fixture.ServiceProvider.GetRequiredService<IServiceFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ConstructorSchema
            );
            RadTreeView radTreeView = new()
            {
                MultiSelect = true
            };
            treeViewXmlDocumentHelper.LoadXmlDocument(initialConstructorsXml);
            IConfigureConstructorsForm configureConstructorsForm = new ConfigureConstructorsFormMock
            (
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name),
                treeViewXmlDocumentHelper,
                radTreeView,
                treeViewXmlDocumentHelper.XmlTreeDocument,
                _fixture.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>()
            );
            treeViewBuilderFactory.GetConfigureConstructorsTreeViewBuilder(configureConstructorsForm).Build(radTreeView, treeViewXmlDocumentHelper.XmlTreeDocument);
            return configureConstructorsForm;
        }

        private static readonly string initialConstructorsXml = @"<form>
                                                                    <constructor name=""TestResponseA"" >
                                                                        <typeName>Contoso.Test.Business.Responses.TestResponseA</typeName>
                                                                        <parameters>
                                                                            <literalParameter name=""stringProperty"" >
                                                                                <literalType>String</literalType>
                                                                                <control>SingleLineTextBox</control>
                                                                                <optional>false</optional>
                                                                                <useForEquality>true</useForEquality>
                                                                                <useForHashCode>false</useForHashCode>
                                                                                <useForToString>true</useForToString>
                                                                                <propertySource />
                                                                                <propertySourceParameter />
                                                                                <defaultValue />
                                                                                <domain />
                                                                                <comments />
                                                                            </literalParameter>
                                                                        </parameters>
                                                                        <genericArguments />
                                                                        <summary>Updates the access the access after field.</summary>
                                                                    </constructor>
                                                                    <folder name=""Folder B"">
		                                                                <constructor name=""TestResponseB"" >
                                                                            <typeName>Contoso.Test.Business.Responses.TestResponseB</typeName>
                                                                            <parameters>
                                                                                <literalParameter name=""stringProperty"" >
                                                                                    <literalType>String</literalType>
                                                                                    <control>SingleLineTextBox</control>
                                                                                    <optional>false</optional>
                                                                                    <useForEquality>true</useForEquality>
                                                                                    <useForHashCode>false</useForHashCode>
                                                                                    <useForToString>true</useForToString>
                                                                                    <propertySource />
                                                                                    <propertySourceParameter />
                                                                                    <defaultValue />
                                                                                    <domain />
                                                                                    <comments />
                                                                                </literalParameter>
                                                                                <literalParameter name=""intProperty"" >
                                                                                    <literalType>Integer</literalType>
                                                                                    <control>SingleLineTextBox</control>
                                                                                    <optional>false</optional>
                                                                                    <useForEquality>true</useForEquality>
                                                                                    <useForHashCode>false</useForHashCode>
                                                                                    <useForToString>true</useForToString>
                                                                                    <propertySource />
                                                                                    <propertySourceParameter />
                                                                                    <defaultValue />
                                                                                    <domain />
                                                                                    <comments />
                                                                                </literalParameter>
                                                                            </parameters>
                                                                            <genericArguments />
                                                                            <summary>Updates the access the access after field.</summary>
                                                                        </constructor>
											                        </folder>
                                                                    <folder name=""Folder C"">
		                                                                <constructor name=""TestResponseC"" >
                                                                            <typeName>Contoso.Test.Business.Responses.TestResponseC</typeName>
                                                                            <parameters>
                                                                                <objectParameter name=""objectProperty"">
                                                                                    <objectType>Contoso.Test.Business.Responses.TestResponseA</objectType>
														                            <optional>false</optional>
                                                                                    <useForEquality>false</useForEquality>
                                                                                    <useForHashCode>false</useForHashCode>
                                                                                    <useForToString>true</useForToString>
                                             				                        <comments></comments>
                                                                                </objectParameter>
                                                                            </parameters>
                                                                            <genericArguments />
                                                                            <summary>Updates the access the access after field.</summary>
                                                                        </constructor>
											                        </folder>
                                                                </form>";

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

    public class ConfigureConstructorsXmlTreeViewSynchronizerFixture : IDisposable
    {
        public ConfigureConstructorsXmlTreeViewSynchronizerFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
            WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            EnumHelper = ServiceProvider.GetRequiredService<IEnumHelper>();
            TypeHelper = ServiceProvider.GetRequiredService<ITypeHelper>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            VariableFactory = ServiceProvider.GetRequiredService<IVariableFactory>();

            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
            );

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.VariableList = new VariableList
            (
                new Dictionary<string, VariableBase>
                {
                    ["VariableTypeNotLoaded"] = VariableFactory.GetObjectVariable
                    (
                        "VariableTypeNotLoaded",
                        "VariableTypeNotLoaded",
                        VariableCategory.StringKeyIndexer,
                        "",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "VariableTypeNotLoaded"
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        LiteralVariable GetLiteralVariable(string name, LiteralVariableType literalVariableType)
            => VariableFactory.GetLiteralVariable
            (
                name,
                name,
                VariableCategory.StringKeyIndexer,
                TypeHelper.ToId(EnumHelper.GetSystemType(literalVariableType)),
                "",
                "flowManager.FlowDataCache.Items",
                "Field.Property.Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                literalVariableType,
                LiteralVariableInputStyle.SingleLineTextBox,
                "",
                "",
                new List<string>()
            );

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        internal IServiceProvider ServiceProvider;
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
        internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IConstructorFactory ConstructorFactory;
        internal IEnumHelper EnumHelper;
        internal ITypeHelper TypeHelper;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IVariableFactory VariableFactory;
    }
}
