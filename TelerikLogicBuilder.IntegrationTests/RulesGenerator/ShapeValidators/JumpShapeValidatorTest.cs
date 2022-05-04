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
    public class JumpShapeValidatorTest : IClassFixture<JumpShapeValidatorFixture>
    {
        private readonly JumpShapeValidatorFixture _fixture;

        public JumpShapeValidatorTest(JumpShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateJumpShapeValidator()
        {
            //arrange
            IJumpShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IJumpShapeValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void JumpShapeValidationSucceeds()
        {
            //arrange
            IJumpShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IJumpShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(JumpShapeValidationSucceeds));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.JUMPOBJECT 
                        && s.FromConnects.Count > 0 
                        && s.FromConnects[1].FromPart == (short)VisFromParts.visBegin;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationBecauseJumpShapeHasNoData()
        {
            //arrange
            IJumpShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IJumpShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationBecauseJumpShapeHasNoData));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.JUMPOBJECT
                        && s.FromConnects.Count > 0
                        && s.FromConnects[1].FromPart == (short)VisFromParts.visEnd;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.jumpShapeDataRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationBecauseJumpShapeHasNoConnectors()
        {
            //arrange
            IJumpShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IJumpShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationBecauseJumpShapeHasNoConnectors));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.JUMPOBJECT
                        && s.FromConnects.Count == 0;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.noConnectorsOnJumpFormat, errors.First().Message);
        }

        [Fact]
        public void FailsValidationBecauseJumpShapeHasBothIncomingAndOutgoingConnectors()
        {
            //arrange
            IJumpShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IJumpShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationBecauseJumpShapeHasBothIncomingAndOutgoingConnectors));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.JUMPOBJECT
                        && s.FromConnects.Count == 2;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.jumpConnectorsBothDirections, errors.First().Message);
        }

        [Fact]
        public void FailsValidationBecauseJumpShapeHasMoreThanOneOutgoingConnector()
        {
            //arrange
            IJumpShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IJumpShapeValidator>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationBecauseJumpShapeHasMoreThanOneOutgoingConnector));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    return s.Master.NameU == UniversalMasterName.JUMPOBJECT
                        && s.FromConnects.Count == 2 
                        && s.FromConnects[1].FromPart == (short)VisFromParts.visBegin;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.jumpShape1OutGoing, errors.First().Message);
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(JumpShapeValidatorTest)}\{fileNameNoExtension}.vsdx");

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

    public class JumpShapeValidatorFixture : IDisposable
    {
        public JumpShapeValidatorFixture()
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
