using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ResultMessageBuilderTest
    {
        public ResultMessageBuilderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateResultMessageBuilder()
        {
            //arrange
            IResultMessageBuilder builder = serviceProvider.GetRequiredService<IResultMessageBuilder>();

            //assert
            Assert.NotNull(builder);
        }

        [Fact]
        public void CanCreateResultMessageFromStringInput()
        {
            //arrange
            IResultMessageBuilder builder = serviceProvider.GetRequiredService<IResultMessageBuilder>();

            //act
            var result = builder.BuilderMessage("Result Message");

            //assert
            Assert.Equal("Result Message", result.Message);
        }

        [Fact]
        public void CanCreateResultMessageFromTableSource()
        {
            //arrange
            IResultMessageBuilder builder = serviceProvider.GetRequiredService<IResultMessageBuilder>();

            //act
            var result = builder.BuilderMessage
            (
                serviceProvider.GetRequiredService<IStructuresFactory>().GetTableFileSource
                (
                    @"C:\folder\file.tbl",
                    2, 
                    3
                ), 
                "Table Source Message"
            );

            //assert
            Assert.Equal("Table Source Message", result.Message);
            Assert.Equal
            (
                @"<tableErrorSource fileFullName=""C:\folder\file.tbl"" rowIndex=""2"" columnIndex=""3"" />", 
                result.LinkHiddenText
            );
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.tableVisibleLinkFormat,
                    @"file.tbl",
                    2,
                    3
                ), 
                result.LinkVisibleText
            );
        }

        [Fact]
        public void CanCreateResultMessageFromVisioSource()
        {
            //arrange
            IResultMessageBuilder builder = serviceProvider.GetRequiredService<IResultMessageBuilder>();
            
            //act
            var result = builder.BuilderMessage
            (
                serviceProvider.GetRequiredService<IStructuresFactory>().GetVisioFileSource
                (
                    @"C:\folder\file.vsd",
                    21,
                    2,
                    "Action",
                    31,
                    3
                ),
                "Visio Source Message"
            );

            //assert
            Assert.Equal("Visio Source Message", result.Message);
            Assert.Equal
            (
                @"<diagramErrorSource fileFullName=""C:\folder\file.vsd"" pageIndex=""2"" shapeIndex=""3"" pageId=""21"" shapeId=""31"" shapeMasterName=""Action"" />",
                result.LinkHiddenText
            );
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.diagramVisibleLinkFormat,
                    @"file.vsd",
                    2,
                    "Action",
                    3
                ),
                result.LinkVisibleText
            );
        }
    }
}
