using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application = ABIS.LogicBuilder.FlowBuilder.Configuration.Application;

namespace TelerikLogicBuilder.IntegrationTests.RulesGenerator
{
    public class DiagramValidatorTest : IClassFixture<DiagramValidatorFixture>
    {
        private readonly DiagramValidatorFixture _fixture;

        public DiagramValidatorTest(DiagramValidatorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateDiagramValidatorFactory()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();

            //assert
            Assert.NotNull(rulesGeneratorFactory);
        }

        [Fact]
        public void CreateDiagramValidatorThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => _fixture.ServiceProvider.GetRequiredService<IDiagramValidator>());
        }

        [Fact]
        public async Task DiagramValidationSucceeds()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(DiagramValidationSucceeds));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Empty(errors);
        }

        [Fact]
        public async Task FailsValidationForDuplicateToJumpShapes()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForDuplicateToJumpShapes));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.jumpShapeUniqueTextForToShape, errors.First().Message);
        }

        [Fact]
        public async Task FailsValidationForMissingFromJumpShape()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMissingFromJumpShape));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.jumpShapeNoMatchingFromShape, errors.First().Message);
        }

        [Fact]
        public async Task FailsValidationForMissingToJumpShape()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMissingToJumpShape));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.jumpShapeNoMatchingToShape, errors.First().Message);
        }

        [Fact]
        public async Task FailsValidationForConnectorNotConnectingTwoShapes()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForConnectorNotConnectingTwoShapes));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal(Strings.connectorRequires2Shapes, errors.First().Message);
        }

        [Fact]
        public async Task FailsValidationForMissingBeginShape()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMissingBeginShape));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.beginFlowShapeRequired, $"{nameof(FailsValidationForMissingBeginShape)}.vsdx"), 
                errors.First().Message
            );
        }

        [Fact]
        public async Task FailsValidationForMultipleBeginShapes()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForMultipleBeginShapes));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                string.Format(CultureInfo.CurrentCulture, Strings.beginShapeCount, $"{nameof(FailsValidationForMultipleBeginShapes)}.vsdx"),
                errors.First().Message
            );
        }

        [Fact]
        public async Task FailsValidationForModuleEndInBeginFlowDocument()
        {
            //arrange
            IRulesGeneratorFactory rulesGeneratorFactory = _fixture.ServiceProvider.GetRequiredService<IRulesGeneratorFactory>();
            string sourceFile = GetFullSourceFilePath(nameof(FailsValidationForModuleEndInBeginFlowDocument));
            IShapeHelper shapeHelper = _fixture.ServiceProvider.GetRequiredService<IShapeHelper>();
            var applicationTypeInfo = _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name);
            Document visioDocument = _fixture.VisioApplication.Documents.OpenEx
            (
                sourceFile,
                (short)VisOpenSaveArgs.visOpenCopy
            );
            var progress = new Progress<ProgressMessage>(percent =>
            {
            });
            var cancellationToken = new CancellationTokenSource();

            //act
            IList<ResultMessage> errors = await rulesGeneratorFactory.GetDiagramValidator
            (
                sourceFile,
                visioDocument,
                applicationTypeInfo,
                progress,
                cancellationToken
            ).Validate();

            CloseVisioDocument(visioDocument);

            //assert
            Assert.Equal
            (
                Strings.moduleEndIsInvalidForBeginFlow,
                errors.First().Message
            );
        }

        private static string GetFullSourceFilePath(string fileNameNoExtension)
            => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @$"Diagrams\{nameof(DiagramValidatorTest)}\{fileNameNoExtension}.vsdx");

        private static void CloseVisioDocument(Document visioDocument)
        {
            visioDocument.Saved = true;
            visioDocument.Close();
        }
    }

    public class DiagramValidatorFixture : IDisposable
    {
        public DiagramValidatorFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<IMainWindow>().Instance = new Mocks.MockMdiParent();
            ProjectPropertiesItemFactory = ServiceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
			WebApiDeploymentItemFactory = ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            FunctionFactory = ServiceProvider.GetRequiredService<IFunctionFactory>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            ParameterFactory = ServiceProvider.GetRequiredService<IParameterFactory>();
            ReturnTypeFactory = ServiceProvider.GetRequiredService<IReturnTypeFactory>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
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

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["ClearErrorMessages"] = FunctionFactory.GetFunction
                    (
                        "ClearErrorMessages",
                        "Clear",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.FlowDataCache.Response.ErrorMessages",
                        "Field.Property.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                        ""
                    ),
                    ["WriteToLog"] = FunctionFactory.GetFunction
                    (
                        "WriteToLog",
                        "WriteToLog",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>()
                        {
                            ParameterFactory.GetLiteralParameter
                            (
                                "message",
                                false,
                                "",
                                LiteralParameterType.String,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>()
                             )
                        },
                        new List<string> { },
                        ReturnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                        ""
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
            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
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
        internal IFunctionFactory FunctionFactory;
        internal ILoadContextSponsor LoadContextSponsor;
        internal IParameterFactory ParameterFactory;
        internal IReturnTypeFactory ReturnTypeFactory;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
    }
}
