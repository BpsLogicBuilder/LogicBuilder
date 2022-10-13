using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.Tests.RulesGenerator
{
    public class ShapeHelperTest : IClassFixture<ShapeHelperFixture>
    {
        private readonly ShapeHelperFixture _fixture;

        public ShapeHelperTest(ShapeHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateShapeHelper()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void CheckForDuplicateMultipleChoicesThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CheckForDuplicateMultipleChoices)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CheckForDuplicateMultipleChoices(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{46979888-7878-47FA-A234-3FD8D0C62793}"),
                exception.Message
            );
        }

        [Fact]
        public void CheckForDuplicateMultipleChoicesReturnsDuplicateConnectorIndexForDuplicateConnectorText()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CheckForDuplicateMultipleChoices)}\ReturnsDuplicateConnectorIndexForDuplicateConnectorText.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CheckForDuplicateMultipleChoices(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                3,
                result
            );
        }

        [Fact]
        public void CheckForDuplicateMultipleChoicesReturnsZeroForNoDuplicateChoices()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CheckForDuplicateMultipleChoices)}\ReturnsDuplicateConnectorIndexForDuplicateConnectorText.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CheckForDuplicateMultipleChoices(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                3,
                result
            );
        }

        [Fact]
        public void CountDialogFunctionsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountDialogFunctions)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountDialogFunctions(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{7A51CB43-3EB9-40CD-B2B4-10014576D21C}"),
                exception.Message
            );
        }

        [Fact]
        public void CountDialogFunctionsReturnsOneForOneDialogFunction()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountDialogFunctions)}\ReturnsOneForOneDialogFunction.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountDialogFunctions(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                1,
                result
            );
        }

        [Fact]
        public void CountDialogFunctionsReturnsZeroForNoDialogFunction()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountDialogFunctions)}\ReturnsZeroForNoDialogFunction.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);

            //act
            var result = helper.CountDialogFunctions(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountDialogFunctionsReturnsZeroForSahpeWithNoContent()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountDialogFunctions)}\ReturnsZeroForShapeWithNoContent.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountDialogFunctions(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountFunctionsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountFunctions)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountFunctions(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{2B5BB080-A9EB-49DF-8204-5322E062E9BF}"),
                exception.Message
            );
        }

        [Fact]
        public void CountFunctionsReturnsOneForOneDialogFunction()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountFunctions)}\ReturnsOneForOneDialogFunction.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountFunctions(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                1,
                result
            );
        }

        [Fact]
        public void CountFunctionsReturnsTwoForTwoFunctions()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountFunctions)}\ReturnsTwoForTwoFunctions.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);

            //act
            var result = helper.CountFunctions(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                2,
                result
            );
        }

        [Fact]
        public void CountFunctionsReturnsZeroForShapeWithNoContent()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountFunctions)}\ReturnsZeroForShapeWithNoContent.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountFunctions(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountIncomingConnectorsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountIncomingConnectors)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountIncomingConnectors(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{AFCF9E84-6AB2-4A4D-BC5E-2B57550E3C11}"),
                exception.Message
            );
        }

        [Fact]
        public void CountIncomingConnectorsReturnsThreeWithThreeIncomingConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountIncomingConnectors)}\ReturnsThreeWithThreeIncomingConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountIncomingConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                3,
                result
            );
        }

        [Fact]
        public void CountInvalidMultipleChoiceConnectorsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountInvalidMultipleChoiceConnectors)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountInvalidMultipleChoiceConnectors(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{51176A6A-419D-4762-9BDC-3A1F4A99171D}"),
                exception.Message
            );
        }

        [Fact]
        public void CountInvalidMultipleChoiceConnectorsReturnsOneForOneInvalidConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountInvalidMultipleChoiceConnectors)}\ReturnsOneForOneInvalidConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountInvalidMultipleChoiceConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                1,
                result
            );
        }

        [Fact]
        public void CountInvalidMultipleChoiceConnectorsReturnsZeroForAllConnectorsValid()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountInvalidMultipleChoiceConnectors)}\ReturnsZeroForAllConnectorsValid.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountInvalidMultipleChoiceConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountInvalidMultipleChoiceConnectorsReturnsZeroForShapeWithNoContent()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountInvalidMultipleChoiceConnectors)}\ReturnsZeroForShapeWithNoContent.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountInvalidMultipleChoiceConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountOutgoingBlankConnectorsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingBlankConnectors)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountOutgoingBlankConnectors(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{6D6FBE5B-59A2-4372-BADE-B95698BD0EBF}"),
                exception.Message
            );
        }

        [Fact]
        public void CountOutgoingBlankConnectorsReturnsOneForOneBlankConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingBlankConnectors)}\ReturnsOneForOneBlankConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountOutgoingBlankConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                1,
                result
            );
        }

        [Fact]
        public void CountOutgoingBlankConnectorsReturnsZeroForNoBlankConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingBlankConnectors)}\ReturnsZeroForNoBlankConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountOutgoingBlankConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountOutgoingConnectorsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingConnectors)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountOutgoingConnectors(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{79AE6046-FD96-41CA-8617-87C28FCC4144}"),
                exception.Message
            );
        }

        [Fact]
        public void CountOutgoingConnectorsReturnsThreeForThreeOutgoingConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingConnectors)}\ReturnsThreeForThreeOutgoingConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountOutgoingConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                3,
                result
            );
        }

        [Fact]
        public void CountOutgoingConnectorsReturnsZeroForNoOutgoingConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingConnectors)}\ReturnsZeroForNoOutgoingConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountOutgoingConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void CountOutgoingNonApplicationConnectorsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingNonApplicationConnectors)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.CountOutgoingNonApplicationConnectors(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{81187778-E241-4737-BAE9-B9F4B43DE55E}"),
                exception.Message
            );
        }

        [Fact]
        public void CountOutgoingNonApplicationConnectorsThreeForThreeOutgoingConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingNonApplicationConnectors)}\ReturnsThreeForThreeOutgoingConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountOutgoingNonApplicationConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                3,
                result
            );
        }

        [Fact]
        public void CountOutgoingNonApplicationConnectorsReturnsZeroForNoOutgoingConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountOutgoingNonApplicationConnectors)}\ReturnsZeroForNoOutgoingConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.CountOutgoingNonApplicationConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                0,
                result
            );
        }

        [Fact]
        public void GetApplicationListThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetApplicationList)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetApplicationList(shape, null!));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{FC8A71A1-81E0-43EF-811E-D143D0943F1D}"),
                exception.Message
            );
        }

        [Fact]
        public void GetApplicationListReturnsApplicationsUsingOtherFromMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetApplicationList)}\ReturnsApplicationsUsingOtherFromMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.OTHERSCONNECTOBJECT, visioDocument);
            Shape fromShape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var result = helper.GetApplicationList(connector, new ShapeBag(fromShape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App01", "App02", "App03", "App04", "App05", })
            );
        }

        [Fact]
        public void GetApplicationListReturnsApplicationsUsingOtherFromShapeBag()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetApplicationList)}\ReturnsApplicationsUsingOtherFromShapeBag.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.OTHERSCONNECTOBJECT, visioDocument);
            Shape fromShape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);

            //act
            var result = helper.GetApplicationList(connector, new ShapeBag(fromShape, new string[] { "App06", "App07", "App08", "App09", "App10" }));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App06", "App07", "App08", "App09", "App10" })
            );
        }

        [Fact]
        public void GetApplicationListReturnsExpectedApplicationSpecificConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetApplicationList)}\ReturnsExpectedApplicationSpecificConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.APP03CONNECTOBJECT, visioDocument);
            Shape fromShape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);

            //act
            var result = helper.GetApplicationList(connector, new ShapeBag(fromShape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App03" })
            );
        }

        [Fact]
        public void GetApplicationNameReturnsApplicationNameFromApplicationSpecificConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetApplicationName)}\ReturnsApplicationNameFromApplicationSpecificConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.APP03CONNECTOBJECT, visioDocument);

            //act
            var result = helper.GetApplicationName(connector);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal("App03", result);
        }

        [Fact]
        public void GetApplicationnameThrowsForInvalidMasterName()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetApplicationName("xyz"));

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{2FADB011-5D91-415D-B698-F62A4C96A8BE}"),
                exception.Message
            );
        }

        [Theory]
        [InlineData(UniversalMasterName.APP01CONNECTOBJECT, "App01")]
        [InlineData(UniversalMasterName.APP02CONNECTOBJECT, "App02")]
        [InlineData(UniversalMasterName.APP03CONNECTOBJECT, "App03")]
        [InlineData(UniversalMasterName.APP04CONNECTOBJECT, "App04")]
        [InlineData(UniversalMasterName.APP05CONNECTOBJECT, "App05")]
        [InlineData(UniversalMasterName.APP06CONNECTOBJECT, "App06")]
        [InlineData(UniversalMasterName.APP07CONNECTOBJECT, "App07")]
        [InlineData(UniversalMasterName.APP08CONNECTOBJECT, "App08")]
        [InlineData(UniversalMasterName.APP09CONNECTOBJECT, "App09")]
        [InlineData(UniversalMasterName.APP10CONNECTOBJECT, "App10")]
        public void GetApplicationNameReturnsApplicationNameFromMasterName(string masterNameU, string expectedApplicationsName)
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();

            //act
            var result = helper.GetApplicationName(masterNameU);

            //assert
            Assert.Equal(expectedApplicationsName, result);
        }

        [Fact]
        public void GetFromShapeThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetFromShape)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetFromShape(connector));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1C081184-8203-433C-8985-5AD5DF2C312A}"),
                exception.Message
            );
        }

        [Fact]
        public void GetFromShapeThrowsIfNoFromShapeIsConnected()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetFromShape)}\ThrowsIfNoFromShapeIsConnected.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetFromShape(connector));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{226DB763-8E68-4431-B778-9B11DFBA0B3E}"),
                exception.Message
            );
        }

        [Fact]
        public void GetFromShapeReturnsExpectedFromShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetFromShape)}\ReturnsExpectedFromShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var result = helper.GetFromShape(connector);
            string resultMasterName = result.Master.NameU;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                UniversalMasterName.ACTION,
                resultMasterName
            );
        }

        [Fact]
        public void GetMultipleChoiceConnectorDataThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetMultipleChoiceConnectorData)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetMultipleChoiceConnectorData(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{10D222F7-DB67-41B9-B377-7E5986345BEF}"),
                exception.Message
            );
        }

        [Fact]
        public void GetMultipleChoiceConnectorDataThrowsForINoDialogFunctions()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetMultipleChoiceConnectorData)}\ThrowsForINoDialogFunctions.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetMultipleChoiceConnectorData(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{7A753DBD-E4E4-4ED2-8C8B-C2B5C0B9FDEB}"),
                exception.Message
            );
        }

        [Fact]
        public void GetMultipleChoiceConnectorDataReturnsListOfConnectorDataForValidShapes()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetMultipleChoiceConnectorData)}\ReturnsListOfConnectorDataForValidShapes.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var result = helper.GetMultipleChoiceConnectorData(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                2,
                result.Count
            );
        }

        [Fact]
        public void GetOtherApplicationsThrowsIfNotConnectedToMergeObject()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOtherApplications)}\ThrowsIfNotConnectedToMergeObject.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.APP03CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOtherApplications(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{07850355-EFFF-4469-85A3-D923DF1B5F63}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOtherApplicationsReturnsOtherApplicationsUsingFromMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOtherApplications)}\ReturnsOtherApplicationsUsingFromMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.OTHERSCONNECTOBJECT, visioDocument);

            //act
            var result = helper.GetOtherApplications(shape);
            CloseVisioDocument(visioDocument);

            //assert
            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App01", "App02", "App03", "App04", "App05", })
            );
        }

        [Fact]
        public void GetOtherApplicationsReturnsOtherApplicationsUsingToMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOtherApplications)}\ReturnsOtherApplicationsUsingToMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.OTHERSCONNECTOBJECT, visioDocument);

            //act
            var result = helper.GetOtherApplications(shape);
            CloseVisioDocument(visioDocument);

            //assert
            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App01", "App02", "App03", "App04", "App05", })
            );
        }

        [Fact]
        public void GetOtherApplicationsListThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOtherApplicationsList)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOtherApplicationsList(shape, null!));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{FFFA3C7A-1B02-4BD0-AD46-B7BED83D1477}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOtherApplicationsListReturnsApplicationsUsingOtherFromMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOtherApplicationsList)}\ReturnsApplicationsUsingOtherFromMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.OTHERSCONNECTOBJECT, visioDocument);
            Shape fromShape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var result = helper.GetOtherApplicationsList(connector, new ShapeBag(fromShape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App01", "App02", "App03", "App04", "App05", })
            );
        }

        [Fact]
        public void GetOtherApplicationsListReturnsApplicationsUsingOtherFromShapeBag()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOtherApplicationsList)}\ReturnsApplicationsUsingOtherFromShapeBag.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape connector = GetOnlyShape(UniversalMasterName.OTHERSCONNECTOBJECT, visioDocument);
            Shape fromShape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);

            //act
            var result = helper.GetOtherApplicationsList(connector, new ShapeBag(fromShape, new string[] { "App06", "App07", "App08", "App09", "App10" }));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App06", "App07", "App08", "App09", "App10" })
            );
        }

        [Fact]
        public void GetOutgoingBlankConnectorThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingBlankConnector)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONNECTOBJECT, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOutgoingBlankConnector(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{05313042-7AE9-40D2-B7DD-EEC98D06E74C}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOutgoingBlankConnectorThrowsForNoBlankConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingBlankConnector)}\ThrowsForNoBlankConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOutgoingBlankConnector(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{B11075A7-465F-418E-96BA-E5A1F833E922}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOutgoingBlankConnectorReturnsTheBlankConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingBlankConnector)}\ReturnsTheBlankConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var connector = helper.GetOutgoingBlankConnector(shape);
            var connectorFromShapeMaster = helper.GetFromShape(connector).Master.NameU;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                UniversalMasterName.DIALOG,
                connectorFromShapeMaster
            );
        }

        [Fact]
        public void GetOutgoingBlankConnectorsThrowsForInvalidFromShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingBlankConnectors)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.DIALOG, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOutgoingBlankConnectors(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{D2E0F01D-5D0A-4107-A10F-E5BEBF64EDA9}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOutgoingBlankConnectorReturnsTheListOfBlankConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingBlankConnectors)}\ReturnsTheListOfBlankConnectors.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var connectors = helper.GetOutgoingBlankConnectors(shape).Select(s => s.Master.NameU).ToArray();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                connectors.ToHashSet().SetEquals
                (
                    new string[] 
                    { 
                        UniversalMasterName.APP06CONNECTOBJECT,
                        UniversalMasterName.APP07CONNECTOBJECT,
                        UniversalMasterName.APP08CONNECTOBJECT,
                        UniversalMasterName.APP09CONNECTOBJECT,
                        UniversalMasterName.APP10CONNECTOBJECT,
                        UniversalMasterName.OTHERSCONNECTOBJECT
                    }
                )
            );
        }

        [Fact]
        public void GetOutgoingNoConnectorThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingNoConnector)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOutgoingNoConnector(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{7613E57D-D39E-46FF-BDDB-9B2A83ED391D}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOutgoingNoConnectorReturnsNullForMissingNoConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingNoConnector)}\ReturnsNullForMissingNoConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONDITIONOBJECT, visioDocument);

            //act
            var result = helper.GetOutgoingNoConnector(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public void GetOutgoingNoConnectorReturnsTheNoConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingNoConnector)}\ReturnsTheNoConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONDITIONOBJECT, visioDocument);

            //act
            var result = helper.GetOutgoingNoConnector(shape);
            string connectorText = result!.Text;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.decisionConnectorNoText, connectorText);
        }

        [Fact]
        public void GetOutgoingYesConnectorThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingYesConnector)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetOutgoingYesConnector(shape));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{5BAB7A7D-4EB9-40AA-84A4-1468E1D6F52C}"),
                exception.Message
            );
        }

        [Fact]
        public void GetOutgoingYesConnectorReturnsNullForMissingYesConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingYesConnector)}\ReturnsNullForMissingYesConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONDITIONOBJECT, visioDocument);

            //act
            var result = helper.GetOutgoingYesConnector(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public void GetOutgoingYesConnectorReturnsTheYesConnector()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetOutgoingYesConnector)}\ReturnsTheYesConnector.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.CONDITIONOBJECT, visioDocument);

            //act
            var result = helper.GetOutgoingYesConnector(shape);
            string connectorText = result!.Text;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.decisionConnectorYesText, connectorText);
        }

        [Fact]
        public void GetUnusedApplicationsThrowsForInvalidShape()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetUnusedApplications)}\ThrowsForInvalidShape.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.BEGINFLOW, visioDocument);

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetUnusedApplications(shape, true));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{88AB85BA-5397-4745-92E2-68723F95A04B}"),
                exception.Message
            );
        }

        [Fact]
        public void GetUnusedApplicationsReturnsEmptyArrayForOtherConnectorUsingFromMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetUnusedApplications)}\ReturnsEmptyArrayForOtherConnectorUsingFromMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var result = helper.GetUnusedApplications(shape, true);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetUnusedApplicationsReturnsEmptyArrayForOtherConnectorUsingToMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetUnusedApplications)}\ReturnsEmptyArrayForOtherConnectorUsingToMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var result = helper.GetUnusedApplications(shape, false);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetUnusedApplicationsReturnsUnusedApplicationsUsingFromMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetUnusedApplications)}\ReturnsUnusedApplicationsUsingFromMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var result = helper.GetUnusedApplications(shape, true);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App01", "App02" })
            );
        }

        [Fact]
        public void GetUnusedApplicationsReturnsUnusedApplicationsUsingToMerge()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.GetUnusedApplications)}\ReturnsUnusedApplicationsUsingToMerge.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.MERGEOBJECT, visioDocument);

            //act
            var result = helper.GetUnusedApplications(shape, false);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.True
            (
                result.SequenceEqual(new string[] { "App01", "App02" })
            );
        }

        [Theory]
        [InlineData(UniversalMasterName.ACTION, false)]
        [InlineData(UniversalMasterName.MERGEOBJECT, false)]
        [InlineData(UniversalMasterName.MODULE, true)]
        public void HasAllApplicationConnectorsReturnsExpectedResult(string masterName, bool allConnectorsApplication)
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.HasAllApplicationConnectors)}\Diagram.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(masterName, visioDocument);

            //act
            var result = helper.HasAllApplicationConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(allConnectorsApplication, result);
        }

        [Theory]
        [InlineData(UniversalMasterName.ACTION, true)]
        [InlineData(UniversalMasterName.MERGEOBJECT, false)]
        [InlineData(UniversalMasterName.MODULE, false)]
        public void HasAllNonApplicationConnectorsReturnsExpectedResult(string masterName, bool allConnectorsNonApplication)
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.HasAllNonApplicationConnectors)}\Diagram.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(masterName, visioDocument);

            //act
            var result = helper.HasAllNonApplicationConnectors(shape);
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(allConnectorsNonApplication, result);
        }

        private static void CloseVisioDocument(Document visioDocument)
        {
            visioDocument.Saved = true;
            visioDocument.Close();
        }

        private static Shape GetOnlyShape(string masterName, Document document)
        {
            return document.Pages
                .OfType<Page>()
                .Single()
                .Shapes
                .OfType<Shape>()
                .Single(s => s.Master.NameU == masterName);
        }
    }

    public class ShapeHelperFixture : IDisposable
    {
        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;

        public ShapeHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"NotImportant",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    ),
                    ["app02"] = new Application
                    (
                        "App02",
                        "App02",
                        "Contoso.Test.Flow.dll",
                        $@"NotImportant",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["DisplayEditForm"] = new Function
                    (
                        "DisplayEditForm",
                        "DisplayEditForm",
                        FunctionCategories.DialogForm,
                        "",
                        "flowManager.DialogFunctions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetObjectParameter
                            (
                                "setting",
                                false,
                                "",
                                "Contoso.Forms.Parameters.DataForm.DataFormSettingsParameters",
                                true,
                                false,
                                true
                             )
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        "",
                        ContextProvider
                    ),
                    ["SetupNavigationMenu"] = new Function
                    (
                        "SetupNavigationMenu",
                        "UpdateNavigationBar",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.Actions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetObjectParameter
                            (
                                "navBar",
                                false,
                                "",
                                "Contoso.Forms.Parameters.Navigation.NavigationBarParameters",
                                true,
                                false,
                                true
                             )
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        "",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            VisioApplication = new InvisibleApp();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach(Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }
    }
}
