using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class UpdateConstructorsTest
    {
        public UpdateConstructorsTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanUpdateConstructor()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadConstructors loadConstructors = serviceProvider.GetRequiredService<ILoadConstructors>();
            IUpdateConstructors updateConstructors = serviceProvider.GetRequiredService<IUpdateConstructors>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanUpdateConstructor)
            );

            //act
            updateConstructors.Update(GetDocumentToSave());
            var result = loadConstructors.Load();

            //assert
            Assert.Equal(XmlDataConstants.FORMELEMENT, result.DocumentElement!.Name);
            Assert.Equal("OperatorGroup", result.SelectSingleNode("//constructor")!.Attributes![XmlDataConstants.NAMEATTRIBUTE]!.Value);

            static XmlDocument GetDocumentToSave()
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
                                          </folder>
                                        </form>");

                return xmlDocument;
            }
        }
    }
}
