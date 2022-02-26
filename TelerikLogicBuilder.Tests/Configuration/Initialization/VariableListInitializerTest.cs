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
    public class VariableListInitializerTest
    {
        public VariableListInitializerTest()
		{
			Initialize();
		}

		#region Fields
		private IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateVariableList()
		{
			//arrange
			IVariableListInitializer variableListInitializer = serviceProvider.GetRequiredService<IVariableListInitializer>();
			ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
			IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
			IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
			IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
			configurationService.ProjectProperties = createProjectProperties.Create
			(
				pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
				nameof(CanCreateVariableList)
			);
			updateVariables.Update(GetDocument());

			//act
			var result = variableListInitializer.InitializeList();

			//assert
			Assert.Equal(2, result.Variables.Count);
			Assert.Equal("Literals", result.VariablesTreeFolder.FolderNames[0].Name);
			Assert.Single(result.VariablesTreeFolder.FileNames);
			Assert.Single(result.VariablesTreeFolder.FolderNames);
			Assert.Single(result.VariablesTreeFolder.FolderNames[0].FileNames);
			Assert.Empty(result.VariablesTreeFolder.FolderNames[0].FolderNames);

			static XmlDocument GetDocument()
			{
				XmlDocument xmlDocument = new();
				xmlDocument.LoadXml(@"<folder name=""Decisions"">
                                          <objectVariable name=""DDD"">
                                              <memberName>acrg</memberName>
                                              <variableCategory>Property</variableCategory>
                                              <castVariableAs />
                                              <typeName />
                                              <referenceName />
                                              <referenceDefinition />
                                              <castReferenceAs />
                                              <referenceCategory>This</referenceCategory>
                                              <evaluation>Implemented</evaluation>
                                              <comments />
                                              <metadata />
                                              <objectType>MMM</objectType>
                                            </objectVariable>
                                            <folder name=""Literals"">
												<literalListVariable name=""List MLT ACRG"">
													<memberName>acrg</memberName>
													<variableCategory>Property</variableCategory>
													<castVariableAs />
													<typeName />
													<referenceName />
													<referenceDefinition />
													<castReferenceAs />
													<referenceCategory>This</referenceCategory>
													<evaluation>Implemented</evaluation>
													<comments />
													<metadata />
													<literalType>Decimal</literalType>
													<listType>GenericList</listType>
													<control>HashSetForm</control>
													<elementControl>SingleLineTextBox</elementControl>
													<propertySource />
													<defaultValue />
													<domain />
												</literalListVariable>
											</folder>
                                        </folder>");

				return xmlDocument;
			}
		}

		private void Initialize()
		{
			serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
		}
	}
}
