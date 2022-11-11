using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Telerik.WinControls.UI;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class ConfigureProjectPropertiesTreeviewBuilderTest
    {
        public ConfigureProjectPropertiesTreeviewBuilderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateConfigureProjectPropertiesTreeviewBuilder()
        {
            //arrange
            IConfigureProjectPropertiesTreeviewBuilder service = serviceProvider.GetRequiredService<IConfigureProjectPropertiesTreeviewBuilder>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void BuildTreeViewSucceeds()
        {
            //arrange
            IConfigureProjectPropertiesTreeviewBuilder service = serviceProvider.GetRequiredService<IConfigureProjectPropertiesTreeviewBuilder>();
            RadTreeView radTreeView = new();

            //act
            service.Build(radTreeView, GetXmlDocument(App01, App02));

            //assert
            Assert.Single(radTreeView.Nodes);
            Assert.Equal(2, radTreeView.Nodes[0].Nodes.Count); 
            Assert.Equal(ImageIndexes.PROJECTFOLDERIMAGEINDEX, radTreeView.Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.APPLICATIONIMAGEINDEX, radTreeView.Nodes[0].Nodes[0].ImageIndex);
        }

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
