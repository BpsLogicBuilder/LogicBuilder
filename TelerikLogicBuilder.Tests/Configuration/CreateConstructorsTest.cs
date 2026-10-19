using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class CreateConstructorsTest
    {
        public CreateConstructorsTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateConstructor()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateConstructors createConstructors = serviceProvider.GetRequiredService<ICreateConstructors>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateConstructor)
            );

            //act
            var result = createConstructors.Create();

            //assert
            Assert.Equal(XmlDataConstants.FORMELEMENT, result.DocumentElement!.Name);
        }

        [Fact]
        public void CreateConstructorFileNotEncrypted()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateConstructors createConstructors = serviceProvider.GetRequiredService<ICreateConstructors>();
            IFileIOHelper fileIOHelper = serviceProvider.GetRequiredService<IFileIOHelper>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateConstructor)
            );

            //act
            var result = createConstructors.Create();
            string fullPath = pathHelper.CombinePaths
            (
                configurationService.ProjectProperties.ProjectPath,
                ConfigurationFiles.Constructors
            );
            string savedText = fileIOHelper.ReadFromFile(fullPath);
            System.Xml.XmlElement xmlElement = xmlDocumentHelpers.ToXmlElement(savedText);

            //assert
            Assert.Equal(XmlDataConstants.FORMELEMENT, result.DocumentElement!.Name);
            Assert.Equal(XmlDataConstants.FORMELEMENT, xmlElement.Name);
        }
    }
}
