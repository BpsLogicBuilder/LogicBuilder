using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
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
    public class ApplicationSpecificFlowShapeValidatorTest : IClassFixture<ApplicationSpecificFlowShapeValidatorFixture>
    {
        private readonly ApplicationSpecificFlowShapeValidatorFixture _fixture;

        public ApplicationSpecificFlowShapeValidatorTest(ApplicationSpecificFlowShapeValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateApplicationSpecificFlowShapeValidator()
        {
            //arrange
            IApplicationSpecificFlowShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationSpecificFlowShapeValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void ApplicationSpecificFlowShapeValidationSucceeds()
        {
            //arrange
            IApplicationSpecificFlowShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationSpecificFlowShapeValidator>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(ApplicationSpecificFlowShapeValidationSucceeds)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(ApplicationSpecificFlowShapeValidationSucceeds)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationForDuplicateOutgoingConnector()
        {
            //arrange
            IApplicationSpecificFlowShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationSpecificFlowShapeValidator>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForDuplicateOutgoingConnector)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForDuplicateOutgoingConnector)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.duplicateOutgoingConnector, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForDuplicateIncommingConnector()
        {
            //arrange
            IApplicationSpecificFlowShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationSpecificFlowShapeValidator>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForDuplicateIncommingConnector)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForDuplicateIncommingConnector)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.duplicateIncomingConnector, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForIncomingCountNotEqualToOutgoingCount()
        {
            //arrange
            IApplicationSpecificFlowShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationSpecificFlowShapeValidator>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForIncomingCountNotEqualToOutgoingCount)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForIncomingCountNotEqualToOutgoingCount)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.applicationConnectorMismatch, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForMasterMismatchBetweenIncomingAndOutgoing()
        {
            //arrange
            IApplicationSpecificFlowShapeValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationSpecificFlowShapeValidator>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForMasterMismatchBetweenIncomingAndOutgoing)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape(UniversalMasterName.ACTION, visioDocument);
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationSpecificFlowShapeValidatorTest\{nameof(FailsValidationForMasterMismatchBetweenIncomingAndOutgoing)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.applicationConnectorMismatch, errors.First().Message);
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
