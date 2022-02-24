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
    public class UpdateFragmentsTest
    {
        public UpdateFragmentsTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanUpdateFragmentsFile()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadFragments loadFragments = serviceProvider.GetRequiredService<ILoadFragments>();
            IUpdateFragments updateFragments = serviceProvider.GetRequiredService<IUpdateFragments>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanUpdateFragmentsFile)
            );

            //act
            updateFragments.Update(GetDocumentToSave());
            var result = loadFragments.Load();

            //assert
            Assert.Equal(XmlDataConstants.FOLDERELEMENT, result.DocumentElement.Name);
            Assert.Single
            (
                result.SelectNodes
                (
                    $"//{XmlDataConstants.FRAGMENTELEMENT}"
                )
                .OfType<XmlElement>()
            );

            XmlDocument GetDocumentToSave()
            {
                XmlDocument xmlDocument = new();
                xmlDocument.LoadXml(@"<folder name=""Fragments"">
                                        <fragment name=""ParameterOperatorParameters"">
                                            <constructor name=""ParameterOperatorParameters"" visibleText =""ParameterOperatorParameters: parameterName=$it"">
                                            <genericArguments />
                                            <parameters>
                                                <literalParameter name=""parameterName"" >$it</literalParameter>
                                            </parameters>
                                            </constructor>
                                        </fragment>
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
