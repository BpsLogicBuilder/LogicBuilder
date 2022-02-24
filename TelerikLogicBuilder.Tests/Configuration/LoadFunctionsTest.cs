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
    public class LoadFunctionsTest
    {
        public LoadFunctionsTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLoadFunctions()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateFunctions createFunctions = serviceProvider.GetRequiredService<ICreateFunctions>();
            ILoadFunctions loadFunctions = serviceProvider.GetRequiredService<ILoadFunctions>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanLoadFunctions)
            );
            createFunctions.Create();

            //act
            var result = loadFunctions.Load();

            //assert
            Assert.Equal(XmlDataConstants.FORMSELEMENT, result.DocumentElement.Name);
            Assert.Equal(2, result.DocumentElement.ChildNodes.OfType<XmlElement>().Count());
            Assert.NotNull
            (
                result.SelectSingleNode
                (
                    $"/forms/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']"
                )
            );
            Assert.NotNull
            (
                result.SelectSingleNode
                (
                    $"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']"
                )
            );
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
