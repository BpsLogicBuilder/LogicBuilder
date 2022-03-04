using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class ApplicationXmlParserTest
    {
        public ApplicationXmlParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateApplicationXmlParser()
        {
            //arrange
            IApplicationXmlParser applicationXmlParser = serviceProvider.GetRequiredService<IApplicationXmlParser>();

            //assert
            Assert.NotNull(applicationXmlParser);
        }

        [Fact]
        public void ParseWorks()
        {
            //arrange
            IApplicationXmlParser applicationXmlParser = serviceProvider.GetRequiredService<IApplicationXmlParser>();

            //act
            var result = applicationXmlParser.Parse(GetXmlDocument().DocumentElement!);

            //assert
            Assert.Equal("App01", result.Name);
            Assert.Equal("app01", result.Nickname);
            Assert.Equal("http://test.test1.test2/header1", result.WebApiDeployment.PostFileDataUrl);
        }

        [Fact]
        public void ParseThrowsIfDocumentElementNameIsNotApplication()
        {
            //arrange
            IApplicationXmlParser applicationXmlParser = serviceProvider.GetRequiredService<IApplicationXmlParser>();
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml("<activityAssembly>BusinessApp.EXE</activityAssembly>");

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => applicationXmlParser.Parse(xmlDocument.DocumentElement!));
        }

        static XmlDocument GetXmlDocument()
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(@"<application name=""App01"" nickname=""app01"">
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
                                    </application>");

            return xmlDocument;
        }
    }
}
