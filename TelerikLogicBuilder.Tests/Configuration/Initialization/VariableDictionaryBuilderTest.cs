using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class VariableDictionaryBuilderTest
    {
        public VariableDictionaryBuilderTest()
		{
			Initialize();
		}

		#region Fields
		private IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateVariableDictionary()
		{
			//arrange
			IVariableDictionaryBuilder variableDictionaryBuilder = serviceProvider.GetRequiredService<IVariableDictionaryBuilder>();

			//act
			var result = variableDictionaryBuilder.GetDictionary(GetDocument());

			//assert
			Assert.Equal(2, result.Count);

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
