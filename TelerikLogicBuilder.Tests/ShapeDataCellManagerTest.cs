using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ShapeDataCellManagerTest : IClassFixture<ShapeDataCellManagerFixture>
    {
        private readonly ShapeDataCellManagerFixture _fixture;

        public ShapeDataCellManagerTest(ShapeDataCellManagerFixture constructorTypeHelperFixture)
        {
            _fixture = constructorTypeHelperFixture;
        }

        [Fact]
        public void CanCreateShapeDataCellManager()
        {
            //act
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();

            //assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void CanAddPropertyCell()
        {
            //arrange
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();
            Shape shape = GetOnlyShape();
            string cellName = "5EEE8B0351F34433A7759BB5A45BDD6B";
            string label = "6F0E492A0C634C30AFF67C9C20A52280";

            //act
            manager.AddPropertyCell(shape, cellName, label);
            _fixture.VisioDocument.Saved = true;

            //assert
            Assert.True(manager.CellExists(shape, cellName));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CellExistsThrowsForEmptyCellName(string cellName)
        {
            //arrange
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();
            Shape shape = GetOnlyShape();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => manager.CellExists(shape, cellName));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{FCA76B2D-2C4C-4F5E-8E26-C5E62013B9E2}"), 
                exception.Message
            );
        }

        [Fact]
        public void CellExistsReturnsFalseForNonExixtentCell()
        {
            //arrange
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();
            Shape shape = GetOnlyShape();
            string cellName = "3BBDA83B26F84269A31D74B852F137F2";

            //act
            var result = manager.CellExists(shape, cellName);

            //assert
            Assert.False(result);
        }

        [Fact]
        public void GetPropertyStringReturnsEmptyStringForNonExixtentCell()
        {
            //arrange
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();
            Shape shape = GetOnlyShape();
            string cellName = "DABB5FDD2F074700BBC51A1AF16769FA";

            //act
            var result = manager.GetPropertyString(shape, cellName);

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public void CanSetPropertyString()
        {
            //arrange
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();
            Shape shape = GetOnlyShape();
            string cellName = "64F8282264D1422CB6CE74CF775FE10F";
            string label = "EAE02A75A27E4049856B557AB0A180A7";
            string textToAdd = "{A5F0AA95-A0CF-47CD-81BD-E3F995C97828}";

            //act
            manager.AddPropertyCell(shape, cellName, label);
            manager.SetPropertyString(shape, cellName, textToAdd);
            _fixture.VisioDocument.Saved = true;

            //assert
            Assert.Equal(textToAdd, manager.GetPropertyString(shape, cellName));
        }

        [Fact]
        public void CanSetRulesDataString()
        {
            //arrange
            IShapeDataCellManager manager = _fixture.ServiceProvider.GetRequiredService<IShapeDataCellManager>();
            Shape shape = GetOnlyShape();
            string textToAdd = "{EFA48F62-7E35-41EC-BA28-B587B48AE8A8}";

            //act
            manager.SetRulesDataString(shape, textToAdd);
            _fixture.VisioDocument.Saved = true;

            //assert
            Assert.Equal(textToAdd, manager.GetRulesDataString(shape));
        }

        private Shape GetOnlyShape()
        {
            return _fixture.VisioDocument.Pages
                .OfType<Page>()
                .Single()
                .Shapes
                .OfType<Shape>()
                .Single();
        }
    }

    public class ShapeDataCellManagerFixture : IDisposable
    {
        internal InvisibleApp VisioApplication;
        internal Document VisioDocument;
        internal IServiceProvider ServiceProvider;

        public ShapeDataCellManagerFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            VisioApplication = new InvisibleApp();
            VisioDocument = VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Diagrams\ShapeDataCellManagerTest.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            VisioDocument.Close();
            VisioApplication.Quit();
        }
    }
}
