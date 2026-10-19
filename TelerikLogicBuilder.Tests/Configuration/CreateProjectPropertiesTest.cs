using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class CreateProjectPropertiesTest
    {
        public CreateProjectPropertiesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateCreateProjectProperties()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();

            //act
            ProjectProperties projectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateCreateProjectProperties)
            );

            //assert
            Assert.NotNull(projectProperties);
            Assert.Equal("App01", projectProperties.ApplicationList.First().Value.Name);
        }

        [Fact]
        public void CreatedProjectPropertiesFileIsNotEncrypted()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IFileIOHelper fileIOHelper = serviceProvider.GetRequiredService<IFileIOHelper>();
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();

            //act
            ProjectProperties projectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CreatedProjectPropertiesFileIsNotEncrypted)
            );
            string savedText = fileIOHelper.ReadFromFile(projectProperties.ProjectFileFullName);
            System.Xml.XmlElement xmlElement = xmlDocumentHelpers.ToXmlElement(savedText);

            //assert
            Assert.NotNull(projectProperties);
            Assert.Equal(XmlDataConstants.PROJECTPROPERTIESELEMENT, xmlElement.Name);
        }
    }
}
