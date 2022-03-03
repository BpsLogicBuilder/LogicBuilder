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
    public class UpdateVariablesTest
    {
        public UpdateVariablesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanUpdateVariablesFile()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadVariables loadVariables = serviceProvider.GetRequiredService<ILoadVariables>();
            IUpdateVariables updateVariables = serviceProvider.GetRequiredService<IUpdateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanUpdateVariablesFile)
            );

            //act
            updateVariables.Update(GetDocumentToSave());
            var result = loadVariables.Load();

            //assert
            Assert.Equal(XmlDataConstants.FOLDERELEMENT, result.DocumentElement!.Name);
            Assert.Equal
            (
                2, 
                result.SelectNodes
                (
                    $"//{XmlDataConstants.LITERALVARIABLEELEMENT}|//{XmlDataConstants.OBJECTVARIABLEELEMENT}|//{XmlDataConstants.LITERALLISTVARIABLEELEMENT}|//{XmlDataConstants.OBJECTLISTVARIABLEELEMENT}"
                )!
                .OfType<XmlElement>()
                .Count()
            );

            static XmlDocument GetDocumentToSave()
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
                                        </folder>");

                return xmlDocument;
            }
        }
    }
}
