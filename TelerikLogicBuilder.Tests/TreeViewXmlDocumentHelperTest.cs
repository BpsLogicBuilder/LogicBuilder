using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class TreeViewXmlDocumentHelperTest
    {
        public TreeViewXmlDocumentHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateTreeViewXmlDocumentHelperThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<ITreeViewXmlDocumentHelper>());
        }

        [Fact]
        public void LoadXmlDocumentSucceedsForValidSchema()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadProjectProperties loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            string projectFileFullName = $"{pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name, nameof(LoadXmlDocumentSucceedsForValidSchema), nameof(LoadXmlDocumentSucceedsForValidSchema))}{FileExtensions.PROJECTFILEEXTENSION}";
            ProjectProperties projectProperties = createProjectProperties.Create
            (
                projectFileFullName
            );

            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(projectProperties.ToXml);
            Assert.Equal("ProjectProperties", treeViewXmlDocumentHelper.XmlTreeDocument.DocumentElement!.Name);
        }

        [Fact]
        public void LoadXmlDocumentThrowsForInvalidSchema()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadProjectProperties loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            string projectFileFullName = $"{pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name, nameof(LoadXmlDocumentThrowsForInvalidSchema), nameof(LoadXmlDocumentThrowsForInvalidSchema))}{FileExtensions.PROJECTFILEEXTENSION}";
            ProjectProperties projectProperties = createProjectProperties.Create
            (
                projectFileFullName
            );

            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.FragmentsSchema
            );

            Assert.Throws<LogicBuilderException>(() => treeViewXmlDocumentHelper.LoadXmlDocument(projectFileFullName));
        }

        [Fact]
        public void LoadXmlDocumentThrowsForInvalidPath()
        {
            //arrange
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();

            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ProjectPropertiesSchema
            );

            Assert.Throws<LogicBuilderException>(() => treeViewXmlDocumentHelper.LoadXmlDocument(@"C\NotExists\notExistsEither.vsd"));
        }
    }
}
