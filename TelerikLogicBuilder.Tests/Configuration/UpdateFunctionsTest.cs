using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class UpdateFunctionsTest
    {
        public UpdateFunctionsTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanUpdateFunctionsFile()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadFunctions loadFunctions = serviceProvider.GetRequiredService<ILoadFunctions>();
            IUpdateFunctions updateFunctions = serviceProvider.GetRequiredService<IUpdateFunctions>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanUpdateFunctionsFile)
            );

            //act
            updateFunctions.Update(GetDocumentToSave());
            var result = loadFunctions.Load();

            //assert
            Assert.Equal(XmlDataConstants.FORMSELEMENT, result.DocumentElement.Name);
            Assert.Equal(2, result.SelectNodes($"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']/function").OfType<XmlElement>().Count());

            static XmlDocument GetDocumentToSave()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<forms>
                                          <form name=""FUNCTIONS"">
                                            <function name=""Access after"" >
                                              <memberName>agent broadcast commit.acca</memberName>
                                              <functionCategory>Standard</functionCategory>
                                              <typeName />
                                              <referenceName>referenceNameHere</referenceName>
                                              <referenceDefinition>Field.Property.Property</referenceDefinition>
                                              <castReferenceAs />
                                              <referenceCategory>InstanceReference</referenceCategory>
                                              <parametersLayout>Sequential</parametersLayout>
                                              <parameters>
                                                <literalParameter name=""value"" >
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
                                            <function name=""Action"" >
                                              <memberName>agent broadcast commit.action</memberName>
                                              <functionCategory>Standard</functionCategory>
                                              <typeName />
                                              <referenceName>referenceNameHere</referenceName>
                                              <referenceDefinition />
                                              <castReferenceAs />
                                              <referenceCategory>This</referenceCategory>
                                              <parametersLayout>Sequential</parametersLayout>
                                              <parameters>
                                                <literalParameter name=""value"" >
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
                                                <literalParameter name=""updatecommit"" >
                                                  <literalType>String</literalType>
                                                  <control>SingleLineTextBox</control>
                                                  <optional>true</optional>
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
                                              <summary>Updates the action code.</summary>
                                            </function>
                                          </form>
                                          <form name=""BUILT IN FUNCTIONS"">
                                          </form>
                                        </forms>");

                return xmlDocument;
            }
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
