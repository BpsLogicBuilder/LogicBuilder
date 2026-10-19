using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class CreateVariablesTest
    {
        public CreateVariablesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateVariablesFile()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateVariables createVariables = serviceProvider.GetRequiredService<ICreateVariables>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateVariablesFile)
            );

            //act
            var result = createVariables.Create();

            //assert
            Assert.Equal(XmlDataConstants.FOLDERELEMENT, result.DocumentElement!.Name);
        }

        [Fact]
        public void CreateVariablesFileNotEncrypted()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateVariables createVariables = serviceProvider.GetRequiredService<ICreateVariables>();
            IFileIOHelper fileIOHelper = serviceProvider.GetRequiredService<IFileIOHelper>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateVariablesFile)
            );

            //act
            var result = createVariables.Create();
            string fullPath = pathHelper.CombinePaths
            (
                configurationService.ProjectProperties.ProjectPath,
                ConfigurationFiles.Variables
            );
            string savedText = fileIOHelper.ReadFromFile(fullPath);
            System.Xml.XmlDocument xmlDocument = xmlDocumentHelpers.ToXmlDocument(savedText);

            //assert
            Assert.Equal(XmlDataConstants.FOLDERELEMENT, result.DocumentElement!.Name);
            Assert.Equal(XmlDataConstants.FOLDERELEMENT, xmlDocument.DocumentElement!.Name);
        }
    }
}
