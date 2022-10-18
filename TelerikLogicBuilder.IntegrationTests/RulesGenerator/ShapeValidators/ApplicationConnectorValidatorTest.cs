using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
        public void ApplicationConnectorValidationSucceeds()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(ApplicationConnectorValidationSucceeds));
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
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public void FailsValidationIfConnectedToLessThanTwoShapes()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfConnectedToLessThanTwoShapes));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            Shape shape = GetOnlyShape
            (
                visioDocument,
                s => s.Master.NameU == UniversalMasterName.APP02CONNECTOBJECT
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.connectorRequires2Shapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfLessThanTwoApplicationsConfigured()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfLessThanTwoApplicationsConfigured));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
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
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            _fixture.ConfigurationService.ProjectProperties = ConfiguredWithTwoApplications;
            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.twoApplicationsMinimum, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfBothEndsAreConnectedToTheSameShape()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfBothEndsAreConnectedToTheSameShape));
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
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.ACTION;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.shapeConnectedToBothEnds, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfToShapeIsNotPermitted()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfToShapeIsNotPermitted));
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
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape toShape = shapeHelper.GetToShape(s);
                    return toShape.Master.NameU == UniversalMasterName.ENDFLOW;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.validApplicationSpecificShapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfFromShapeIsNotPermitted()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfFromShapeIsNotPermitted));
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
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.JUMPOBJECT;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.validApplicationSpecificShapes, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfTheCorrespondingApplicationIsNotConfigued()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfTheCorrespondingApplicationIsNotConfigued));
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
                    if (s.Master.NameU != UniversalMasterName.APP03CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.applicationNotConfigured, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfTheCurrentModuleIsExcludedForTheApplication()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfTheCurrentModuleIsExcludedForTheApplication));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
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
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

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
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfToModuleShapeHasNoData));
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
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape toShape = shapeHelper.GetToShape(s);
                    return toShape.Master.NameU == UniversalMasterName.MODULE;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.moduleShapeDataRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfFromModuleShapeHasNoData()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfFromModuleShapeHasNoData));
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
                    if (s.Master.NameU != UniversalMasterName.APP02CONNECTOBJECT)
                        return false;

                    Shape fromShape = shapeHelper.GetFromShape(s);
                    return fromShape.Master.NameU == UniversalMasterName.MODULE;
                }
            );
            List<ResultMessage> errors = new();
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.moduleShapeDataRequired, errors.First().Message);
        }

        [Fact]
        public void FailsValidationIfToExternalModuleIsExcluded()
        {
            //arrange
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfToExternalModuleIsExcluded));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
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
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

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
            IShapeValidatorFactory validatorFactory = _fixture.ServiceProvider.GetRequiredService<IShapeValidatorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationIfFromExternalModuleIsExcluded));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
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
            var applicationTypeInfo = _fixture.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>().GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);

            //act
            validatorFactory.GetConnectorValidator
            (
                sourceFile,
                GetPage(visioDocument),
                shape,
                errors,
                applicationTypeInfo
            ).Validate();

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
                var configurationItemFactory = _fixture.ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
                return configurationItemFactory.GetProjectProperties
                (
                    "Contoso",
                    @"C:\ProjectPath",
                    new Dictionary<string, Application>
                    {
                        ["app01"] = configurationItemFactory.GetApplication
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
                            configurationItemFactory.GetWebApiDeployment("", "", "", "")
                        )
                    },
                    new HashSet<string>()
                );
            }
        }

        private ProjectProperties ConfiguredWithTwoApplications
        {
            get
            {
                var configurationItemFactory = _fixture.ServiceProvider.GetRequiredService<IConfigurationItemFactory>();
                return configurationItemFactory.GetProjectProperties
                (
                    "Contoso",
                    @"C:\ProjectPath",
                    new Dictionary<string, Application>
                    {
                        ["app01"] = configurationItemFactory.GetApplication
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
                            configurationItemFactory.GetWebApiDeployment("", "", "", "")
                        ),
                        ["app02"] = configurationItemFactory.GetApplication
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
                            configurationItemFactory.GetWebApiDeployment("", "", "", "")
                        )
                    },
                    new HashSet<string>()
                );
            }
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(ApplicationConnectorValidatorTest)}\{fileNameNoExtension}.vsdx");

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
