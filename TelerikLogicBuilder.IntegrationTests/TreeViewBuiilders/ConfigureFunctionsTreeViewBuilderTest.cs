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
    public class ConfigureFunctionsTreeViewBuilderTest : IClassFixture<ConfigureFunctionsTreeViewBuilderFixture>
    {
        private readonly ConfigureFunctionsTreeViewBuilderFixture _fixture;

        public ConfigureFunctionsTreeViewBuilderTest(ConfigureFunctionsTreeViewBuilderFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CreateConfigureFunctionsTreeViewBuilderThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsTreeViewBuilder>());
        }

        [Fact]
        public void CanBuildConfigureFunctionsTreeView()
        {
            //arrange
            ITreeViewBuilderFactory factory = _fixture.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            IServiceFactory serviceFactory = _fixture.ServiceProvider.GetRequiredService<IServiceFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.FunctionsSchema
            );
            XmlDocument xmlDocument = GetXmlDocument(@"<forms>
                                                          <form name=""FUNCTIONS"">
                                                            <folder name=""Functions"">
                                                              <function name=""StaticVoidMethod"" >
                                                                <memberName>StaticVoidMethod</memberName>
                                                                <functionCategory>Standard</functionCategory>
                                                                <typeName>Contoso.Test.Business.StaticNonGenericClass</typeName>
                                                                <referenceName></referenceName>
                                                                <referenceDefinition></referenceDefinition>
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
                                                                <summary>Updates the access the access after field.</summary>
                                                              </function>
                                                            </folder>
                                                          </form>
                                                          <form name=""BUILT IN FUNCTIONS"">
                                                            <folder name=""Built In Functions"">
                                                            </folder>
                                                          </form>
                                                        </forms>");
            RadTreeView radTreeView = new();
            IConfigureFunctionsForm configureFunctionsForm = new ConfigureFunctionsFormMock
            (
                _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name),
                treeViewXmlDocumentHelper,
                radTreeView,
                treeViewXmlDocumentHelper.XmlTreeDocument,
                 _fixture.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>()
            );


            //act
            factory.GetConfigureFunctionsTreeViewBuilder(configureFunctionsForm).Build(radTreeView, xmlDocument);

            //assert
            Assert.NotNull(radTreeView.Nodes[0]);//root folder
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)radTreeView.Nodes[0].Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));//function
            Assert.True(compareImages.AreEqual(((StateImageRadTreeNode)radTreeView.Nodes[0].Nodes[0].Nodes[0]).StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));//literalParameter
            Assert.Equal(ImageIndexes.METHODIMAGEINDEX, radTreeView.Nodes[0].Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.LITERALPARAMETERIMAGEINDEX, radTreeView.Nodes[0].Nodes[0].Nodes[0].ImageIndex);
        }

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class ConfigureFunctionsTreeViewBuilderFixture : IDisposable
    {
        public ConfigureFunctionsTreeViewBuilderFixture()
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
