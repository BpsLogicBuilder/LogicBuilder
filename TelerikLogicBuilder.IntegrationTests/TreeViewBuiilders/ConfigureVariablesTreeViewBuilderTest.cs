using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
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
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
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

namespace TelerikLogicBuilder.IntegrationTests.TreeViewBuiilders
{
    public class ConfigureVariablesTreeViewBuilderTest : IClassFixture<ConfigureVariablesTreeViewBuilderFixture>
    {
        private readonly ConfigureVariablesTreeViewBuilderFixture _fixture;

        public ConfigureVariablesTreeViewBuilderTest(ConfigureVariablesTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateConfigureVariablesTreeViewBuilderThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<IConfigureVariablesTreeViewBuilder>());
        }

        [Fact]
        public void CanBuildConfigureVariablesTreeView()
        {
            //arrange
            ITreeViewBuilderFactory factory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            IServiceFactory serviceFactory = _fixture.ServiceProvider.GetRequiredService<IServiceFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.VariablesSchema
            );
            XmlDocument xmlDocument = GetXmlDocument(@"<folder name=""Decisions"">
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
                                                        </folder>");
            RadTreeView radTreeView = new();
            treeViewXmlDocumentHelper.LoadXmlDocument(xmlDocument.OuterXml);
            IConfigureVariablesForm configureVariablesForm = new ConfigureVariablesFormMock
            (
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name), 
                treeViewXmlDocumentHelper,
                radTreeView,
                treeViewXmlDocumentHelper.XmlTreeDocument,
                _fixture.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>()
            );

            //act
            factory.GetConfigureVariablesTreeViewBuilder(configureVariablesForm).Build(radTreeView, xmlDocument);

            //assert
            Assert.NotNull(radTreeView.Nodes[0]);//root folder
            Assert.True(radTreeView.Nodes[0].Nodes.Count > 0);//folder or variable children of the root
            Assert.True(radTreeView.Nodes[0].Nodes[1].Nodes.Count > 0);//child folder has child node
            Assert.True(radTreeView.Nodes[0] is StateImageRadTreeNode);
            Assert.True(radTreeView.Nodes[0].Nodes[0] is StateImageRadTreeNode);
            Assert.True(radTreeView.Nodes[0].Nodes[1] is StateImageRadTreeNode);
            Assert.True(radTreeView.Nodes[0].Nodes[1].Nodes[0] is StateImageRadTreeNode);
        }

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class ConfigureVariablesTreeViewBuilderFixture : IDisposable
    {
        public ConfigureVariablesTreeViewBuilderFixture()
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
