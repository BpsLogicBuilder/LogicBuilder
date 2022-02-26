using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class VariableTreeFolderBuilderTest
    {
        public VariableTreeFolderBuilderTest()
		{
			Initialize();
		}

		#region Fields
		private IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateVariableTreeFolder()
		{
			//arrange
			IVariableTreeFolderBuilder variableTreeFolderBuilder = serviceProvider.GetRequiredService<IVariableTreeFolderBuilder>();

			//act
			var result = variableTreeFolderBuilder.GetTreeFolder(GetDocument());

			//assert
			Assert.Equal("Literals", result.FolderNames[0].Name);
			Assert.Single(result.FileNames);
			Assert.Single(result.FolderNames);
			Assert.Single(result.FolderNames[0].FileNames);
			Assert.Empty(result.FolderNames[0].FolderNames);

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
