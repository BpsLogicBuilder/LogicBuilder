using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator.ShapeValidators
{
    public class BeginShapeValidatorTest : IClassFixture<BeginShapeValidatorFixture>
    {
        private readonly BeginShapeValidatorFixture _fixture;

        public BeginShapeValidatorTest(BeginShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateBeginShapeValidator()
        {
            //arrange
            IBeginShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IBeginShapeValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void BeginShapeValidationSucceeds()
        {
            //arrange
            IBeginShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IBeginShapeValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\BeginShapeValidatorTest\{nameof(BeginShapeValidationSucceeds)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\BeginShapeValidatorTest\{nameof(BeginShapeValidationSucceeds)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationForMultipleOutgoingConnectors()
        {
            //arrange
            IBeginShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IBeginShapeValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\BeginShapeValidatorTest\{nameof(FailsValidationForMultipleOutgoingConnectors)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\BeginShapeValidatorTest\{nameof(FailsValidationForMultipleOutgoingConnectors)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.beginShapeOutgoingRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForIncommingConnector()
        {
            //arrange
            IBeginShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IBeginShapeValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\BeginShapeValidatorTest\{nameof(FailsValidationForIncommingConnector)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\BeginShapeValidatorTest\{nameof(FailsValidationForIncommingConnector)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.beginShapeIncoming, errors.First().Message);
        }

        private static void CloseVisioDocument(Document visioDocument)
        {
            visioDocument.Saved = true;
            visioDocument.Close();
        }

        private static Shape GetOnlyShape(Document document, Func<Shape, bool> selector)
        {
            return document.Pages
                .OfType<Page>()
                .Single()
                .Shapes
                .OfType<Shape>()
                .Single(selector);
        }

        private static Page GetPage(Document document)
        {
            return document.Pages[1];
        }

    }

    public class BeginShapeValidatorFixture : IDisposable
    {
        public BeginShapeValidatorFixture()
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

            VisioApplication = new InvisibleApp();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (Document document in VisioApplication.Documents)
            {
                document.Saved = true;
                document.Close();
            }
            VisioApplication.Quit();
        }

        internal InvisibleApp VisioApplication;
        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
    }
}
