using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class ProjectPropertiesXmlParserTest
    {
        public ProjectPropertiesXmlParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateProjectPropertiesXmlParser()
        {
            //arrange
            IProjectPropertiesXmlParser projectPropertiesXmlParser = serviceProvider.GetRequiredService<IProjectPropertiesXmlParser>();

            //assert
            Assert.NotNull(projectPropertiesXmlParser);
        }

        [Fact]
        public void ParseWorks()
        {
            //arrange
            IProjectPropertiesXmlParser projectPropertiesXmlParser = serviceProvider.GetRequiredService<IProjectPropertiesXmlParser>();

            //act
            var result = projectPropertiesXmlParser.GeProjectProperties(GetXmlDocument().DocumentElement!, "MyProject", @"C:\ProjectPath\");

            //assert
            Assert.Equal("MyProject", result.ProjectName);
            Assert.Equal(@"C:\ProjectPath\", result.ProjectPath);
            Assert.Equal(@"c:\projectpath\myproject.lbproj", result.ProjectFileFullName.ToLowerInvariant());
            Assert.Equal("BusinessApp.EXE", result.ApplicationList.First().Value.ActivityAssembly);
        }

        [Fact]
        public void ParseThrowsIfDocumentElementNameIsNotProjectProperties()
        {
            //arrange
            IProjectPropertiesXmlParser projectPropertiesXmlParser = serviceProvider.GetRequiredService<IProjectPropertiesXmlParser>();
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml("<activityAssembly>BusinessApp.EXE</activityAssembly>");

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => projectPropertiesXmlParser.GeProjectProperties(xmlDocument.DocumentElement!, "MyProject", @"C:\_RulesProject\deployment\"));
        }

        static XmlDocument GetXmlDocument()
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(@"<ProjectProperties>
                                      <useSharePoint>false</useSharePoint>
                                      <web>http://serverName/webSite</web>
                                      <documentLibrary>Projects</documentLibrary>
                                      <useDefaultCredentials>true</useDefaultCredentials>
                                      <userName>John Smith</userName>
                                      <domain>Domain</domain>
                                      <applications>
                                        <application name=""App01"" nickname=""app01"">
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
                                        </application>
                                      </applications>
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
