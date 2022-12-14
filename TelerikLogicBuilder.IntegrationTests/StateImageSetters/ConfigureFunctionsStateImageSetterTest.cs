using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
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
    public class ConfigureFunctionsStateImageSetterTest : IClassFixture<ConfigureFunctionsStateImageSetterFixture>
    {
        private readonly ConfigureFunctionsStateImageSetterFixture _fixture;

        public ConfigureFunctionsStateImageSetterTest(ConfigureFunctionsStateImageSetterFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateConfigureFunctionsStateImageSetter()
        {
            //arrange
            IConfigureFunctionsStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsStateImageSetter>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void NodeAndParentShowCheckedForValidFunction()
        {
            //arrange
            IConfigureFunctionsStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsStateImageSetter>();
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
            XmlElement xmlElement = GetXmlElement(@"<function name=""TestResponseA"" >
                                                        <memberName>Person</memberName>
                                                        <functionCategory>Standard</functionCategory>
                                                        <typeName></typeName>
                                                        <referenceName>referenceNameHere</referenceName>
                                                        <referenceDefinition />
                                                        <castReferenceAs />
                                                        <referenceCategory>This</referenceCategory>
                                                        <parametersLayout>Sequential</parametersLayout>
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
                                                        <returnType>
                                                          <literal>
                                                            <literalType>Void</literalType>
                                                          </literal>
                                                        </returnType>
                                                        <summary>Updates the access the access after field.</summary>
                                                    </function>");
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            service.SetImage(xmlElement, childNode, application);

            //assert
            Assert.True(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.CheckMark));
        }

        [Fact]
        public void NodeAndParentShowErrorForInvalidReturnType()
        {
            //arrange
            IConfigureFunctionsStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsStateImageSetter>();
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
            XmlElement xmlElement = GetXmlElement(@"<function name=""TestResponseA"" >
                                                        <memberName>Person</memberName>
                                                        <functionCategory>Standard</functionCategory>
                                                        <typeName>Contoso.Test.Business.Responses.TestResponseA</typeName>
                                                        <referenceName>referenceNameHere</referenceName>
                                                        <referenceDefinition />
                                                        <castReferenceAs />
                                                        <referenceCategory>This</referenceCategory>
                                                        <parametersLayout>Sequential</parametersLayout>
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
                                                        <returnType>
                                                          <object>
                                                            <objectType>XYZ</objectType>
                                                          </object>
                                                        </returnType>
                                                        <summary>Updates the access the access after field.</summary>
                                                    </function>");
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            service.SetImage(xmlElement, childNode, application);

            //assert
            Assert.True(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
        }

        [Fact]
        public void NodeAndParentShowErrorForInvalidTypeName()
        {
            //arrange
            IConfigureFunctionsStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsStateImageSetter>();
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
            XmlElement xmlElement = GetXmlElement(@"<function name=""TestResponseA"" >
                                                        <memberName>Person</memberName>
                                                        <functionCategory>Standard</functionCategory>
                                                        <typeName>ZZZ</typeName>
                                                        <referenceName></referenceName>
                                                        <referenceDefinition />
                                                        <castReferenceAs />
                                                        <referenceCategory>Type</referenceCategory>
                                                        <parametersLayout>Sequential</parametersLayout>
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
                                                        <returnType>
                                                          <literal>
                                                            <literalType>Void</literalType>
                                                          </literal>
                                                        </returnType>
                                                        <summary>Updates the access the access after field.</summary>
                                                    </function>");
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            service.SetImage(xmlElement, childNode, application);

            //assert
            Assert.True(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
        }

        [Fact]
        public void NodeAndParentShowErrorForInalidParameter()
        {
            //arrange
            IConfigureFunctionsStateImageSetter service = _fixture.ServiceProvider.GetRequiredService<IConfigureFunctionsStateImageSetter>();
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
            XmlElement xmlElement = GetXmlElement(@"<function name=""TestResponseA"" >
                                                        <memberName>Person</memberName>
                                                        <functionCategory>Standard</functionCategory>
                                                        <typeName></typeName>
                                                        <referenceName>referenceNameHere</referenceName>
                                                        <referenceDefinition />
                                                        <castReferenceAs />
                                                        <referenceCategory>This</referenceCategory>
                                                        <parametersLayout>Sequential</parametersLayout>
                                                        <parameters>
                                                            <objectParameter name=""objectProperty"">
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
														        <optional>false</optional>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                             				   <comments></comments>
                                                            </objectParameter>
                                                        </parameters>
                                                        <genericArguments />
                                                        <returnType>
                                                          <literal>
                                                            <literalType>Void</literalType>
                                                          </literal>
                                                        </returnType>
                                                        <summary>Updates the access the access after field.</summary>
                                                    </function>");
            ApplicationTypeInfo application = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            service.SetImage(xmlElement, childNode, application);

            //assert
            Assert.True(compareImages.AreEqual(childNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
            Assert.True(compareImages.AreEqual(parentNode.StateImage, ABIS.LogicBuilder.FlowBuilder.Properties.Resources.Error));
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

    public class ConfigureFunctionsStateImageSetterFixture : IDisposable
    {
        public ConfigureFunctionsStateImageSetterFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
            WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConstructorFactory = ServiceProvider.GetRequiredService<IConstructorFactory>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

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

            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

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
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}
