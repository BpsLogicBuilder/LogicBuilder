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
    public class CreateFunctionsTest
    {
        public CreateFunctionsTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateFunctionsFile()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateFunctions createFunctions = serviceProvider.GetRequiredService<ICreateFunctions>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateFunctionsFile)
            );

            //act
            var result = createFunctions.Create();

            //assert
            Assert.Equal(XmlDataConstants.FORMSELEMENT, result.DocumentElement!.Name);
            Assert.Equal(2, result.DocumentElement.ChildNodes.OfType<XmlElement>().Count());
        }

        [Fact]
        public void CreateFunctionsFileNotEncrypted()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateFunctions createFunctions = serviceProvider.GetRequiredService<ICreateFunctions>();
            IFileIOHelper fileIOHelper = serviceProvider.GetRequiredService<IFileIOHelper>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateFunctionsFile)
            );

            //act
            var result = createFunctions.Create();
            string fullPath = pathHelper.CombinePaths
            (
                configurationService.ProjectProperties.ProjectPath,
                ConfigurationFiles.Functions
            );
            string savedText = fileIOHelper.ReadFromFile(fullPath);
            XmlDocument xmlDocument = xmlDocumentHelpers.ToXmlDocument(savedText);

            //assert
            Assert.Equal(XmlDataConstants.FORMSELEMENT, result.DocumentElement!.Name);
            Assert.Equal(XmlDataConstants.FORMSELEMENT, xmlDocument.DocumentElement!.Name);
            Assert.Equal(2, result.DocumentElement.ChildNodes.OfType<XmlElement>().Count());
            Assert.Single(xmlDocument.DocumentElement.ChildNodes.OfType<XmlElement>());//createFunctions.Create() returns the built-in functions form also
        }
    }
}
