using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
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
    public class ApplicationSpecificFlowShapeValidatorTest : IClassFixture<ApplicationSpecificFlowShapeValidatorFixture>
    {
        private readonly ApplicationSpecificFlowShapeValidatorFixture _fixture;

        public ApplicationSpecificFlowShapeValidatorTest(ApplicationSpecificFlowShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ApplicationSpecificFlowShapeValidationSucceeds()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(ApplicationSpecificFlowShapeValidationSucceeds));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationForDuplicateOutgoingConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForDuplicateOutgoingConnector));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.duplicateOutgoingConnector, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForDuplicateIncomingConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForDuplicateIncomingConnector));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.duplicateIncomingConnector, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForIncomingCountNotEqualToOutgoingCount()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForIncomingCountNotEqualToOutgoingCount));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.applicationConnectorMismatch, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForMasterMismatchBetweenIncomingAndOutgoing()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMasterMismatchBetweenIncomingAndOutgoing));
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.applicationConnectorMismatch, errors.First().Message);
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(ApplicationSpecificFlowShapeValidatorTest)}\{fileNameNoExtension}.vsdx");

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

        private static Page GetPage(Document document)
        {
            return document.Pages
                .OfType<Page>()
                .Single();
        }
    }

    public class ApplicationSpecificFlowShapeValidatorFixture : IDisposable
    {
        public ApplicationSpecificFlowShapeValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            ConfigurationItemFactory = ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = ConfigurationItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ConfigurationItemFactory.GetApplication
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
                        ConfigurationItemFactory.GetWebApiDeployment("", "", "", "")
                    )
                },
                new HashSet<string>()
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
        internal IConfigurationItemFactory ConfigurationItemFactory;
        internal IConfigurationService ConfigurationService;
        internal IContextProvider ContextProvider;
    }
}
