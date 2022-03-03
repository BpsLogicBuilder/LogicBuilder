using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class FunctionListInitializerTest
    {
        public FunctionListInitializerTest()
		{
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

		#region Fields
		private readonly IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateFunctionList()
		{
			//arrange
			IFunctionListInitializer functionListInitializer = serviceProvider.GetRequiredService<IFunctionListInitializer>();
			ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
			IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
			IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
			IUpdateFunctions updateFunctions = serviceProvider.GetRequiredService<IUpdateFunctions>();
			configurationService.ProjectProperties = createProjectProperties.Create
			(
				pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
				nameof(CanCreateFunctionList)
			);
			updateFunctions.Update(GetDocument());

			//act
			var result = functionListInitializer.InitializeList();

			//assert
			Assert.Equal(2, result.VoidFunctionsTreeFolder.FileNames.Count);
			Assert.Equal(3, result.BooleanFunctionsTreeFolder.FolderNames[0].FileNames.Count);
			Assert.Equal("Booleans", result.BooleanFunctionsTreeFolder.FolderNames[0].Name);

			static XmlDocument GetDocument()
			{
				XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<forms>
                                          <form name=""FUNCTIONS"">
                                            <folder name=""Functions"">
                                              <function name=""Access after"" >
                                                <memberName>AccessAfter</memberName>
                                                <functionCategory>Standard</functionCategory>
                                                <typeName />
                                                <referenceName>foo.Bar.Boz</referenceName>
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
                                                <memberName>Action</memberName>
                                                <functionCategory>Standard</functionCategory>
                                                <typeName />
                                                <referenceName></referenceName>
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
                                              <folder name=""Booleans"">
                                                <function name=""After"">
                                                  <memberName>After</memberName>
                                                  <functionCategory>Standard</functionCategory>
                                                  <typeName />
                                                  <referenceName></referenceName>
                                                  <referenceDefinition />
                                                  <castReferenceAs />
                                                  <referenceCategory>This</referenceCategory>
                                                  <parametersLayout>Sequential</parametersLayout>
                                                  <parameters>
                                                    <literalParameter name=""val1"">
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
                                                    <literalParameter name=""after"">
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
                                                    <literalParameter name=""val2"">
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
                                                      <literalType>Boolean</literalType>
                                                    </literal>
                                                  </returnType>
                                                  <summary>NA</summary>
                                                </function>
                                                <function name=""Before"">
                                                  <memberName>Before</memberName>
                                                  <functionCategory>Standard</functionCategory>
                                                  <typeName />
                                                  <referenceName></referenceName>
                                                  <referenceDefinition />
                                                  <castReferenceAs />
                                                  <referenceCategory>This</referenceCategory>
                                                  <parametersLayout>Sequential</parametersLayout>
                                                  <parameters>
                                                    <literalParameter name=""val1"">
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
                                                    <literalParameter name=""before"">
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
                                                    <literalParameter name=""val2"">
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
                                                      <literalType>Boolean</literalType>
                                                    </literal>
                                                  </returnType>
                                                  <summary>NA</summary>
                                                </function>
                                                <function name=""Contains"">
                                                  <memberName>regexp</memberName>
                                                  <functionCategory>DialogForm</functionCategory>
                                                  <typeName>referenceNameHere</typeName>
                                                  <referenceName></referenceName>
                                                  <referenceDefinition />
                                                  <castReferenceAs />
                                                  <referenceCategory>Type</referenceCategory>
                                                  <parametersLayout>Sequential</parametersLayout>
                                                  <parameters>
                                                    <literalParameter name=""val1"">
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
                                                    <literalParameter name=""val2"">
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
                                                      <literalType>Boolean</literalType>
                                                    </literal>
                                                  </returnType>
                                                  <summary>NA</summary>
                                                </function>
                                              </folder>
                                            </folder>
                                          </form>
                                          <form name=""BUILT IN FUNCTIONS"">
                                            <folder name=""Built In Functions"">
                                            </folder>
                                          </form>
                                        </forms>");

                return xmlDocument;
			}
		}
	}
}
