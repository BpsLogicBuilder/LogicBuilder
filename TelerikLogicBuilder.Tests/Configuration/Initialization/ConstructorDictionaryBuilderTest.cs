using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class ConstructorDictionaryBuilderTest
    {
        public ConstructorDictionaryBuilderTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateConstructorDictionary()
        {
            //arrange
            IConstructorDictionaryBuilder constructorDictionaryBuilder = serviceProvider.GetRequiredService<IConstructorDictionaryBuilder>();

            //act
            var result = constructorDictionaryBuilder.GetDictionary(GetDocument());

            //assert
            Assert.Equal(2, result.Count);

            static XmlDocument GetDocument()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<form>
                                          <folder name=""folderName"">
                                            <constructor name=""OperatorGroup"">
	                                            <typeName>LogicBuilder.Forms.Parameters.Grid.OperatorGroup</typeName>
	                                            <parameters>
		                                            <literalParameter name=""typeName"">
			                                            <literalType>String</literalType>
			                                            <control>SingleLineTextBox</control>
			                                            <optional>false</optional>
			                                            <useForEquality>true</useForEquality>
			                                            <useForHashCode>true</useForHashCode>
			                                            <useForToString>true</useForToString>
			                                            <propertySource />
			                                            <propertySourceParameter />
			                                            <defaultValue>string</defaultValue>
			                                            <domain>
				                                            <item>string</item>
				                                            <item>number</item>
			                                            </domain>
			                                            <comments></comments>
		                                            </literalParameter>
		                                            <objectListParameter name=""Operators"">
                                                        <objectType>Operator</objectType>
			                                            <listType>GenericList</listType>
			                                            <control>HashSetForm</control>
			                                            <optional>false</optional>
			                                            <comments></comments>
		                                            </objectListParameter>
	                                            </parameters>
	                                            <genericArguments />
	                                            <summary></summary>
                                            </constructor>
											<constructor name=""Operator"">
												<typeName>LogicBuilder.Forms.Parameters.Grid.Operator</typeName>
												<parameters>
													<literalParameter name=""name"">
														<literalType>String</literalType>
														<control>SingleLineTextBox</control>
														<optional>false</optional>
														<useForEquality>true</useForEquality>
														<useForHashCode>true</useForHashCode>
														<useForToString>true</useForToString>
														<propertySource />
														<propertySourceParameter />
														<defaultValue>eq</defaultValue>
														<domain>
															<item>eq</item>
															<item>neq</item>
															<item>lt</item>
															<item>lte</item>
															<item>gt</item>
															<item>gte</item>
															<item>contains</item>
															<item>doesnotcontain</item>
															<item>startswith</item>
															<item>endswith</item>
															<item>isnotempty</item>
															<item>isempty</item>
															<item>isnotnull</item>
															<item>isnull</item>
														</domain>
														<comments></comments>
													</literalParameter>
													<literalParameter name=""description"" >
														<literalType>String</literalType>
														<control>SingleLineTextBox</control>
														<optional>false</optional>
														<useForEquality>false</useForEquality>
														<useForHashCode>false</useForHashCode>
														<useForToString>true</useForToString>
														<propertySource />
														<propertySourceParameter />
														<defaultValue>Equals</defaultValue>
														<domain>
															<item>Equals</item>
															<item>Not Equal To</item>
															<item>Less Than</item>
															<item>Less Than or Equal To</item>
															<item>Greater Than</item>
															<item>Greater Than or Equal To</item>
															<item>Contains</item>
															<item>Does not contain</item>
															<item>Starts with</item>
															<item>Ends with</item>
															<item>Is not empty</item>
															<item>Is empty</item>
															<item>Is not null</item>
															<item>Is null</item>
														</domain>
														<comments></comments>
													</literalParameter>
												</parameters>
												<genericArguments />
												<summary></summary>
											</constructor>
                                          </folder>
                                        </form>");

                return xmlDocument;
            }
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
