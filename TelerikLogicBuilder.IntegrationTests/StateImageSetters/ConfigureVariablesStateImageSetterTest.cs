using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;

namespace TelerikLogicBuilder.IntegrationTests.StateImageSetters
{
    public class ConfigureVariablesStateImageSetterTest : IClassFixture<ConfigureVariablesStateImageSetterFixture>
    {
        private readonly ConfigureVariablesStateImageSetterFixture _fixture;

        public ConfigureVariablesStateImageSetterTest(ConfigureVariablesStateImageSetterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateConfigureVariablesStateImageSetter()
        {
            //arrange
            IConfigureVariablesStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureVariablesStateImageSetter>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void NodeAndParentShowCheckedForValidVariable()
        {
            //arrange
            IConfigureVariablesStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureVariablesStateImageSetter>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode? childNode;
            StateImageRadTreeNode parentNode = new
            (
                "Variables",
                new RadTreeNode[]
                {
                    (
                        childNode = new StateImageRadTreeNode
                        (
                            "StringItem"
                        )
                    )
                }
            );
            XmlElement xmlElement = GetXmlElement(@"<literalVariable name=""StringItem"">
			                                                <memberName>acrg</memberName>
			                                                <variableCategory>Property</variableCategory>
			                                                <castVariableAs />
			                                                <typeName />
			                                                <referenceName />
			                                                <referenceDefinition />
			                                                <castReferenceAs />
			                                                <referenceCategory>This</referenceCategory>
			                                                <evaluation>Implemented</evaluation>
			                                                <comments />
			                                                <metadata />
			                                                <literalType>Decimal</literalType>
			                                                <control>SingleLineTextBox</control>
			                                                <propertySource />
			                                                <defaultValue />
			                                                <domain>
						                                        <item>true</item>
						                                        <item>false</item>
					                                        </domain>
		                                                </literalVariable>");
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            service.SetImage(xmlElement, childNode, application);

            //assert
            Assert.True(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.False(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
            Assert.False(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
        }

        [Fact]
        public void NodeAndParentShowErrorForInalidVariable()
        {
            //arrange
            IConfigureVariablesStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureVariablesStateImageSetter>();
            ICompareImages compareImages = _fixture.ServiceProvider.GetRequiredService<ICompareImages>();
            StateImageRadTreeNode? childNode;
            StateImageRadTreeNode parentNode = new
            (
                "Variables",
                new RadTreeNode[]
                {
                    (
                        childNode = new StateImageRadTreeNode
                        (
                            "StringItem"
                        )
                    )
                }
            );
            XmlElement xmlElement = GetXmlElement(@"<objectVariable name=""VariableTypeNotLoaded"">
                                                      <memberName>VariableTypeNotLoaded</memberName>
                                                      <variableCategory>Property</variableCategory>
                                                      <castVariableAs />
                                                      <typeName />
                                                      <referenceName />
                                                      <referenceDefinition />
                                                      <castReferenceAs />
                                                      <referenceCategory>This</referenceCategory>
                                                      <evaluation>Implemented</evaluation>
                                                      <comments />
                                                      <metadata />
                                                      <objectType>MMM</objectType>
                                                    </objectVariable>");
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            service.SetImage(xmlElement, childNode, application);

            //assert
            Assert.True(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
            Assert.False(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.False(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
        }

        private static XmlElement GetXmlElement(string xmlString)
            => GetXmlDocument(xmlString).DocumentElement!;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class ConfigureVariablesStateImageSetterFixture : IDisposable
    {
        public ConfigureVariablesStateImageSetterFixture()
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
