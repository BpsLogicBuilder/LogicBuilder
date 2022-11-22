using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlTreeViewSynchronizers
{
    public class ProjectPropertiesXmlTreeViewSynchronizerTest
    {
        public ProjectPropertiesXmlTreeViewSynchronizerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateProjectPropertiesXmlTreeViewSynchronizerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IProjectPropertiesXmlTreeViewSynchronizer>());
        }

        [Fact]
        public void AddValidApplicationNodeSucceeds()
        {
            //arrange
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(GetXmlDocument(App01, "").OuterXml);
            RadTreeView radTreeView = oneApplicationTreeView;
            
            IProjectPropertiesXmlTreeViewSynchronizer projectPropertiesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetProjectPropertiesXmlTreeViewSynchronizer
            (
                new ConfigureProjectPropertiesMock
                (
                    treeViewXmlDocumentHelper,
                    radTreeView,
                    treeViewXmlDocumentHelper.XmlTreeDocument
                )
            );

            XmlElement newApplicationNode = xmlDocumentHelpers.AddElementToDoc
            (
                treeViewXmlDocumentHelper.XmlTreeDocument,
                xmlDocumentHelpers.ToXmlElement(App02)
            );

            //act
            projectPropertiesXmlTreeViewSynchronizer.AddApplicationNode
            (
                radTreeView.Nodes[0],
                newApplicationNode
            );

            //assert
            Assert.Equal(2, radTreeView.Nodes[0].Nodes.Count);
            Assert.Equal
            (
                2, 
                xmlDocumentHelpers.GetChildElements
                (
                    xmlDocumentHelpers.SelectSingleElement(treeViewXmlDocumentHelper.XmlTreeDocument, rootNodeXPath)
                ).Count
            );
        }

        [Fact]
        public void AddInvalidApplicationNodeThrows()
        {
            //arrange
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(GetXmlDocument(App01, "").OuterXml);
            RadTreeView radTreeView = oneApplicationTreeView;
            IProjectPropertiesXmlTreeViewSynchronizer projectPropertiesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetProjectPropertiesXmlTreeViewSynchronizer
            (
                new ConfigureProjectPropertiesMock
                (
                    treeViewXmlDocumentHelper,
                    radTreeView,
                    treeViewXmlDocumentHelper.XmlTreeDocument
                )
            );

            XmlElement newApplicationNode = xmlDocumentHelpers.AddElementToDoc
            (
                treeViewXmlDocumentHelper.XmlTreeDocument,
                xmlDocumentHelpers.ToXmlElement(App01)
            );

            //assert
            Assert.Throws<LogicBuilderException>
            (
                () => projectPropertiesXmlTreeViewSynchronizer.AddApplicationNode
                (
                    radTreeView.Nodes[0],
                    newApplicationNode
                )
            );
        }

        [Fact]
        public void DeleteNodeSucceedsIfResultingXmlIsValid()
        {
            //arrange
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(GetXmlDocument(App01, App02).OuterXml);

            RadTreeView radTreeView = twoApplicationTreeView;
            IProjectPropertiesXmlTreeViewSynchronizer projectPropertiesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetProjectPropertiesXmlTreeViewSynchronizer
            (
                new ConfigureProjectPropertiesMock
                (
                    treeViewXmlDocumentHelper,
                    radTreeView,
                    treeViewXmlDocumentHelper.XmlTreeDocument
                )
            );

            //act
            projectPropertiesXmlTreeViewSynchronizer.DeleteNode
            (
                radTreeView.Nodes[0].Nodes[0]
            );

            //assert
            Assert.Single(radTreeView.Nodes[0].Nodes);
            Assert.Single
            (
                xmlDocumentHelpers.GetChildElements
                (
                    xmlDocumentHelpers.SelectSingleElement(treeViewXmlDocumentHelper.XmlTreeDocument, rootNodeXPath)
                )            
            );
            Assert.Equal
            (
                "App02", 
                xmlDocumentHelpers.SelectSingleElement
                (
                    treeViewXmlDocumentHelper.XmlTreeDocument, 
                    $"{rootNodeXPath}/application[@name=\"App02\"]"
                ).GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
            );
        }

        [Fact]
        public void AddDeleteNodeThrowsIfResultingXmlIsInvalid()
        {
            //arrange
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(GetXmlDocument(App01, "").OuterXml);

            RadTreeView radTreeView = oneApplicationTreeView;
            IProjectPropertiesXmlTreeViewSynchronizer projectPropertiesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetProjectPropertiesXmlTreeViewSynchronizer
            (
                new ConfigureProjectPropertiesMock
                (
                    treeViewXmlDocumentHelper,
                    radTreeView,
                    treeViewXmlDocumentHelper.XmlTreeDocument
                )
            );

            //assert
            Assert.Throws<LogicBuilderException>
            (
                () => projectPropertiesXmlTreeViewSynchronizer.DeleteNode
                (
                    radTreeView.Nodes[0].Nodes[0]
                )
            );
        }

        private readonly RadTreeView oneApplicationTreeView = new()
        {
            Nodes =
            {
                new RadTreeNode
                {
                    Text = "Project Properties",
                    Name = rootNodeXPath,
                    ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                    Nodes =
                    {
                        new RadTreeNode
                        {
                            Text = "App01",
                            Name = $"{rootNodeXPath}/application[@name=\"App01\"]",
                            ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                        }
                    }
                }
            }
        };

        private readonly RadTreeView twoApplicationTreeView = new()
        {
            Nodes =
            {
                new RadTreeNode
                {
                    Text = "Project Properties",
                    Name = rootNodeXPath,
                    ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                    Nodes =
                    {
                        new RadTreeNode
                        {
                            Text = "App01",
                            Name = $"{rootNodeXPath}/application[@name=\"App01\"]",
                            ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                        },
                        new RadTreeNode
                        {
                            Text = "App02",
                            Name = $"{rootNodeXPath}/application[@name=\"App02\"]",
                            ImageIndex = ImageIndexes.APPLICATIONIMAGEINDEX
                        }
                    }
                }
            }
        };

        private static readonly string rootNodeXPath = $"/{XmlDataConstants.PROJECTPROPERTIESELEMENT}/{XmlDataConstants.APPLICATIONSELEMENT}";
        private static readonly string App01 = @"<application name=""App01"" nickname=""app01"">
                                          <activityAssembly>BusinessApp.EXE</activityAssembly>
                                          <activityAssemblyPath>C:\Program Files\</activityAssemblyPath>
                                          <runtime>NetFramework</runtime>
                                          <loadAssemblyPaths>
                                            <path>arg1</path>
                                            <path>arg2</path>
                                          </loadAssemblyPaths>
                                          <activityClass>BusinessApp.Activity</activityClass>
                                          <applicationExecutable>BusinessApp.EXE</applicationExecutable>
                                          <applicationExecutablePath>C:\Program Files\</applicationExecutablePath>
                                          <startupArguments>
                                            <argument>arg1</argument>
                                            <argument>arg2</argument>
                                            <argument>arg3</argument>
                                          </startupArguments>
                                          <resourceFile>strings.res</resourceFile>
                                          <resourceFileDeploymentPath>C:\_RulesProject\deployment\</resourceFileDeploymentPath>
                                          <rulesFile>expert.rules</rulesFile>
                                          <rulesDeploymentPath>C:\_RulesProject\deployment\</rulesDeploymentPath>
                                          <excludedModules>
                                            <module>moduleA</module>
                                            <module>moduleB</module>
                                            <module>moduleC</module>
                                            <module>moduleD</module>
                                          </excludedModules>
                                          <remoteDeployment endPointAddress=""http://address.test1.test2/service"" securityMode=""Message"" dnsValue=""cert"">
                                            <userName>userName</userName>
                                            <password>password</password>
                                            <addressHeaders>
                                              <header name=""header1"" namespace="""" value=""http://test.test1.test2/header1"" />
                                              <header name=""header1"" namespace="""" value=""http://test.test1.test2/header2"" />
                                            </addressHeaders>
                                          </remoteDeployment>
                                          <webApiDeployment>
                                            <postFileDataUrl>http://test.test1.test2/header1</postFileDataUrl>
                                            <postVariablesMetaDataUrl>http://test.test1.test2/header1</postVariablesMetaDataUrl>
                                            <deleteRulesUrl>http://test.test1.test2/header1</deleteRulesUrl>
                                            <deleteAllRulesUrl>http://test.test1.test2/header1</deleteAllRulesUrl>
                                          </webApiDeployment>
                                        </application>";

        private static readonly string App02 = @"<application name=""App02"" nickname=""app02"">
                                          <activityAssembly>BusinessApp.EXE</activityAssembly>
                                          <activityAssemblyPath>C:\Program Files\</activityAssemblyPath>
                                          <runtime>NetFramework</runtime>
                                          <loadAssemblyPaths>
                                            <path>arg1</path>
                                            <path>arg2</path>
                                          </loadAssemblyPaths>
                                          <activityClass>BusinessApp.Activity</activityClass>
                                          <applicationExecutable>BusinessApp.EXE</applicationExecutable>
                                          <applicationExecutablePath>C:\Program Files\</applicationExecutablePath>
                                          <startupArguments>
                                            <argument>arg1</argument>
                                            <argument>arg2</argument>
                                            <argument>arg3</argument>
                                          </startupArguments>
                                          <resourceFile>strings.res</resourceFile>
                                          <resourceFileDeploymentPath>C:\_RulesProject\deployment\</resourceFileDeploymentPath>
                                          <rulesFile>expert.rules</rulesFile>
                                          <rulesDeploymentPath>C:\_RulesProject\deployment\</rulesDeploymentPath>
                                          <excludedModules>
                                            <module>moduleA</module>
                                            <module>moduleB</module>
                                            <module>moduleC</module>
                                            <module>moduleD</module>
                                          </excludedModules>
                                          <remoteDeployment endPointAddress=""http://address.test1.test2/service"" securityMode=""Message"" dnsValue=""cert"">
                                            <userName>userName</userName>
                                            <password>password</password>
                                            <addressHeaders>
                                              <header name=""header1"" namespace="""" value=""http://test.test1.test2/header1"" />
                                              <header name=""header1"" namespace="""" value=""http://test.test1.test2/header2"" />
                                            </addressHeaders>
                                          </remoteDeployment>
                                          <webApiDeployment>
                                            <postFileDataUrl>http://test.test1.test2/header1</postFileDataUrl>
                                            <postVariablesMetaDataUrl>http://test.test1.test2/header1</postVariablesMetaDataUrl>
                                            <deleteRulesUrl>http://test.test1.test2/header1</deleteRulesUrl>
                                            <deleteAllRulesUrl>http://test.test1.test2/header1</deleteAllRulesUrl>
                                          </webApiDeployment>
                                        </application>";

        static XmlDocument GetXmlDocument(string app1, string app2)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(@$"<ProjectProperties>
                                      <useSharePoint>false</useSharePoint>
                                      <web>http://serverName/webSite</web>
                                      <documentLibrary>Projects</documentLibrary>
                                      <useDefaultCredentials>true</useDefaultCredentials>
                                      <userName>John Smith</userName>
                                      <domain>Domain</domain>
                                      <applications>{app1}{app2}</applications>
                                      <questionsHierarchyObjectTypes>
                                        <objectTypesGroup name=""Form"" />
                                        <objectTypesGroup name=""Row"" />
                                        <objectTypesGroup name=""Column"" />
                                        <objectTypesGroup name=""Question"" />
                                        <objectTypesGroup name=""Answer"" />
                                      </questionsHierarchyObjectTypes>
                                      <inputQuestionsHierarchyObjectTypes>
                                        <objectTypesGroup name=""Form"">
                                          <objectType>Operator</objectType>
                                          <objectType>OperatorGroup</objectType>
                                        </objectTypesGroup>
                                        <objectTypesGroup name=""Row"" />
                                        <objectTypesGroup name=""Column"" />
                                        <objectTypesGroup name=""Question"" />
                                      </inputQuestionsHierarchyObjectTypes>
                                      <connectorObjectTypes>
                                        <objectType>Operator</objectType>
                                        <objectType>Grid.Pageable</objectType>
                                      </connectorObjectTypes>
                                    </ProjectProperties>");

            return xmlDocument;
        }
    }
}
