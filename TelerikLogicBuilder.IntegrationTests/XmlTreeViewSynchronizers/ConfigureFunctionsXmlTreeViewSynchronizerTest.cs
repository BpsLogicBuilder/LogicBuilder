using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
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
    public class ConfigureFunctionsXmlTreeViewSynchronizerTest : IClassFixture<ConfigureFunctionsXmlTreeViewSynchronizerFixture>
    {
        private readonly ConfigureFunctionsXmlTreeViewSynchronizerFixture _fixture;

        public ConfigureFunctionsXmlTreeViewSynchronizerTest(ConfigureFunctionsXmlTreeViewSynchronizerFixture fixture)
        {
            _fixture = fixture;
        }

        readonly string rootXPath = $"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.FUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";

        [Fact]
        public void CreateConfigureFunctionsXmlTreeViewSynchronizerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsXmlTreeViewSynchronizer>());
        }

        [Fact]
        public void AddFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.AddFolder((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0], "AFolder");
            synchronizer.AddFolder((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0], "XFolder");

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("AFolder", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("XFolder", configureFunctionsForm.TreeView.Nodes[0].Nodes[4].Text);
            Assert.Equal($"{rootXPath}/folder[@name=\"AFolder\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"XFolder\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[4].Name);
            Assert.Equal("AFolder", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"AFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("XFolder", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"XFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddFunctionWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement newFunctionElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFunctionsForm.XmlDocument,
                GetXmlElement(@"<function name=""IsNullOrEmpty"" >
                                    <memberName>IsNullOrEmpty</memberName>
                                    <functionCategory>Standard</functionCategory>
                                    <typeName>System.String</typeName>
                                    <referenceName></referenceName>
                                    <referenceDefinition></referenceDefinition>
                                    <castReferenceAs />
                                    <referenceCategory>Type</referenceCategory>
                                    <parametersLayout>Sequential</parametersLayout>
                                    <parameters>
                                        <literalParameter name=""str"" >
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
                                    <returnType>
                                        <literal>
                                        <literalType>Boolean</literalType>
                                        </literal>
                                    </returnType>
                                    <summary></summary>
                                </function>")
            );

            //act
            StateImageRadTreeNode newTreeNode = synchronizer.AddFunctionNode((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0], newFunctionElement);

            //assert
            Assert.Equal(newTreeNode, configureFunctionsForm.TreeView.SelectedNode);
            Assert.True(compareImages.AreEqual(newTreeNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("IsNullOrEmpty", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]/parameters/literalParameter[@name=\"str\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Name);
            Assert.Equal("IsNullOrEmpty", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("str", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]/parameters/literalParameter[@name=\"str\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddFunctionNodesWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement firstFunctionElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFunctionsForm.XmlDocument,
                GetXmlElement(@"<function name=""IsNullOrEmpty"" >
                                    <memberName>IsNullOrEmpty</memberName>
                                    <functionCategory>Standard</functionCategory>
                                    <typeName>System.String</typeName>
                                    <referenceName></referenceName>
                                    <referenceDefinition></referenceDefinition>
                                    <castReferenceAs />
                                    <referenceCategory>Type</referenceCategory>
                                    <parametersLayout>Sequential</parametersLayout>
                                    <parameters>
                                        <literalParameter name=""str"" >
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
                                    <returnType>
                                        <literal>
                                        <literalType>Boolean</literalType>
                                        </literal>
                                    </returnType>
                                    <summary></summary>
                                </function>")
            );

            XmlElement secondFunctionElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFunctionsForm.XmlDocument,
                GetXmlElement(@"<function name=""IsNullOrWhiteSpace"" >
                                    <memberName>IsNullOrWhiteSpace</memberName>
                                    <functionCategory>Standard</functionCategory>
                                    <typeName>System.String</typeName>
                                    <referenceName></referenceName>
                                    <referenceDefinition></referenceDefinition>
                                    <castReferenceAs />
                                    <referenceCategory>Type</referenceCategory>
                                    <parametersLayout>Sequential</parametersLayout>
                                    <parameters>
                                        <literalParameter name=""str"" >
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
                                    <returnType>
                                        <literal>
                                        <literalType>Boolean</literalType>
                                        </literal>
                                    </returnType>
                                    <summary></summary>
                                </function>")
            );

            //act
            synchronizer.AddFunctionNodes
            (
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0],
                new XmlElement[] { firstFunctionElement, secondFunctionElement }
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("Copy", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("IsNullOrEmpty", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("IsNullOrWhiteSpace", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Text);
            Assert.Equal($"{rootXPath}/function[@name=\"Copy\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/function[@name=\"IsNullOrWhiteSpace\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Name);
            Assert.Equal("Copy", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"Copy\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("IsNullOrEmpty", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("IsNullOrWhiteSpace", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"IsNullOrWhiteSpace\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement newParameterElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFunctionsForm.XmlDocument,
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
            StateImageRadTreeNode newTreeNode = synchronizer.AddParameterNode((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[0], newParameterElement);

            //assert
            Assert.Equal(newTreeNode, configureFunctionsForm.TreeView.SelectedNode);
            Assert.True(compareImages.AreEqual(newTreeNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("anotherStringProperty", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Text);
            Assert.Equal($"{rootXPath}/function[@name=\"Copy\"]/parameters/literalParameter[@name=\"anotherStringProperty\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("anotherStringProperty", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"Copy\"]/parameters/literalParameter[@name=\"anotherStringProperty\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void DeleteFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteNode((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder C\"]"));
        }

        [Fact]
        public void DeleteFunctionWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteNode((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Empty(configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]"));
            Assert.NotNull(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]"));
        }

        [Fact]
        public void DeleteParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteParameterNode((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Single(configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg1\"]"));
            Assert.NotNull(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg2\"]"));
        }

        [Fact]
        public void MoveFunctionNodeWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2];
            StateImageRadTreeNode movingTreeNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[0];

            //act
            synchronizer.MoveFunctionNode
            (
                destinationFolder,
                movingTreeNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"Copy\"]"));
            Assert.Equal("Folder C", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Copy\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Name);
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes.Count);
        }

        [Fact]
        public void MoveFolderNodeWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2];
            StateImageRadTreeNode movingTreeNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1];

            //act
            synchronizer.MoveFolderNode
            (
                destinationFolder,
                movingTreeNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]"));
            Assert.Equal("Folder C", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Name);
            Assert.Equal(ImageIndexes.METHODIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Nodes[0].ImageIndex);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Nodes[0].Name);
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes.Count);
        }

        [Fact]
        public void MoveFoldersAndFunctions()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersFunctionsAndParameters
            (
                destinationFolder,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Single(configureFunctionsForm.TreeView.Nodes[0].Nodes);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"Copy\"]"));
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]"));
            Assert.Equal("Folder C", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Copy\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.METHODIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].ImageIndex);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].Name);
            Assert.Equal(3, configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParametersToFunctionWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFunctionNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersFunctionsAndParameters
            (
                destinationFunctionNode,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg1\"]"));
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg2\"]"));
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"str\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"arg1\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"arg2\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].ImageIndex);
            Assert.Equal(3, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParametersBeforeParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationParameterNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersFunctionsAndParameters
            (
                destinationParameterNode,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg1\"]"));
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg2\"]"));
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"arg1\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"arg2\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"str\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[2].ImageIndex);
            Assert.Equal(3, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParameterBeforeParameterWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationParameterNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0];
            StateImageRadTreeNode movingParameterNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1];

            //act
            synchronizer.MoveParameterBeforeParameter
            (
                destinationParameterNode,
                movingParameterNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.NotNull(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg1\"]"));
            Assert.NotNull(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg2\"]"));
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg2\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg1\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1].Name);
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveParameterToFunctionWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFunctionNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0];
            StateImageRadTreeNode movingParameterNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes[1];

            //act
            synchronizer.MoveParameterToFunction
            (
                destinationFunctionNode,
                movingParameterNode
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.NotNull(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"arg2\"]"));
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/folder[@name=\"Folder B\"]/function[@name=\"StaticVoidMethod B\"]/parameters/literalParameter[@name=\"arg2\"]"));
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"str\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[0].Name);
            Assert.Equal($"{rootXPath}/folder[@name=\"Folder C\"]/function[@name=\"Intern C\"]/parameters/literalParameter[@name=\"arg2\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes[1].Name);
            Assert.Single(configureFunctionsForm.TreeView.Nodes[0].Nodes[1].Nodes[0].Nodes);
            Assert.Equal(2, configureFunctionsForm.TreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void ReplaceFunctionWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureFunctionsForm configureFunctionsForm = GetConfigureFunctionsForm();
            IConfigureFunctionsXmlTreeViewSynchronizer synchronizer = factory.GetConfigureFunctionsXmlTreeViewSynchronizer(configureFunctionsForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode existingFunctionNode = (StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0].Nodes[0];
            XmlElement replacementFunctionElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureFunctionsForm.XmlDocument,
                GetXmlElement(@"<function name=""IsNullOrEmpty"" >
                                    <memberName>IsNullOrEmpty</memberName>
                                    <functionCategory>Standard</functionCategory>
                                    <typeName>System.String</typeName>
                                    <referenceName></referenceName>
                                    <referenceDefinition></referenceDefinition>
                                    <castReferenceAs />
                                    <referenceCategory>Type</referenceCategory>
                                    <parametersLayout>Sequential</parametersLayout>
                                    <parameters>
                                        <literalParameter name=""str"" >
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
                                    <returnType>
                                        <literal>
                                        <literalType>Boolean</literalType>
                                        </literal>
                                    </returnType>
                                    <summary></summary>
                                </function>")
            );

            //act
            synchronizer.ReplaceFunctionNode(existingFunctionNode, replacementFunctionElement);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureFunctionsForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(3, configureFunctionsForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"Copy\"]"));
            Assert.NotNull(configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]"));
            Assert.Equal($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]", configureFunctionsForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("IsNullOrEmpty", ((XmlElement)configureFunctionsForm.XmlDocument.SelectSingleNode($"{rootXPath}/function[@name=\"IsNullOrEmpty\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        private IConfigureFunctionsForm GetConfigureFunctionsForm()
        {
            ITreeViewBuilderFactory treeViewBuilderFactory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            IServiceFactory serviceFactory = _fixture.ServiceProvider.GetRequiredService<IServiceFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.FunctionsSchema
            );
            RadTreeView radTreeView = new()
            {
                MultiSelect = true
            };
            treeViewXmlDocumentHelper.LoadXmlDocument(initialFunctionsXml);
            IConfigureFunctionsForm configureFunctionsForm = new ConfigureFunctionsFormMock
            (
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name),
                treeViewXmlDocumentHelper,
                radTreeView,
                treeViewXmlDocumentHelper.XmlTreeDocument,
                _fixture.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>()
            );
            treeViewBuilderFactory.GetConfigureFunctionsTreeViewBuilder(configureFunctionsForm).Build(radTreeView, treeViewXmlDocumentHelper.XmlTreeDocument);
            return configureFunctionsForm;
        }

        private static readonly string initialFunctionsXml = @"<forms>
                                                                  <form name=""FUNCTIONS"">
                                                                    <folder name=""Functions"">
                                                                      <function name=""Copy"" >
                                                                        <memberName>Copy</memberName>
                                                                        <functionCategory>Standard</functionCategory>
                                                                        <typeName>System.String</typeName>
                                                                        <referenceName></referenceName>
                                                                        <referenceDefinition></referenceDefinition>
                                                                        <castReferenceAs />
                                                                        <referenceCategory>Type</referenceCategory>
                                                                        <parametersLayout>Sequential</parametersLayout>
                                                                        <parameters>
                                                                          <literalParameter name=""str"" >
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
                                                                        <returnType>
                                                                          <literal>
                                                                            <literalType>String</literalType>
                                                                          </literal>
                                                                        </returnType>
                                                                        <summary></summary>
                                                                      </function>
                                                                      <folder name=""Folder B"">
                                                                          <function name=""StaticVoidMethod B"" >
                                                                            <memberName>StaticVoidMethod</memberName>
                                                                            <functionCategory>Standard</functionCategory>
                                                                            <typeName>Contoso.Test.Business.StaticNonGenericClass</typeName>
                                                                            <referenceName></referenceName>
                                                                            <referenceDefinition />
                                                                            <castReferenceAs />
                                                                            <referenceCategory>Type</referenceCategory>
                                                                            <parametersLayout>Sequential</parametersLayout>
                                                                            <parameters>
                                                                              <literalParameter name=""arg1"" >
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
                                                                              <literalParameter name=""arg2"" >
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
                                                                            <returnType>
                                                                              <literal>
                                                                                <literalType>Void</literalType>
                                                                              </literal>
                                                                            </returnType>
                                                                            <summary></summary>
                                                                          </function>
											                          </folder>
                                                                      <folder name=""Folder C"">
                                                                          <function name=""Intern C"" >
                                                                            <memberName>Intern</memberName>
                                                                            <functionCategory>Standard</functionCategory>
                                                                            <typeName>System.String</typeName>
                                                                            <referenceName></referenceName>
                                                                            <referenceDefinition />
                                                                            <castReferenceAs />
                                                                            <referenceCategory>Type</referenceCategory>
                                                                            <parametersLayout>Sequential</parametersLayout>
                                                                            <parameters>
                                                                              <literalParameter name=""str"" >
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
                                                                            <returnType>
                                                                              <literal>
                                                                                <literalType>String</literalType>
                                                                              </literal>
                                                                            </returnType>
                                                                            <summary></summary>
                                                                          </function>
											                          </folder>
                                                                    </folder>
                                                                  </form>
                                                                  <form name=""BUILT IN FUNCTIONS"">
                                                                    <folder name=""Built In Functions"">
                                                                    </folder>
                                                                  </form>
                                                                </forms>";

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

    public class ConfigureFunctionsXmlTreeViewSynchronizerFixture : IDisposable
    {
        public ConfigureFunctionsXmlTreeViewSynchronizerFixture()
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
