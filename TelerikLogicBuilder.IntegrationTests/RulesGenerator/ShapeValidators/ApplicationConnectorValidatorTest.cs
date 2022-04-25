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
using System.Globalization;
using System.Linq;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator.ShapeValidators
{
    public class ApplicationConnectorValidatorTest : IClassFixture<ApplicationConnectorValidatorFixture>
    {
        private readonly ApplicationConnectorValidatorFixture _fixture;

        public ApplicationConnectorValidatorTest(ApplicationConnectorValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateApplicationConnectorValidator()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();

            //assert
            Assert.NotNull(validator);
        }

        [Fact]
        public void ApplicationConnectorValidationSucceeds()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(ApplicationConnectorValidationSucceeds)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s => 
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(ApplicationConnectorValidationSucceeds)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationIfConnectedToLessThanTwoShapes()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfConnectedToLessThanTwoShapes)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s => s.Master.NameU == UniversalMasterName.APP02CONNECTOBJECT
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfConnectedToLessThanTwoShapes)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.connectorRequires2Shapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfLessThanTwoApplicationsConfigured()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfLessThanTwoApplicationsConfigured)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithOneApplication;
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfLessThanTwoApplicationsConfigured)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithTwoApplications;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.twoApplicationsMinimum, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfBothEndsAreConnectedToTheSameShape()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfBothEndsAreConnectedToTheSameShape)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.ACTION;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfBothEndsAreConnectedToTheSameShape)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.shapeConnectedToBothEnds, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfToShapeIsNotPermitted()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfToShapeIsNotPermitted)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape toShape = shapeHelper.GetToShape(s);
                    return toShape.Master.NameU == UniversalMasterName.ENDFLOW;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfToShapeIsNotPermitted)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.validApplicationSpecificShapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfFromShapeIsNotPermitted()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfFromShapeIsNotPermitted)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.JUMPOBJECT;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfFromShapeIsNotPermitted)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.validApplicationSpecificShapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfTheCorrespondingApplicationIsNotConfigued()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfTheCorrespondingApplicationIsNotConfigued)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP03CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfTheCorrespondingApplicationIsNotConfigued)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.applicationNotConfigured, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfTheCurrentModuleIsExcludedForTheApplication()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfTheCurrentModuleIsExcludedForTheApplication)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            _fixture.ConfigurationService.ProjectProperties.ApplicationList["app02"].ExcludedModules.Add(nameof(FailsValidationIfTheCurrentModuleIsExcludedForTheApplication).ToLowerInvariant());

            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfTheCurrentModuleIsExcludedForTheApplication)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            _fixture.ConfigurationService.ProjectProperties.ApplicationList["app02"].ExcludedModules.Clear();
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture, 
                    Strings.moduleIsExcludedFormat,
                    nameof(FailsValidationIfTheCurrentModuleIsExcludedForTheApplication).ToLowerInvariant(), 
                    "App02"
                ), 
                errors.First().Message
            );
        }

        [Fact]
        public void FailsValidationIfToModuleShapeHasNoData()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfToModuleShapeHasNoData)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape toShape = shapeHelper.GetToShape(s);
                    return toShape.Master.NameU == UniversalMasterName.MODULE;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfToModuleShapeHasNoData)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.moduleShapeDataRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfFromModuleShapeHasNoData()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfFromModuleShapeHasNoData)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MODULE;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfFromModuleShapeHasNoData)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.moduleShapeDataRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfToExternalModuleIsExcluded()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfToExternalModuleIsExcluded)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            _fixture.ConfigurationService.ProjectProperties.ApplicationList["app02"].ExcludedModules.Add("validatecourse");
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape toShape = shapeHelper.GetToShape(s);
                    return toShape.Master.NameU == UniversalMasterName.MODULE;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfToExternalModuleIsExcluded)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            _fixture.ConfigurationService.ProjectProperties.ApplicationList["app02"].ExcludedModules.Clear();
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.moduleIsExcludedFormat,
                    "validatecourse",
                    "App02"
                ),
                errors.First().Message
            );
        }

        [Fact]
        public void FailsValidationIfFromExternalModuleIsExcluded()
        {
            //arrange
            IApplicationConnectorValidator validator = _fixture.ServiceProvider.GetRequiredService<IApplicationConnectorValidator>();
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfFromExternalModuleIsExcluded)}.vsdx"),
                (short)VisOpenSaveArgs.visOpenCopy
            );
            _fixture.ConfigurationService.ProjectProperties.ApplicationList["app02"].ExcludedModules.Add("validatecourse");
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s =>
                {
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MODULE;
                }
            );
            List<ResultMessage> errors = new();

            //act
            validator.Validate
            (
                @$"C:\TelerikLogicBuilder\TelerikLogicBuilder.IntegrationTests\Diagrams\ApplicationConnectorValidatorTest\{nameof(FailsValidationIfFromExternalModuleIsExcluded)}.vsdx",
                GetPage(visioDocument),
                shape,
                errors
            );

            _fixture.ConfigurationService.ProjectProperties.ApplicationList["app02"].ExcludedModules.Clear();
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.moduleIsExcludedFormat,
                    "validatecourse",
                    "App02"
                ),
                errors.First().Message
            );
        }

        private ProjectProperties ConfiguredWithOneApplication
        {
            get
            {
                var contextProvider = _fixture.ServiceProvider.GetRequiredService<IContextProvider>();
                return new ProjectProperties
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
                            new WebApiDeployment("", "", "", "", contextProvider),
                            contextProvider
                        )
                    },
                    new HashSet<string>(),
                    contextProvider
                );
            }
        }

        private ProjectProperties ConfiguredWithTwoApplications
        {
            get
            {
                var contextProvider = _fixture.ServiceProvider.GetRequiredService<IContextProvider>();
                return new ProjectProperties
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
                            new WebApiDeployment("", "", "", "", contextProvider),
                            contextProvider
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
                            new WebApiDeployment("", "", "", "", contextProvider),
                            contextProvider
                        )
                    },
                    new HashSet<string>(),
                    contextProvider
                );
            }
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

    public class ApplicationConnectorValidatorFixture : IDisposable
    {
        public ApplicationConnectorValidatorFixture()
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
