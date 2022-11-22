using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.GenericArguments
{
    public class GenericConfigManagerTest
    {
        public GenericConfigManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider(); ;
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateGenericConfigManager()
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //assert
            Assert.NotNull(service);
        }

        [Fact]
        public void GetDefaultLiteralGenericConfigSucceeds()
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //act
            var result = service.GetDefaultLiteralGenericConfig("A");

            //assert
            Assert.Equal("A", result.GenericArgumentName);
            Assert.Equal(GenericConfigCategory.Literal, result.GenericConfigCategory);
        }

        [Fact]
        public void GetDefaultObjectGenericConfigSucceeds()
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //act
            var result = service.GetDefaultObjectGenericConfig("A");

            //assert
            Assert.Equal("A", result.GenericArgumentName);
            Assert.Equal(GenericConfigCategory.Object, result.GenericConfigCategory);
        }

        [Fact]
        public void GetDefaultLiteralListGenericConfigSucceeds()
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //act
            var result = service.GetDefaultLiteralListGenericConfig("A");

            //assert
            Assert.Equal("A", result.GenericArgumentName);
            Assert.Equal(GenericConfigCategory.LiteralList, result.GenericConfigCategory);
        }

        [Fact]
        public void GetDefaultObjectListGenericConfigSucceeds()
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //act
            var result = service.GetDefaultObjectListGenericConfig("A");

            //assert
            Assert.Equal("A", result.GenericArgumentName);
            Assert.Equal(GenericConfigCategory.ObjectList, result.GenericConfigCategory);
        }

        [Theory]
        [InlineData(typeof(int), GenericConfigCategory.Literal)]
        [InlineData(typeof(System.Data.DataTable), GenericConfigCategory.Object)]
        [InlineData(typeof(IList<string>), GenericConfigCategory.LiteralList)]
        [InlineData(typeof(IList<System.Data.DataTable>), GenericConfigCategory.ObjectList)]
        internal void CreatesExpectedGenericConfig(Type type, GenericConfigCategory expectedCategory)
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //act
            var result = service.CreateGenericConfig("A", type);

            //assert
            Assert.Equal("A", result.GenericArgumentName);
            Assert.Equal(expectedCategory, result.GenericConfigCategory);
        }

        [Theory]
        [InlineData(typeof(int), GenericConfigCategory.Literal)]
        [InlineData(typeof(System.Data.DataTable), GenericConfigCategory.Object)]
        [InlineData(typeof(IList<string>), GenericConfigCategory.LiteralList)]
        [InlineData(typeof(IList<System.Data.DataTable>), GenericConfigCategory.ObjectList)]
        internal void CreatesExpectedGenericConfigs(Type type, GenericConfigCategory expectedCategory)
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();

            //act
            var result = service.CreateGenericConfigs(new string[] { "A" }, new Type[] { type });

            //assert
            Assert.Equal("A", result[0].GenericArgumentName);
            Assert.Equal(expectedCategory, result[0].GenericConfigCategory);
        }

        [Fact]
        public void ReconcileGenericArgumentsUpdatesMissingConfigsWithObjectConfigs()
        {
            //arrange
            IGenericConfigManager service = serviceProvider.GetRequiredService<IGenericConfigManager>();
            IList<string> configuredGenericArgumentNames = new string[] { "A", "B", "C", "D" };
            IList<GenericConfigBase> dataConfigGenericArguments = new GenericConfigBase[]
            {
                service.GetDefaultLiteralGenericConfig("A"),
                service.GetDefaultLiteralListGenericConfig("B")
            };

            //act
            var result = service.ReconcileGenericArguments(configuredGenericArgumentNames, dataConfigGenericArguments);

            //assert
            Assert.Equal("A", result[0].GenericArgumentName);
            Assert.Equal(GenericConfigCategory.Literal, result[0].GenericConfigCategory);
            Assert.Equal("B", result[1].GenericArgumentName);
            Assert.Equal(GenericConfigCategory.LiteralList, result[1].GenericConfigCategory);
            Assert.Equal("C", result[2].GenericArgumentName);
            Assert.Equal(GenericConfigCategory.Object, result[2].GenericConfigCategory);
            Assert.Equal("D", result[3].GenericArgumentName);
            Assert.Equal(GenericConfigCategory.Object, result[3].GenericConfigCategory);
        }
    }
}
