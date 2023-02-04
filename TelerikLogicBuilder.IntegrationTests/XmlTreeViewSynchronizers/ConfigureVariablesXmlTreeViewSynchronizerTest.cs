using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
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
    public class ConfigureVariablesXmlTreeViewSynchronizerTest : IClassFixture<ConfigureVariablesXmlTreeViewSynchronizerFixture>
    {
        private readonly ConfigureVariablesXmlTreeViewSynchronizerFixture _fixture;

        public ConfigureVariablesXmlTreeViewSynchronizerTest(ConfigureVariablesXmlTreeViewSynchronizerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateConfigureVariablesXmlTreeViewSynchronizerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<IConfigureVariablesXmlTreeViewSynchronizer>());
        }

        [Fact]
        public void AddFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);

            //act
            synchronizer.AddFolder((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0], "AFolder");
            synchronizer.AddFolder((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0], "XFolder");

            //assert
            Assert.Equal("AFolder", configureVariablesForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("XFolder", configureVariablesForm.TreeView.Nodes[0].Nodes[4].Text);
            Assert.Equal("/folder/folder[@name=\"AFolder\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("/folder/folder[@name=\"XFolder\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[4].Name);
            Assert.Equal("AFolder", ((XmlElement)configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"AFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("XFolder", ((XmlElement)configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"XFolder\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddVariableWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement newVariableElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureVariablesForm.XmlDocument,
                GetXmlElement(@"<literalVariable name=""IntegerItem"">
			                        <memberName>IntegerItem</memberName>
			                        <variableCategory>Property</variableCategory>
			                        <castVariableAs />
			                        <typeName />
			                        <referenceName />
			                        <referenceDefinition></referenceDefinition>
			                        <castReferenceAs />
			                        <referenceCategory>This</referenceCategory>
			                        <evaluation>Implemented</evaluation>
			                        <comments />
			                        <metadata />
			                        <literalType>Integer</literalType>
			                        <control>SingleLineTextBox</control>
			                        <propertySource />
			                        <defaultValue />
			                        <domain />
		                        </literalVariable>")
            );

            //act
            StateImageRadTreeNode newTreeNode = synchronizer.AddVariableNode((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0], newVariableElement);

            //assert
            Assert.Equal(newTreeNode, configureVariablesForm.TreeView.SelectedNode);
            Assert.True(compareImages.AreEqual(newTreeNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("IntegerItem", configureVariablesForm.TreeView.Nodes[0].Nodes[1].Text);
            Assert.Equal("/folder/literalVariable[@name=\"IntegerItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[1].Name);
            Assert.Equal("IntegerItem", ((XmlElement)configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"IntegerItem\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void AddVariableNodesWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            XmlElement firstVariableElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureVariablesForm.XmlDocument,
                GetXmlElement(@"<literalVariable name=""IntegerItem"">
			                        <memberName>IntegerItem</memberName>
			                        <variableCategory>Property</variableCategory>
			                        <castVariableAs />
			                        <typeName />
			                        <referenceName />
			                        <referenceDefinition></referenceDefinition>
			                        <castReferenceAs />
			                        <referenceCategory>This</referenceCategory>
			                        <evaluation>Implemented</evaluation>
			                        <comments />
			                        <metadata />
			                        <literalType>Integer</literalType>
			                        <control>SingleLineTextBox</control>
			                        <propertySource />
			                        <defaultValue />
			                        <domain />
		                        </literalVariable>")
            );
            XmlElement secondVariableElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureVariablesForm.XmlDocument,
                GetXmlElement(@"<literalVariable name=""BooleanItem"">
			                        <memberName>BooleanItem</memberName>
			                        <variableCategory>Property</variableCategory>
			                        <castVariableAs />
			                        <typeName />
			                        <referenceName />
			                        <referenceDefinition></referenceDefinition>
			                        <castReferenceAs />
			                        <referenceCategory>This</referenceCategory>
			                        <evaluation>Implemented</evaluation>
			                        <comments />
			                        <metadata />
			                        <literalType>Boolean</literalType>
			                        <control>SingleLineTextBox</control>
			                        <propertySource />
			                        <defaultValue />
			                        <domain />
		                        </literalVariable>")
            );

            //act
            synchronizer.AddVariableNodes
            (
                (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0], 
                new XmlElement[] { firstVariableElement, secondVariableElement }
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal("BooleanItem", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("IntegerItem", configureVariablesForm.TreeView.Nodes[0].Nodes[2].Text);
            Assert.Equal("/folder/literalVariable[@name=\"BooleanItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/literalVariable[@name=\"IntegerItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[2].Name);
            Assert.Equal("BooleanItem", ((XmlElement)configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"BooleanItem\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
            Assert.Equal("IntegerItem", ((XmlElement)configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"IntegerItem\"]")!).GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
        }

        [Fact]
        public void DeleteVariableWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();

            //act
            synchronizer.DeleteNode((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[0]);

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Equal(2, configureVariablesForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"DecimalItem\"]"));
        }

        [Fact]
        public void DeleteFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);

            //act
            synchronizer.DeleteNode((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[1]);

            //assert
            Assert.Equal(2, configureVariablesForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]"));
        }

        [Fact]
        public void MoveFolderWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[2];
            StateImageRadTreeNode movingFolder = (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[1];

            //act
            synchronizer.MoveFolderNode(destinationFolder, movingFolder);

            //assert
            Assert.Equal(2, configureVariablesForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]"));
            Assert.NotNull(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]"));
            Assert.Equal("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[1].Nodes[1].Name);
        }

        [Fact]
        public void MoveFoldersAndVariables()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[2];
            IList<StateImageRadTreeNode> movingTreeNodes = new StateImageRadTreeNode[]
            {
                (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[0],
                (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[1]
            };

            //act
            synchronizer.MoveFoldersAndVariables
            (
                destinationFolder,
                movingTreeNodes
            );

            //assert
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.Single(configureVariablesForm.TreeView.Nodes[0].Nodes);
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"DecimalItem\"]"));
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]"));
            Assert.Equal("Objects", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Text);
            Assert.Equal("/folder/folder[@name=\"Objects\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/literalVariable[@name=\"DecimalItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/objectVariable[@name=\"Object\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes[1].Name);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Name);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].ImageIndex);
            Assert.Equal("/folder/folder[@name=\"Objects\"]/folder[@name=\"Literals\"]/literalVariable[@name=\"StringItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes[2].Nodes[0].Name);
            Assert.Equal(3, configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes.Count);
        }

        [Fact]
        public void MoveVariableWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            StateImageRadTreeNode destinationFolder = (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[1];
            StateImageRadTreeNode movingVariableNode = (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[0];

            //act
            synchronizer.MoveVariableNode(destinationFolder, movingVariableNode);

            //assert
            Assert.Equal(2, configureVariablesForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"DecimalItem\"]"));
            Assert.NotNull(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/folder[@name=\"Literals\"]/literalVariable[@name=\"DecimalItem\"]"));
            Assert.Equal("/folder/folder[@name=\"Literals\"]/literalVariable[@name=\"DecimalItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Nodes[0].Name);
        }

        [Fact]
        public void ReplaceVariableWorks()
        {
            //arrange
            IXmlTreeViewSynchronizerFactory factory = _fixture.ServiceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            IConfigureVariablesForm configureVariablesForm = GetConfigureVariablesForm();
            IConfigureVariablesXmlTreeViewSynchronizer synchronizer = factory.GetConfigureVariablesXmlTreeViewSynchronizer(configureVariablesForm);
            IXmlDocumentHelpers xmlDocumentHelpers = _fixture.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>();
            StateImageRadTreeNode existingVariableNode = (StateImageRadTreeNode)configureVariablesForm.TreeView.Nodes[0].Nodes[0];
            XmlElement replacementVariableElement = xmlDocumentHelpers.AddElementToDoc
            (
                configureVariablesForm.XmlDocument,
                GetXmlElement(@"<literalVariable name=""IntegerItem"">
			                        <memberName>IntegerItem</memberName>
			                        <variableCategory>Property</variableCategory>
			                        <castVariableAs />
			                        <typeName />
			                        <referenceName />
			                        <referenceDefinition></referenceDefinition>
			                        <castReferenceAs />
			                        <referenceCategory>This</referenceCategory>
			                        <evaluation>Implemented</evaluation>
			                        <comments />
			                        <metadata />
			                        <literalType>Integer</literalType>
			                        <control>SingleLineTextBox</control>
			                        <propertySource />
			                        <defaultValue />
			                        <domain />
		                        </literalVariable>")
            );

            //act
            synchronizer.ReplaceVariableNode(existingVariableNode, replacementVariableElement);

            //assert
            Assert.Equal(3, configureVariablesForm.TreeView.Nodes[0].Nodes.Count);
            Assert.Null(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"DecimalItem\"]"));
            Assert.NotNull(configureVariablesForm.XmlDocument.SelectSingleNode("/folder/literalVariable[@name=\"IntegerItem\"]"));
            Assert.Equal("/folder/literalVariable[@name=\"IntegerItem\"]", configureVariablesForm.TreeView.Nodes[0].Nodes[0].Name);
        }

        private IConfigureVariablesForm GetConfigureVariablesForm()
        {
            ITreeViewBuilderFactory treeViewBuilderFactory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            IServiceFactory serviceFactory = _fixture.ServiceProvider.GetRequiredService<IServiceFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.VariablesSchema
            );
            RadTreeView radTreeView = new()
            {
                MultiSelect = true
            };
            treeViewXmlDocumentHelper.LoadXmlDocument(initialVariablesXml);
            IConfigureVariablesForm configureVariablesForm = new ConfigureVariablesFormMock
            (
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name),
                treeViewXmlDocumentHelper,
                radTreeView,
                treeViewXmlDocumentHelper.XmlTreeDocument,
                _fixture.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>()
            );
            treeViewBuilderFactory.GetConfigureVariablesTreeViewBuilder(configureVariablesForm).Build(radTreeView, treeViewXmlDocumentHelper.XmlTreeDocument);
            return configureVariablesForm;
        }

        private static readonly string initialVariablesXml = @"<folder name=""Decisions"">
                                                                  <literalVariable name=""DecimalItem"">
			                                                            <memberName>DecimalItem</memberName>
			                                                            <variableCategory>Property</variableCategory>
			                                                            <castVariableAs />
			                                                            <typeName />
			                                                            <referenceName />
			                                                            <referenceDefinition></referenceDefinition>
			                                                            <castReferenceAs />
			                                                            <referenceCategory>This</referenceCategory>
			                                                            <evaluation>Implemented</evaluation>
			                                                            <comments />
			                                                            <metadata />
			                                                            <literalType>Decimal</literalType>
			                                                            <control>SingleLineTextBox</control>
			                                                            <propertySource />
			                                                            <defaultValue />
			                                                            <domain />
		                                                            </literalVariable>
                                                                    <folder name=""Literals"">
		                                                                <literalVariable name=""StringItem"">
			                                                                <memberName>StringItem</memberName>
			                                                                <variableCategory>Property</variableCategory>
			                                                                <castVariableAs />
			                                                                <typeName />
			                                                                <referenceName />
			                                                                <referenceDefinition></referenceDefinition>
			                                                                <castReferenceAs />
			                                                                <referenceCategory>This</referenceCategory>
			                                                                <evaluation>Implemented</evaluation>
			                                                                <comments />
			                                                                <metadata />
			                                                                <literalType>String</literalType>
			                                                                <control>SingleLineTextBox</control>
			                                                                <propertySource />
			                                                                <defaultValue />
			                                                                <domain />
		                                                                </literalVariable>
											                        </folder>
                                                                    <folder name=""Objects"">
		                                                                <objectVariable name=""Object"">
                                                                          <memberName>somObject</memberName>
                                                                          <variableCategory>Property</variableCategory>
                                                                          <castVariableAs />
                                                                          <typeName />
                                                                          <referenceName />
                                                                          <referenceDefinition />
                                                                          <castReferenceAs />
                                                                          <referenceCategory>This</referenceCategory>
                                                                          <evaluation>Implemented</evaluation>
                                                                          <comments>Comment</comments>
                                                                          <metadata />
                                                                          <objectType>System.Object</objectType>
                                                                        </objectVariable>
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

    public class ConfigureVariablesXmlTreeViewSynchronizerFixture : IDisposable
    {
        public ConfigureVariablesXmlTreeViewSynchronizerFixture()
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
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
                new Dictionary<string, Function>(),
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
