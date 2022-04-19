using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
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
        public void CanCreateExceptionHelper()
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
        public void CountIncomingConnectorsReturnsThreeWithThreeIncommingConnectors()
        {
            //arrange
            IShapeHelper helper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ShapeHelperTest\{nameof(IShapeHelper.CountIncomingConnectors)}\ReturnsThreeWithThreeIncommingConnectors.vsdx"),
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
        public void GetApplicationListThrowsIfShapeBagsOtherConnectorApplicationsIsNull()
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
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => helper.GetApplicationList(connector, new ShapeBag(fromShape)));
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{7A61589E-4F94-4513-BBC7-11C8B7994445}"),
                exception.Message
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

        private void CloseVisioDocument(Document visioDocument)
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

        public ShapeHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
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
                            new ObjectParameter
                            (
                                "setting",
                                false,
                                "",
                                "Contoso.Forms.Parameters.DataForm.DataFormSettingsParameters",
                                true,
                                false,
                                true,
                                ContextProvider
                             )
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
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
                            new ObjectParameter
                            (
                                "navBar",
                                false,
                                "",
                                "Contoso.Forms.Parameters.Navigation.NavigationBarParameters",
                                true,
                                false,
                                true,
                                ContextProvider
                             )
                        },
                        new List<string> { },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
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
