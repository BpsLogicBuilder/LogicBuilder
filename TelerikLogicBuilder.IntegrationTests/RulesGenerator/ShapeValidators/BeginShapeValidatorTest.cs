﻿using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
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
        public void BeginShapeValidationSucceeds()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(BeginShapeValidationSucceeds));
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
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationForMultipleOutgoingConnectors()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMultipleOutgoingConnectors));
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
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.beginShapeOutgoingRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationForIncomingConnector()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForIncomingConnector));
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
                    return s.Master.NameU == UniversalMasterName.BEGINFLOW;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            rulesGeneratorFactory.GetShapeValidator
            (
                sourceFile,
                GetPage(visioDocument),
                new ShapeBag(shape),
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.beginShapeIncoming, errors.First().Message);
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(BeginShapeValidatorTest)}\{fileNameNoExtension}.vsdx");

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
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            ConfigurationService.ProjectProperties = ProjectPropertiesItemFactory.GetProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = ProjectPropertiesItemFactory.GetApplication
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
                    ),
                    ["app02"] = ProjectPropertiesItemFactory.GetApplication
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
                        WebApiDeploymentItemFactory.GetWebApiDeployment("", "", "", "")
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
        internal IProjectPropertiesItemFactory ProjectPropertiesItemFactory;
		internal IWebApiDeploymentItemFactory WebApiDeploymentItemFactory;
        internal IConfigurationService ConfigurationService;
    }
}
