using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class EmptyTreeFolderRemoverTest
    {
        public EmptyTreeFolderRemoverTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void RemovesEmptyTreeFolders()
        {
            //arrange
            IVariableTreeFolderBuilder variableTreeFolderBuilder = serviceProvider.GetRequiredService<IVariableTreeFolderBuilder>();
            IEmptyTreeFolderRemover emptyTreeFolderRemover = serviceProvider.GetRequiredService<IEmptyTreeFolderRemover>();

            //act
            var result = variableTreeFolderBuilder.GetTreeFolder(GetDocument());

            //assert
            Assert.Equal(2, result.FolderNames.Count);
            result.FolderNames[0].FileNames.Clear();

            emptyTreeFolderRemover.RemoveEmptyFolders(result);
            Assert.Single(result.FolderNames);

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
                                            <folder name=""Literals2"">
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
    }
}
