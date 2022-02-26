using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class FunctionTreeFolderBuilderTest
    {
        public FunctionTreeFolderBuilderTest()
		{
			Initialize();
		}

		#region Fields
		private IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateFunctionTreeFolder()
		{
			//arrange
			IFunctionTreeFolderBuilder functionTreeFolderBuilder = serviceProvider.GetRequiredService<IFunctionTreeFolderBuilder>();

			//act
			var voidFuctionsFolder = functionTreeFolderBuilder.GetVoidFunctionsTreeFolder(GetDocument());
            var booleanFuctionsFolder = functionTreeFolderBuilder.GetBooleanFunctionsTreeFolder(GetDocument());

            //assert
			Assert.Equal(2, voidFuctionsFolder.FileNames.Count);
            Assert.Equal(3, booleanFuctionsFolder.FolderNames[0].FileNames.Count);
            Assert.Equal("Booleans", booleanFuctionsFolder.FolderNames[0].Name);

            static XmlDocument GetDocument()
			{
				XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<forms>
                                          <form name=""FUNCTIONS"">
                                            <folder name=""Functions"">
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
                                              <folder name=""Booleans"">
                                                <function name=""After"">
                                                  <memberName>date is</memberName>
                                                  <functionCategory>Standard</functionCategory>
                                                  <typeName />
                                                  <referenceName>referenceNameHere</referenceName>
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
                                                  <memberName>date is</memberName>
                                                  <functionCategory>Standard</functionCategory>
                                                  <typeName />
                                                  <referenceName>referenceNameHere</referenceName>
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

		private void Initialize()
		{
			serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
		}
	}
}
